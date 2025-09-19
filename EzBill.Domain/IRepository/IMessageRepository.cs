using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
	public interface IMessageRepository
	{
		Task<bool> AddMessageAsync(Messages message);
		Task<List<Messages>> GetMessagesByTripIdAsync(Guid tripId, int page);
	}
}
