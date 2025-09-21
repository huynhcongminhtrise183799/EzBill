using EzBill.Application.ServiceModel.AccountSubscriptions;
using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
	public interface IAccountSubscriptionsService
	{
		Task<bool> AddAccountSubscriptionsAsync(CreateAccountSubscriptionsModel model);
	}
}
