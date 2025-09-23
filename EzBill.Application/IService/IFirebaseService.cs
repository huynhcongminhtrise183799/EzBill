using EzBill.Domain.Entity;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
	public interface IFirebaseService
	{
		Task<FirebaseToken> VerifyIdTokenAsync(string idToken);
		Task<List<string>> SendNotificationToAccountsAsync(Guid tripId);
		Task<List<string>> SendDebtReminderAsync(IEnumerable<Settlement> unpaidSettlements);

	}
}
