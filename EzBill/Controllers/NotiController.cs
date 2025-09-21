using EzBill.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzBill.Controllers
{
	[Route("api/noti/")]
	[ApiController]
	public class NotiController : ControllerBase
	{
		private readonly IFirebaseService _firebaseService;
		public NotiController(IFirebaseService firebaseService)
		{
			_firebaseService = firebaseService;
		}
		[HttpPost("send/{tripId}")]
		public async Task<IActionResult> SendNotiToTripMembers([FromRoute] Guid tripId)
		{
			try
			{
				var result = await _firebaseService.SendNotificationToAccountsAsync(tripId);
				return Ok(new
				{
					message = "Send notification successfully",
					data = result
				});
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
