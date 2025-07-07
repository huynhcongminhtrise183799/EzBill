using EzBill.Application.DTO;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace EzBill.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("event")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO dTO)
        {
            var eventId = Guid.NewGuid();
            var @event = new Event
            {
                EventId = eventId,
                EventName = dTO.EventName,
                EventDescription = dTO.EventDescription,
                ReceiptUrl = dTO.ReceiptUrl,
                TripId = dTO.TripId,
                PaidBy = dTO.PaidBy,
                Currency = dTO.Currency,
                AmountOriginal = dTO.AmountOriginal,
                ExchangeRate = dTO.ExchangeRate,
                AmountInTripCurrency = dTO.AmountInTripCurrency,
                EventDate = DateOnly.FromDateTime(DateTime.Now),
                Event_Use = dTO.Event_Used.Select(e => new Event_Use
                {
                    EventId = eventId,
                    AccountId = e.AccountId
                }).ToList()
            };
            var result = await _eventService.AddEvent(@event);
            if (result)
            {
                return Ok("Add successfully");
            }
            return BadRequest("add failed");
        }
    }
}
