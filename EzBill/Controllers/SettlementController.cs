using EzBill.Application.DTO.Settlement;
using EzBill.Application.IService;
using EzBill.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EzBill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettlementController : ControllerBase
    {
        private readonly ISettlementService _settlementService;

        public SettlementController(ISettlementService settlementService)
        {
            _settlementService = settlementService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateSettlements([FromBody] GenerateSettlementRequest request)
        {
            var result = await _settlementService.GenerateSettlementsAsync(request.TripId);
            return Ok(result);
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetByTrip(Guid tripId)
        {
            var result = await _settlementService.GetSettlementsByTripAsync(tripId);
            return Ok(result);
        }

        [HttpPut("{settlementId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid settlementId, [FromQuery] SettlementStatus status)
        {
            await _settlementService.UpdateSettlementStatusAsync(settlementId, status);
            return NoContent();
        }

        [HttpGet("debtor/{id}")]
        public async Task<IActionResult> GetSettlementsByDebtor(Guid id)
        {
            var result = await _settlementService.GetByDebtorIdAsync(id);
            return Ok(result);
        }

        [HttpGet("account/{id}")]
		public async Task<IActionResult> GetSettlementsByAccount([FromRoute]Guid id, [FromQuery] string state)
		{
			var result = await _settlementService.GetSettlementsByAccountIdWithFiltterAsync(id,state);
			return Ok(result);
		}

		[HttpPut("{settlementId}/pay")]
		public async Task<IActionResult> ChangeStatusToPaid(Guid settlementId)
		{
			var result = await _settlementService.ChangeSettlementStatusToPaid(settlementId);
			if (result)
			{
				return Ok(new { message = "Trả nợ thành công" });
			}
			return BadRequest(new { message = "Trả nợ thất bại" });
		}

	}
}
