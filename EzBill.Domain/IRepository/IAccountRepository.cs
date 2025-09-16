using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
    public interface IAccountRepository
    {
        Task<bool> Register(Account account);
        Task<Account> Login(string email); 

        Task<List<Account>> GetAll();

        Task<Account> GetProfile(Guid accountId);

        Task<Account?> GetByIdAsync(Guid accountId);
        Task<List<Account>> GetByIdsAsync(IEnumerable<Guid> accountIds);

        Task<bool> CheckEmailExist(string email);

		Task<bool> CheckNickName(string nickName);

        Task<bool> CheckPhoneNumber(string phone);

        Task<Account?> FindByEmailAsync(string email);

        Task<bool> Update(Account account);

        Task<List<Account>> GetAccountByEmailForTrip(string email);


	}
}
