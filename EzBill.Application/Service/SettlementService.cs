using EzBill.Application.DTO.Settlement;
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

        public SettlementService(
            ISettlementRepository settlementRepository,
            IEventRepository eventRepository,
            ITaxRefundRepository taxRefundRepository,
            IAccountRepository accountRepository,
            ITripRepository tripRepository)
        {
            _settlementRepository = settlementRepository;
            _eventRepository = eventRepository;
            _taxRefundRepository = taxRefundRepository;
            _accountRepository = accountRepository;
            _tripRepository = tripRepository;
        }

        public async Task<List<SettlementResultDto>> GenerateSettlementsAsync(Guid tripId)
        {
            var events = await _eventRepository.GetByTripIdAsync(tripId);
            var taxRefunds = await _taxRefundRepository.GetByTripIdAsync(tripId);
            var balances = new Dictionary<Guid, double>();

            foreach (var evt in events)
            {
				if (evt.PaidBy.HasValue)
				{
					balances.TryAdd(evt.PaidBy.Value, 0);
					balances[evt.PaidBy.Value] += evt.AmountInTripCurrency;
				}

				foreach (var use in evt.Event_Use)
                {
                    balances.TryAdd(use.AccountId, 0);
                    balances[use.AccountId] -= use.AmountFromGroup ?? 0;
                }
            }

            foreach (var refund in taxRefunds)
            {
                balances.TryAdd(refund.RefundedBy, 0);
                balances[refund.RefundedBy] -= refund.RefundAmount;

                foreach (var usage in refund.TaxRefund_Usages)
                {
                    balances.TryAdd(usage.AccountId, 0);
                    balances[usage.AccountId] += usage.AmountReceived;
                }
            }

            var settlements = new List<Settlement>();
            foreach (var debtor in balances.Where(b => b.Value < 0))
            {
                double owes = -debtor.Value;

                foreach (var creditor in balances.Where(b => b.Value > 0))
                {
                    if (owes <= 0) break;

                    double canReceive = creditor.Value;
                    double amount = Math.Min(owes, canReceive);

                    settlements.Add(new Settlement
                    {
                        SettlementId = Guid.NewGuid(),
                        TripId = tripId,
                        FromAccountId = debtor.Key,
                        ToAccountId = creditor.Key,
                        Amount = amount,
                        Status = SettlementStatus.UNPAID.ToString()
                    });

                    owes -= amount;
                    balances[creditor.Key] -= amount;
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
            var result =  await _settlementRepository.GetByDebtorIdAsync(debtorId);
            if(result == null)
            {
                return new List<Settlement>();
            }
            return result;
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
            if (!settlements.Any())
                return new List<SettlementResultDto>();

            var trip = await _tripRepository.GetByIdAsync(settlements.First().TripId);
            var accountIds = settlements.Select(s => s.FromAccountId)
                                        .Concat(settlements.Select(s => s.ToAccountId))
                                        .Distinct()
                                        .ToList();

            var accounts = await _accountRepository.GetByIdsAsync(accountIds); 

            var result = settlements.Select(s =>
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

            return result;
        }
    }
}
