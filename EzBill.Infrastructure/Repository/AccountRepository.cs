using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly EzBillDbContext _context;

        public AccountRepository(EzBillDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAll()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> GetProfile(Guid accountId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
            if(account == null)
            {
                throw new Exception("Account Not Found");
            }
            return account;
        }

        public async Task<Account> Login(string email, string password)
        {
            var result = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
            if (result == null)
            {
                throw new Exception("Account not found");
            }
            return result;
        }
        public async Task<Account?> GetByIdAsync(Guid accountId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
        }
        public async Task<List<Account>> GetByIdsAsync(IEnumerable<Guid> accountIds)
        {
            return await _context.Accounts
                .Where(a => accountIds.Contains(a.AccountId))
                .ToListAsync();
        }

		public async Task<bool> Register(Account account)
		{
            try
            {
                await _context.Accounts.AddAsync(account);
                await _context.SaveChangesAsync();
                return true;
			}
            catch (Exception)
            {

                return false;
            }

		}
	}
}
