using EzBill.Application.DTO;
using EzBill.Application.DTO.Event;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
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
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
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
    }
}
