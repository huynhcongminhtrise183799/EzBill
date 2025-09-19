using EzBill.Application.ServiceModel.Chat;
using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
    public interface IChatService
    {
		Task<bool> SendMessageAsync(ChatModel chatModel);

		Task<List<GetMessageModel>> GetMessagesByTripAsync(Guid tripId, int page);

	}
}
