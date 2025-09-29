using EzBill.Application.DTO.TaxRefund;
using EzBill.Application.DTO.Trip;
using EzBill.Application.Exceptions;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _repo;
        private readonly IAccountSubscriptionsRepository _accountSubscriptionsRepository;
        private readonly IAccountRepository _accountRepository;

		public TripService(ITripRepository repo, IAccountSubscriptionsRepository accountSubscriptionsRepository, IAccountRepository accountRepository)
        {
            _repo = repo;
			_accountSubscriptionsRepository = accountSubscriptionsRepository;
			_accountRepository = accountRepository;
		}

        public async Task<bool> AddTrip(Trip trip)
        {
			var accountSubscription = await _accountSubscriptionsRepository.GetByAccountId(trip.CreatedBy);
			var account = await _accountRepository.GetByIdAsync(trip.CreatedBy);
			if (account == null) throw new AppException("Tài khoản không tồn tại", 404);
			if (accountSubscription == null) throw new AppException("Tài khoản chưa đăng ký gói dịch vụ", 404);
			if (accountSubscription.GroupRemaining < 0) throw new AppException("Hết lượt tạo group. Vui lòng mua gói mới", 400);
            if(accountSubscription.Plan.MaxMembersPerTrip < trip.TripMembers.Count) throw new AppException($"Gói hiện tại chỉ cho phép tối đa {accountSubscription.Plan.MaxMembersPerTrip} thành viên trong một chuyến đi", 400);
			var result =  await _repo.AddTrip(trip);
			if (result)
            {
                accountSubscription.GroupRemaining -= 1;
                if(accountSubscription.GroupRemaining == 0)
                {
                    accountSubscription.Status = SubscriptionStatus.INACTIVE.ToString();
					await _accountSubscriptionsRepository.UpdateSubscriptions(accountSubscription);
					await _accountRepository.UpdateAccountRole(account.AccountId, AccountRole.FREE_USER.ToString());
				}
				return true;
			}
			return false;

		}
        public async Task<List<TripDto>> GetTripsForAccountAsync(Guid accountId)
        {
            var trips = await _repo.GetTripsByAccountIdAsync(accountId);

            var tripDtos = trips.Select(t => new TripDto
            {
                TripId = t.TripId,
                TripName = t.TripName,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Budget = t.Budget, 
                Members = t.TripMembers.Select(m => new TripMemberDto
                {
                    AccountId = m.AccountId,
                    Email = m.Account.Email,
					Avatar = m.Account.AvatarUrl,
					NickName = m.Account.NickName,
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
					Avatar = trip.TripMembers.FirstOrDefault(m => m.AccountId == g.Key)?.Account?.AvatarUrl,
					NickName = trip.TripMembers.FirstOrDefault(m => m.AccountId == g.Key)?.Account?.NickName,
					PaidAmount = g.Sum(e => e.AmountInTripCurrency),
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
					Avatar = u.Account?.AvatarUrl,
					NickName = u.Account?.NickName,
					AmountReceived = $"{u.AmountReceived:N0}₫"
                }).ToList()
            }).ToList();

            var members = trip.TripMembers.Select(m => new TripMemberDto
            {
                AccountId = m.AccountId,
                Email = m.Account?.Email ?? "",
				Avatar = m.Account?.AvatarUrl,
				NickName = m.Account?.NickName,
				Status = m.Status
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
                TaxRefunds = taxRefunds,
                Members = members
            };
        }


    }
}
