using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
    public interface IAccountService
    {
        Task<string> Login(string email, string password);
        Task<List<Account>> GetAll();

        Task<Account> GetProfile(Guid accountId);

    }
}
