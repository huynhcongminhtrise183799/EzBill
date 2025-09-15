using EzBill.Application.IService;
using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly ITokenService _tokenService;

        public AccountService(IAccountRepository repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        public async Task<List<Account>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Account> GetProfile(Guid accountId)
        {
            return await _repo.GetProfile(accountId);
        }

        public async Task<string> Login(string email, string password)
        {
           var account = await _repo.Login(email, password);
            var token = await _tokenService.GenerateToken(account);
            return token;
        }

		public async Task<bool> Register(Account account)
		{
			return await _repo.Register(account);
		}

		
	}
}
