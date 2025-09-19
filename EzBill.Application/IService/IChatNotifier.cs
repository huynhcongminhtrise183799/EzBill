using EzBill.Application.ServiceModel.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
	public interface IChatNotifier
	{
		Task NotifyMessageAsync(Guid tripId, object message);

	}
}
