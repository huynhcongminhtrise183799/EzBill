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
            var tripMembers = await _tripRepository.GetTripMembersAsync(tripId);

            var balances = tripMembers.ToDictionary(m => m.AccountId, m => 0.0);

            foreach (var member in tripMembers)
            {
                if (member.Amount.HasValue && member.AmountRemainInTrip.HasValue)
                {
                    double budgetUsed = member.Amount.Value - member.AmountRemainInTrip.Value;
                    balances[member.AccountId] += budgetUsed;
                }
            }

            foreach (var evt in events)
            {
                if (evt.PaidBy.HasValue)
                {
                    balances[evt.PaidBy.Value] += evt.AmountInTripCurrency; 
                }

                if (evt.Event_Use != null && evt.Event_Use.Any())
                {
                    foreach (var use in evt.Event_Use)
                    {
                        balances[use.AccountId] -= (use.AmountFromGroup ?? 0) + (use.AmountFromPersonal ?? 0);

                        balances[use.AccountId] += use.AmountFromGroup ?? 0;
                    }
                }
            }

            foreach (var refund in taxRefunds)
            {
                if (refund.TaxRefund_Usages != null && refund.TaxRefund_Usages.Any())
                {
                    foreach (var usage in refund.TaxRefund_Usages)
                    {
                        balances[usage.AccountId] += usage.AmountReceived;
                    }
                }
                else
                {
                    balances[refund.RefundedBy] += refund.RefundAmount;
                }
            }

            var settlements = new List<Settlement>();

            var debtors = balances.Where(b => b.Value < 0).ToList();
            var creditors = balances.Where(b => b.Value > 0).ToList();

            foreach (var debtor in debtors)
            {
                double owes = -debtor.Value;

                foreach (var creditor in creditors.Where(c => c.Value > 0))
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
            var result = await _settlementRepository.GetByDebtorIdAsync(debtorId);
            return result ?? new List<Settlement>();
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
