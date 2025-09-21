using EzBill.Application.IService;
using EzBill.Application.ServiceModel.Plan;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
	public class PlanService : IPlanService
	{
		private readonly IPlanRepository _planRepository;
		public PlanService(IPlanRepository planRepository)
		{
			_planRepository = planRepository;
		}
		public async Task<List<PlanModel>> GetActivePlans()
		{
			var plans = await _planRepository.GetActivePlans();
			return plans.Select(p => new PlanModel
			{
				PlanId = p.PlanId,
				PlanName = p.PlanName,
				Description = p.Description,
				Price = p.Price,
				BillingCycle = (p.BillingCycle == "Monthly") ? "1 Tháng" : "2 ngày",
				Status = p.Status
			}).ToList();
		}
	}
}
