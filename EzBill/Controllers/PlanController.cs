using EzBill.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzBill.Controllers
{
	[Route("api/plan")]
	[ApiController]
	public class PlanController : ControllerBase
	{
		private readonly IPlanService _planService;
		public PlanController(IPlanService planService)
		{
			_planService = planService;
		}
		[HttpGet("active")]
		public async Task<IActionResult> GetActivePlans()
		{
			try
			{
				var plans = await _planService.GetActivePlans();
				return Ok(plans);
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					message = ex.Message
				});
			}
		}
	}
}
