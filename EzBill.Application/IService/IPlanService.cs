using EzBill.Application.ServiceModel.Plan;
using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
	public interface IPlanService
	{
		Task<List<PlanModel>> GetActivePlans();
		Task<Plan?> GetPlanById(Guid planId);

		Task<int> CountAllPlan();

	}
}
