using EzBill.Application.DTO;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EzBill.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpPost("trip")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDTO tripDTO)
        {
            var accountId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if(accountId == null)
            {
                return BadRequest(new
                {
                    message = "Please login"
                });
            }
            var tripId = Guid.NewGuid();
            var trip = new Trip
            {
                TripId = tripId,
                TripName = tripDTO.TripName,
                StartDate = tripDTO.StartDate,
                EndDate = tripDTO.EndDate,
                CreatedBy = Guid.Parse(accountId),
                Budget = tripDTO.Budget,
                Status = TripStatus.ACTIVE.ToString(),
                TripMembers = tripDTO.TripMember.Select(t => new TripMember
                {
                    TripId = tripId,
                    AccountId = Guid.Parse(t.AccountId),
                    Amount = t.Amount,
                    AmountRemainInTrip = t.Amount,
                    Status = TripMemberStatus.ACTIVE.ToString()
                }).ToList()


            };
            var result = await _tripService.AddTrip(trip);
            if (result)
            {
                return Ok(new
                {
                    tripId = tripId
                });
            }
            return BadRequest("Have error");
        }

        [HttpGet("trip/account/{accountId}")]
        public async Task<IActionResult> GetTripsByAccount(Guid accountId)
        {
            var trips = await _tripService.GetTripsForAccountAsync(accountId);

            if (trips == null || trips.Count == 0)
                return NotFound("No trips found for this account.");

            return Ok(trips);
        }

        [HttpGet("{tripId}/details")]
        public async Task<IActionResult> GetTripDetails(Guid tripId)
        {
            var tripDetails = await _tripService.GetTripDetailsAsync(tripId);
            if (tripDetails == null)
                return NotFound("Trip not found");

            return Ok(tripDetails);
        }
    }
}
