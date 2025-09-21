using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
	public interface IPlanRepository
	{
		Task<List<Plan>> GetActivePlans();
		Task<Plan?> GetPlanById(Guid planId);
	}
}
