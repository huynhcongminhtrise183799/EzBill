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

        public async Task<Account> Login(string email)
        {
            var result = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
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

		public async Task<bool> CheckEmailExist(string email)
		{
			var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
			if (account != null)
			{
                return true;
			}
            return false;
		}

		public async Task<bool> CheckNickName(string nickName)
		{
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.NickName == nickName);
			if (account != null)
			{
				return true;
			}
			return false;
		}

		public async Task<Account?> FindByEmailAsync(string email)
		{
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
		}

		public async Task<bool> Update(Account account)
		{
            var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == account.AccountId);
			if (existingAccount == null)
			{
				return false; // Account not found
			}
            // Update the properties of the existing account
            existingAccount.NickName = account.NickName;
            existingAccount.PhoneNumber = account.PhoneNumber;
            existingAccount.AvatarUrl = account.AvatarUrl;
            existingAccount.Gender = account.Gender;
            existingAccount.Email = account.Email;
            existingAccount.Password = account.Password;
			existingAccount.QrCodeUrl = account.QrCodeUrl;
			await _context.SaveChangesAsync();
            return true;
		}

		public async Task<bool> CheckPhoneNumber(string phone)
		{
			var account = await _context.Accounts.FirstOrDefaultAsync(a => a.PhoneNumber == phone);
			if (account != null)
			{
				return true;
			}
			return false;
		}

		public async Task<List<Account>> GetAccountByEmailForTrip(string email)
		{
            return await _context.Accounts
				.Where(a => a.Email.Contains(email))
				.ToListAsync();
		}

		public async Task<bool> UpdateAccountRole(Guid accountId, string role)
		{
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
            if (account == null)
            {
                return false;
			}
            switch (role)
            {
				case "BASIC":
					account.Role = AccountRole.BASIC_USER.ToString();
					break;
				case "VIP":
					account.Role = AccountRole.VIP_USER.ToString();
					break;
				case "DAILY":
					account.Role = AccountRole.DAILY_USER.ToString();
					break;
				default:
                    break;

            }
           
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
