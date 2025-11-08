using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
	public class PlanRepository : IPlanRepository
	{
		private readonly EzBillDbContext _dbContext;
		public PlanRepository(EzBillDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<int> CountAllPlan()
		{
			return await _dbContext.Plans.CountAsync();
		}

		public async Task<List<Plan>> GetActivePlans()
		{
			return await _dbContext.Plans
				.Where(p => p.Status == "ACTIVE")
				.ToListAsync();
		}

		public async Task<Plan?> GetPlanById(Guid planId)
		{
			return await  _dbContext.Plans
				.FirstOrDefaultAsync(p => p.PlanId == planId);
		}
	}
}
