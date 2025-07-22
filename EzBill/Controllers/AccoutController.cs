using EzBill.Application.DTO;
using EzBill.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EzBill.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccoutController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccoutController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                var token = await _accountService.Login(login.Email, login.Password);
                return Ok(new
                {
                    token = token
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

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var accountId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if(accountId == null)
            {
                return Unauthorized(new
                {
                    message = "Please login"
                });
            }
            var account = await _accountService.GetProfile(Guid.Parse(accountId));
            return Ok(account);
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAll();
            return Ok(accounts);
        }

        [HttpGet("account/{id}")]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            var account = await _accountService.GetProfile(id);
            if (account == null)
            {
                return NotFound(new
                {
                    message = "Account not found"
                });
            }
            return Ok(account);
        }
    }
}
