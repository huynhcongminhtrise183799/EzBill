﻿using EzBill.Application.DTO;
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
                TripMembers = tripDTO.TripMember.Select(t => new TripMember
                {
                    TripId = tripId,
                    AccountId = Guid.Parse(t.AccountId),
                    Amount = t.Amount,
                    Status = TripMemberStatus.ACTIVE.ToString()
                }).ToList()
            };
            var result = await _tripService.AddTrip(trip);
            if (result)
            {
                return Ok("Add successfully");
            }
            return BadRequest("Have error");
        }
    }
}
