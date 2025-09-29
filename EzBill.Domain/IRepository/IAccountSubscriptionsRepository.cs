using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
	public interface IAccountSubscriptionsRepository
	{
		Task<bool> AddAccountSubscriptions(AccountSubscriptions accountSubscriptions);

		Task<AccountSubscriptions?> GetByAccountId(Guid accountId);

		Task<bool> UpdateSubscriptions(AccountSubscriptions accountSubscriptions);

	}
}
