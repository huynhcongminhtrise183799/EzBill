using EzBill.Application.Exceptions;
using EzBill.Application.IService;
using EzBill.Application.ServiceModel.AccountSubscriptions;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
	public class AccountSubscriptionsService : IAccountSubscriptionsService
	{
		private readonly IAccountSubscriptionsRepository _repo;
		private readonly IPlanRepository _planRepo;
		public AccountSubscriptionsService(IAccountSubscriptionsRepository repo, IPlanRepository planRepo)
		{
			_repo = repo;
			_planRepo = planRepo;
		}
		public async Task<bool> AddAccountSubscriptionsAsync(CreateAccountSubscriptionsModel model)
		{
			var plan = await _planRepo.GetPlanById(model.PlanId);
			if (plan == null)
			{
				throw new AppException("Plan not found", 404);
			}
			if (plan.Status != "ACTIVE")
			{
				throw new AppException("Plan is not active", 400);
			}
			DateTime now = DateTime.UtcNow;

			// Chuyển sang DateOnly (chỉ giữ ngày, bỏ giờ)
			DateOnly dateOnly = DateOnly.FromDateTime(now);
			var entity = new AccountSubscriptions
			{
				AccountId = model.AccountId,
				PlanId = model.PlanId,
				StartDate = dateOnly,
				EndDate = (plan.BillingCycle == "Monthly") ? dateOnly.AddMonths(1) : dateOnly.AddDays(2),
				GroupRemaining = plan.MaxGroups,
				Status = PlanStatus.ACTIVE.ToString()
			};
			return await _repo.AddAccountSubscriptions(entity);
		}
	}
}
