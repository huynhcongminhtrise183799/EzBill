using EzBill.Application.DTO;
using EzBill.Application.DTO.Event;
using EzBill.Application.IService;
using EzBill.Application.ServiceModel.Event;
using EzBill.Domain.Entity;
using EzBill.Models.Request.Event;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace EzBill.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Application.DTO.Event.CreateEventRequest request)
        {

            var result = await _eventService.CreateEventAsync(request);
            return Ok(result);
        }

        [HttpGet("by-trip/{tripId}")]
        public async Task<IActionResult> GetEventsByTrip(Guid tripId)
        {
            var result = await _eventService.GetEventsByTripAsync(tripId);
            return Ok(result);
        }

        [HttpPut("{id}")]
		public async Task<IActionResult> UpdateEvent([FromRoute]Guid id, [FromBody] UpdateEventRequest request)
		{
            var model = new UpdateEventModel
            {
                EventId = id,
				ExchangeRate = request.ExchangeRate,
				EventName = request.EventName,
				EventDate = request.EventDate,
				EventDescription = request.EventDescription,
				ReceiptUrl = request.ReceiptUrl,
				PaidBy = request.PaidBy,
				Currency = request.Currency,
				AmountOriginal = request.AmountOriginal,
				SplitType = request.SplitType,
				TripId = request.TripId,
				IsGroupMoney = request.IsGroupMoney,
				EventUses = request.EventUses?.Select(e => new EUpdateventUseModel
				{
					AccountId = e.AccountId,
					Ratio = e.Ratio
				}).ToList()
			};
			var result = await _eventService.UpdateEvent(model);
            if (result)
            {
                return Ok(new
                {
                    message = "Update thành công"
                });
            }
			return BadRequest(new
			{
				message = "Update thất bại"
			});
		}

        [HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEvent(Guid id)
		{
			var result = await _eventService.DeleteEvent(id);
			if (result)
			{
				return Ok(new
				{
					message = "Xoá thành công"
				});
			}
			return BadRequest(new
			{
				message = "Xoá thất bại"
			});
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetEventById(Guid id)
		{
			var result = await _eventService.GetEventById(id);
			if (result != null)
			{
				return Ok(result);
			}
			return NotFound(new
			{
				message = "Sự kiện không tồn tại"
			});
		}
	}
}
