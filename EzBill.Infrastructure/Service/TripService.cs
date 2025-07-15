using EzBill.Application.DTO.TaxRefund;
using EzBill.Application.DTO.Trip;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using EzBill.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Service
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _repo;

        public TripService(ITripRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> AddTrip(Trip trip)
        {
            return await _repo.AddTrip(trip);
        }
        public async Task<List<TripDto>> GetTripsForAccountAsync(Guid accountId)
        {
            var trips = await _repo.GetTripsByAccountIdAsync(accountId);

            var tripDtos = trips.Select(t => new TripDto
            {
                TripName = t.TripName,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Budget = t.Budget, 
                Members = t.TripMembers.Select(m => new TripMemberDto
                {
                    AccountId = m.AccountId,
                    Email = m.Account.Email, 
                    Status = m.Status
                }).ToList()
            }).ToList();

            return tripDtos;
        }
        public async Task<TripDetailsDto> GetTripDetailsAsync(Guid tripId)
        {
            var trip = await _repo.GetTripDetailsByIdAsync(tripId);
            if (trip == null) return null;

            var totalEventAmount = trip.Events?.Sum(e => e.AmountInTripCurrency) ?? 0;
            var totalTaxRefundAmount = trip.TaxRefunds?.Sum(tr => tr.RefundAmount) ?? 0;
            var totalUsedAmount = totalEventAmount + totalTaxRefundAmount;

            var eventContributions = trip.Events
                .GroupBy(e => e.PaidBy)
                .Select(g => new EventContributionDto
                {
                    AccountId = g.Key,
                    Email = trip.TripMembers.FirstOrDefault(m => m.AccountId == g.Key)?.Account?.Email ?? "",
                    PaidAmount = g.Sum(e => e.AmountInTripCurrency)
                }).ToList();

            var taxRefunds = trip.TaxRefunds.Select(tr => new Application.DTO.Trip.TaxRefundDto
            {
                Message = "Hoàn thuế",
                ProductName = tr.ProductName,
                OriginalAmount = $"{tr.OriginalAmount:N0}₫",
                RefundPercent = $"{tr.RefundPercent}%",
                RefundAmount = $"{tr.RefundAmount:N0}₫",
                SplitType = tr.SplitType,
                Beneficiaries = tr.TaxRefund_Usages.Select(u => new TaxRefundBeneficiaryDto
                {
                    AccountId = u.AccountId,
                    Ratio = u.Ratio.HasValue ? $"{u.Ratio.Value * 100:N2}%" : "0%",
                    AmountReceived = $"{u.AmountReceived:N0}₫"
                }).ToList()
            }).ToList();

            return new TripDetailsDto
            {
                TripName = trip.TripName,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                Budget = trip.Budget,
                TotalEventAmount = totalEventAmount,
                TotalTaxRefundAmount = totalTaxRefundAmount,
                TotalUsedAmount = totalUsedAmount,
                EventContributions = eventContributions,
                TaxRefunds = taxRefunds
            };
        }


    }
}
