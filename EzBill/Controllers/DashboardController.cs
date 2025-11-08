using EzBill.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzBill.Controllers
{
	[Route("api/dashboard/")]
	[ApiController]
	public class DashboardController : ControllerBase
	{
		private readonly IAccountService _accountService;
		private readonly IPlanService _planService;
		private readonly IPaymentHistoryService _paymentHistoryService;
		public DashboardController(IAccountService accountService, IPlanService planService, IPaymentHistoryService paymentHistoryService)
		{
			_accountService = accountService;
			_planService = planService;
			_paymentHistoryService = paymentHistoryService;
		}
		[HttpGet("users")]
		public async Task<IActionResult> GetTotalUsers()
		{
			var totalUsers = await _accountService.CountAllCustomer();
			return Ok(new { totalUsers });
		}
		[HttpGet("plans")]
		public async Task<IActionResult> GetTotalPlans()
		{
			var totalPlans = await _planService.CountAllPlan();
			return Ok(new { totalPlans });
		}
		[HttpGet("payments/month-nearest")]
		public async Task<IActionResult> GetPaymentsInNearestMonth([FromQuery] int month)
		{
			var payments = await _paymentHistoryService.GetCompletedMonthlySummary(month);
			return Ok(new { payments });
		}
	}
}
