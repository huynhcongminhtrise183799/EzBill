using EzBill.Application.DTO.Account;
using EzBill.Application.ServiceModel.Account;
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
		Task<bool> Register(RegisterModel account);

		Task<string> Login(string email, string password);
        Task<List<Account>> GetAll();

        Task<ProfileModel?> GetProfile(Guid accountId);

        Task<string> LoginWithGoogleAsync(string token);

        Task<Account?> GetAccountByEmail(string email);
        Task<bool> RePassword(RePasswordModel model);

        Task<bool> UpdateProfile(ProfileModel profileModel);

		Task<List<FillterAccountByEmail>> GetAccountByEmailForTrip(string email);

        Task<bool> UpdateAccountRole(Guid accountId, string role);

        Task<int> CountAllCustomer();   

	}
}
