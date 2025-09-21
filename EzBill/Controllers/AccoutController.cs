
using EzBill.Application.DTO.Account;
using EzBill.Application.IService;
using EzBill.Application.ServiceModel.Account;
using EzBill.Models.Request.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace EzBill.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccoutController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
		private readonly IUserDeviceTokenService _userDeviceTokenService;

		public AccoutController(IAccountService accountService, IEmailService emailService, IUserDeviceTokenService userDeviceTokenService)
        {
            _accountService = accountService;
			_emailService = emailService;
			_userDeviceTokenService = userDeviceTokenService;
		}
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
			if (!ModelState.IsValid)
			{
				var error = ModelState.Values
	                        .SelectMany(v => v.Errors)
	                        .Select(e => e.ErrorMessage)
	                        .FirstOrDefault();

				return BadRequest(new
				{
					message = error
				});
			}
			var model = new RegisterModel
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                RePassword = registerRequest.RePassword,
                Gender = registerRequest.Gender,
                NickName = registerRequest.NickName,
                PhoneNumber = registerRequest.PhoneNumber
            };
            var check = await _accountService.Register(model);
            if (check)
            {
                return Ok(new
                {
                    message = "Register successfully"
                });
            }
			return BadRequest(new
			{
				message = "Register failed"
			});
		}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
			if (!ModelState.IsValid)
            {
				var error = ModelState.Values
							.SelectMany(v => v.Errors)
							.Select(e => e.ErrorMessage)
							.FirstOrDefault();

				return BadRequest(new
				{
					message = error
				});
			}
			
                var token = await _accountService.Login(login.Email, login.Password);
                return Ok(new
                {
                    token = token
                });
		}

		[HttpPost("login-google")]
		public async Task<IActionResult> LoginWithGoogle([FromBody] LoginGGRequest request)
		{
			var response = await _accountService.LoginWithGoogleAsync(request.Token);
			return Ok(response);
		}

		[HttpPost("users/{userId}")]
		public async Task<IActionResult> UserDeviceToken([FromRoute] string userId, [FromBody] UserDeviceTokenRequest request)
		{
			var result = await _userDeviceTokenService.SaveDeviceToken(Guid.Parse(userId), request.DeviceId, request.FCMToken);
			if (result)
			{
				return Ok(new
				{
					message = "Save device token successfully"
				});
			}
			return BadRequest(new
			{
				message = "Save device token failed"
			});
		}




		[HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var accountId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if(accountId == null)
            {
                return Unauthorized(new
                {
                    message = "Chưa login"
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

        [HttpPut("account/{id}")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, [FromRoute] string id)
        {
			if (!ModelState.IsValid)
			{
				var error = ModelState.Values
							.SelectMany(v => v.Errors)
							.Select(e => e.ErrorMessage)
							.FirstOrDefault();

				return BadRequest(new
				{
					message = error
				});
			}
			var account = await _accountService.GetProfile(Guid.Parse(id));
			if (account == null)
			{
				return NotFound(new
				{
					message = "Account not found"
				});
			}
			var model = new ProfileModel
            {
				AccountId = Guid.Parse(id),
				AvatarUrl = request.AvatarUrl,
                Email = request.Email,
                Gender = request.Gender,
				NickName = request.NickName,
				PhoneNumber = request.PhoneNumber,
                Role = account.Role
			};
			var check = await _accountService.UpdateProfile(model);
			if (check)
			{
				return Ok(new
				{
					message = "Update profile successfully"
				});
			}
			return BadRequest(new
			{
				message = "Update profile failed"
			});

		}
		[HttpGet("account")]
		public async Task<IActionResult> GetAccountByEmailForTrip([FromQuery] string email)
		{
			var accounts = await _accountService.GetAccountByEmailForTrip(email);
			return Ok(accounts);
		}
	}
}
