using EzBill.Application.DTO.Account;
using EzBill.Application.Exceptions;
using EzBill.Application.IService;
using EzBill.Application.ServiceModel.Account;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.AspNetCore.Identity;
using System.Data;


namespace EzBill.Application.Service
{
	public class AccountService : IAccountService
	{
		private readonly IAccountRepository _repo;
		private readonly ITokenService _tokenService;
		private readonly IPasswordHasher<Account> _passwordHasher;
		private readonly IFirebaseService _firebaseAuthService;

		public AccountService(IAccountRepository repo, ITokenService tokenService, IPasswordHasher<Account> passwordHasher, IFirebaseService firebaseAuthService)
		{
			_repo = repo;
			_tokenService = tokenService;
			_passwordHasher = passwordHasher;
			_firebaseAuthService = firebaseAuthService;
		}

		public async Task<int> CountAllCustomer()
		{
			var count = await _repo.CountAllCustomer();
			return count;
		}

		public async Task<Account?> GetAccountByEmail(string email)
		{
			var account = await _repo.FindByEmailAsync(email);
			if (account == null) throw new AppException("Account không tồn tại", 400);
			return account;
		}

		public async Task<List<FillterAccountByEmail>> GetAccountByEmailForTrip(string email)
		{
			var accounts = await _repo.GetAccountByEmailForTrip(email);
			var result = accounts.Select(a => new FillterAccountByEmail
			{
				AccountId = a.AccountId,
				Email = a.Email,
				NickName = a.NickName,
				Avatar = a.AvatarUrl
			}).ToList();
			return result;
		}

		public async Task<List<Account>> GetAll()
		{
			return await _repo.GetAll();
		}

		public async Task<ProfileModel?> GetProfile(Guid accountId)
		{
			var account = await _repo.GetProfile(accountId);
			if (account == null) throw new AppException("Không tìm thấy account", 400);

			var profileModel = new ProfileModel
			{
				AccountId = accountId,
				AvatarUrl = account.AvatarUrl,
				Email = account.Email,
				Gender = account.Gender ? "Nam" : "Nữ",
				NickName = account.NickName,
				PhoneNumber = account.PhoneNumber,
				Role = account.Role,
				QrCodeUrl = account.QrCodeUrl
			};
			return profileModel;

		}

		public async Task<string> Login(string email, string password)
		{
			try
			{
				var account = await _repo.Login(email);
				var result = _passwordHasher.VerifyHashedPassword(account, account.Password, password);
				if (result == PasswordVerificationResult.Failed) throw new AppException("Password không đúng", 400);
				if (account.Status == AccountStatus.BLOCKED.ToString()) throw new AppException("Tài khoản đã bị khoá", 400);
				if (account.Status == AccountStatus.INACTIVE.ToString()) throw new AppException("Tài khoản chưa được kích hoạt", 400);
				var token = await _tokenService.GenerateToken(account);
				return token;
			}
			catch (Exception)
			{

				throw new AppException("Email không đúng", 400);
			}
		}

		public async Task<string> LoginWithGoogleAsync(string token)
		{
			var decodedToken = await _firebaseAuthService.VerifyIdTokenAsync(token);
			var email = decodedToken.Claims["email"].ToString();
			if(email == null) throw new AppException("Không lấy được email từ trong token firebase", 404);
			string name = decodedToken.Claims.ContainsKey("name") ? decodedToken.Claims["name"].ToString() : email;
			string picture = decodedToken.Claims.ContainsKey("picture") ? decodedToken.Claims["picture"].ToString() : null;
			var account = await _repo.FindByEmailAsync(email);
			if (account == null)
			{
				account = new Account
				{
					AccountId = Guid.NewGuid(),
					Email = email,
					AvatarUrl = picture,
					Gender = true,
					Password = _passwordHasher.HashPassword(null, email),
					PhoneNumber = "",
					NickName = email,
					Role = AccountRole.FREE_USER.ToString(),
					Status = AccountStatus.ACTIVE.ToString()
				};
				bool add = await _repo.Register(account);
				if (!add) throw new AppException("Đăng nhập thất bại", 400);

			}
			else
			{
				if (account.Status == AccountStatus.BLOCKED.ToString()) throw new AppException("Tài khoản đã bị khoá", 400);
				if (account.Status == AccountStatus.INACTIVE.ToString()) throw new AppException("Tài khoản chưa được kích hoạt", 400);
			}

			var accessToken = await _tokenService.GenerateToken(account);
			return accessToken;

		}

		public async Task<bool> Register(RegisterModel account)
		{
			if (account.Password != account.RePassword) throw new AppException("Password và ConfirmPassword không giống nhau", 400);

			var checkEmail = await _repo.CheckEmailExist(account.Email);
			if (checkEmail) throw new AppException("Email đã tồn tại", 400);

			var checkNickName = await _repo.CheckNickName(account.NickName);
			if (checkNickName) throw new AppException("NickName đã tồn tại", 400);

			var checkPhone = await _repo.CheckPhoneNumber(account.PhoneNumber);
			if (checkPhone) throw new AppException("Số điện thoại đã tồn tại", 400);

			var newAccount = new Account
			{
				AccountId = Guid.NewGuid(),
				Email = account.Email,
				Password = _passwordHasher.HashPassword(null, account.Password),
				PhoneNumber = account.PhoneNumber,
				Gender = account.Gender == 0 ? true : false,
				NickName = account.NickName,
				Role = AccountRole.FREE_USER.ToString(),
				Status = AccountStatus.ACTIVE.ToString(),
			};
			return await _repo.Register(newAccount);
		}

		public async Task<bool> RePassword(RePasswordModel model)
		{
			var account = await _repo.FindByEmailAsync(model.Email);
			if (account == null) throw new AppException("Account không tồn tại", 400);
			if (model.Password != model.ConfirmPassword) throw new AppException("Mật khẩu mới và xác nhận mật khẩu không khớp", 400);
			account.Password = _passwordHasher.HashPassword(null, model.Password);
			var result = await _repo.Update(account);
			return result;
		}

		public async Task<bool> UpdateAccountRole(Guid accountId, string role)
		{
			return await _repo.UpdateAccountRole(accountId, role);
		}

		public async Task<bool> UpdateProfile(ProfileModel profileModel)
		{
			var account = await _repo.GetByIdAsync(profileModel.AccountId);
			if (account == null) throw new AppException("Account không tồn tại", 400);

			if (account.Email != profileModel.Email)
			{
				var checkEmail = await _repo.CheckEmailExist(profileModel.Email);
				if (checkEmail) throw new AppException("Email đã tồn tại", 400);
			}
			if (account.NickName != profileModel.NickName)
			{
				var checkNickName = await _repo.CheckNickName(profileModel.NickName);
				if (checkNickName) throw new AppException("NickName đã tồn tại", 400);
			}


			if (account.PhoneNumber != profileModel.PhoneNumber)
			{
				var checkPhone = await _repo.CheckPhoneNumber(profileModel.PhoneNumber);
				if (checkPhone) throw new AppException("Số điện thoại đã tồn tại", 400);
			}

			account.AvatarUrl = profileModel.AvatarUrl;
			account.PhoneNumber = profileModel.PhoneNumber;
			account.Email = profileModel.Email;
			account.Gender = profileModel.Gender == "0" ? true : false;
			account.NickName = profileModel.NickName;
			account.QrCodeUrl = profileModel.QrCodeUrl;
			var result = await _repo.Update(account);
			return result;

		}
	}
}
