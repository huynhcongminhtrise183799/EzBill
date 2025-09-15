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
        Task<Account> Login(string email, string password); 

        Task<List<Account>> GetAll();

        Task<Account> GetProfile(Guid accountId);

        Task<Account?> GetByIdAsync(Guid accountId);
        Task<List<Account>> GetByIdsAsync(IEnumerable<Guid> accountIds);


    }
}
