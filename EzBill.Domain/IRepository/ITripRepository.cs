using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
    public interface ITripRepository
    {
        Task<bool> AddTrip(Trip trip);

        Task<List<TripMember>> GetTripMembersAsync(Guid tripId);
		Task<List<TripMember>> GetTripMembersActiveAsync(Guid tripId);

		Task<Trip?> GetByIdAsync(Guid tripId);

        Task<List<Trip>> GetTripsByAccountIdAsync(Guid accountId);
        Task<Trip> GetTripDetailsByIdAsync(Guid tripId);

		Task<bool> UpdateTripAsync(Trip trip);

	}
}
