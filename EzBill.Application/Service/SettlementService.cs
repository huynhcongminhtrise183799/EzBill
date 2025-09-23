using EzBill.Application.DTO.Settlement;
using EzBill.Application.Exceptions;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
    public class SettlementService : ISettlementService
    {
        private readonly ISettlementRepository _settlementRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITaxRefundRepository _taxRefundRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IFirebaseService _firebaseService;

		public SettlementService(
            ISettlementRepository settlementRepository,
            IEventRepository eventRepository,
            ITaxRefundRepository taxRefundRepository,
            IAccountRepository accountRepository,
            ITripRepository tripRepository,
			IFirebaseService firebaseService
			)
        {
            _settlementRepository = settlementRepository;
            _eventRepository = eventRepository;
            _taxRefundRepository = taxRefundRepository;
            _accountRepository = accountRepository;
            _tripRepository = tripRepository;
			_firebaseService = firebaseService;
		}

        public async Task<bool> ChangeSettlementStatusToPaid(Guid settlementId)
        {
            var result = await _settlementRepository.ChangeSettlementStatus(settlementId, SettlementStatus.PAID.ToString());
            var settlement = await _settlementRepository.GetByIdAsync(settlementId);
            if (settlement == null) throw new AppException("Không tìm thấy settlement", 404);
            if (result)
            {
                var notiResult = await _firebaseService.SendNotiConfirmedAsync(settlement.ToAccountId);
                if (notiResult == null) throw new AppException("Gửi thông báo thất bại", 500);
				return true;
			}
			return false;
		}

		public async Task<List<SettlementResultDto>> GenerateSettlementsAsync(Guid tripId)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId);
            var tripMembers = await _tripRepository.GetTripMembersAsync(tripId);
            var events = await _eventRepository.GetByTripIdAsync(tripId);
            var taxRefunds = await _taxRefundRepository.GetByTripIdAsync(tripId);

            var settlements = new List<Settlement>();

            var memberBudgets = tripMembers.ToDictionary(m => m.AccountId, m => m.AmountRemainInTrip ?? 0);
            var tripOwnerId = trip.CreatedBy;

            foreach (var evt in events)
            {
                if (!evt.PaidBy.HasValue) continue;
                var payerId = evt.PaidBy.Value;

                if (evt.Event_Use != null)
                {
                    foreach (var use in evt.Event_Use)
                    {
                        var userId = use.AccountId;
                        double amountFromGroup = use.AmountFromGroup ?? 0;
                        if (amountFromGroup > 0)
                        {
                            double budgetRemain = memberBudgets[userId];
                            double toSettle = 0;

                            if (userId != payerId)
                                toSettle = Math.Max(amountFromGroup - budgetRemain, 0);

                            memberBudgets[userId] = Math.Max(budgetRemain - amountFromGroup, 0);

                            if (toSettle > 0)
                            {
                                settlements.Add(new Settlement
                                {
                                    SettlementId = Guid.NewGuid(),
                                    TripId = tripId,
                                    FromAccountId = userId,
                                    ToAccountId = payerId,
                                    Amount = toSettle,
                                    Status = SettlementStatus.UNPAID.ToString()
                                });
                            }
                        }

                        if ((use.AmountFromPersonal ?? 0) > 0)
                        {
                            settlements.Add(new Settlement
                            {
                                SettlementId = Guid.NewGuid(),
                                TripId = tripId,
                                FromAccountId = userId,
                                ToAccountId = payerId,
                                Amount = use.AmountFromPersonal.Value,
                                Status = SettlementStatus.UNPAID.ToString()
                            });
                        }
                    }
                }
            }

            foreach (var refund in taxRefunds)
            {
                if (refund.TaxRefund_Usages != null && refund.TaxRefund_Usages.Any())
                {
                    foreach (var usage in refund.TaxRefund_Usages)
                    {
                        settlements.Add(new Settlement
                        {
                            SettlementId = Guid.NewGuid(),
                            TripId = tripId,
                            FromAccountId = refund.RefundedBy,
                            ToAccountId = usage.AccountId,
                            Amount = usage.AmountReceived,
                            Status = SettlementStatus.UNPAID.ToString()
                        });
                    }
                }
            }

            foreach (var member in tripMembers)
            {
                double remainingBudget = memberBudgets[member.AccountId];
                if (remainingBudget <= 0) continue;

                if (member.AccountId != tripOwnerId)
                {
                    settlements.Add(new Settlement
                    {
                        SettlementId = Guid.NewGuid(),
                        TripId = tripId,
                        FromAccountId = tripOwnerId,  
                        ToAccountId = member.AccountId,
                        Amount = remainingBudget,
                        Status = SettlementStatus.UNPAID.ToString()
                    });

                    memberBudgets[member.AccountId] = 0; 
                }
            }

            if (settlements.Any())
            {
                await _settlementRepository.AddRangeAsync(settlements);
                await _settlementRepository.SaveChangesAsync();
            }

            return await MapToDto(settlements);
        }

        public async Task<List<Settlement>?> GetByDebtorIdAsync(Guid debtorId)
        {
            var result = await _settlementRepository.GetUnPaidByDebtorIdAsync(debtorId);
            return result ?? new List<Settlement>();
        }

		public async Task<List<SettlementResultDto>> GetSettlementsByAccountIdWithFiltterAsync(Guid accountId, string state)
		{
            switch (state)
            {
				case "all":
					var allSettlements = await _settlementRepository.GetAllUnPaidSettlementsByAccountId(accountId);
					return await MapToDto(allSettlements);
				case "debt":
					var debtSettlements = await _settlementRepository.GetUnPaidByDebtorIdAsync(accountId);
					return await MapToDto(debtSettlements ?? new List<Settlement>());
				case "credit":
					var creditSettlements = await _settlementRepository.GetUnPaidByCreditorIdAsync(accountId);
					return await MapToDto(creditSettlements ?? new List<Settlement>());
				default:
                    break;
            }
            return null;
        }

		public async Task<List<SettlementResultDto>> GetSettlementsByTripAsync(Guid tripId)
        {
            var settlements = await _settlementRepository.GetByTripIdAsync(tripId);
            return await MapToDto(settlements);
        }

        public async Task UpdateSettlementStatusAsync(Guid settlementId, SettlementStatus status)
        {
            var settlement = await _settlementRepository.GetByIdAsync(settlementId);
            if (settlement == null)
                throw new InvalidOperationException("Settlement không tồn tại.");

            settlement.Status = status.ToString();
            await _settlementRepository.UpdateAsync(settlement);
            await _settlementRepository.SaveChangesAsync();
        }

        private async Task<List<SettlementResultDto>> MapToDto(List<Settlement> settlements)
        {
            if (!settlements.Any()) return new List<SettlementResultDto>();

            var trip = await _tripRepository.GetByIdAsync(settlements.First().TripId);
            var accountIds = settlements.Select(s => s.FromAccountId)
                                        .Concat(settlements.Select(s => s.ToAccountId))
                                        .Distinct()
                                        .ToList();

            var accounts = await _accountRepository.GetByIdsAsync(accountIds);

            return settlements.Select(s =>
            {
                var fromAccount = accounts.FirstOrDefault(a => a.AccountId == s.FromAccountId);
                var toAccount = accounts.FirstOrDefault(a => a.AccountId == s.ToAccountId);

                return new SettlementResultDto
                {
                    SettlementId = s.SettlementId,
                    FromAccountName = fromAccount?.Email ?? "Unknown",
                    ToAccountName = toAccount?.Email ?? "Unknown",
                    Amount = s.Amount,
                    Status = s.Status,
                    TripName = trip?.TripName ?? "Unknown Trip"
                };
            }).ToList();
        }
    }
}
