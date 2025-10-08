using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
	public interface ITripMemberRepository
	{
		Task<bool> UpdateAmountRemain(Guid tripId, Guid accountId, double newAmount);

		Task<bool> AddTripMember(Guid accountId, Guid tripId);

		Task<bool> UpdateTripMember(List<TripMember> accountId, Guid tripId);
	}
}
