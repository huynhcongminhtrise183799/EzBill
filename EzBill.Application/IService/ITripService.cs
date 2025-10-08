using EzBill.Application.DTO.Trip;
using EzBill.Application.ServiceModel.Trip;
using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
    public interface ITripService
    {
        Task<bool> AddTrip(Trip trip);
        Task<List<TripDto>> GetTripsForAccountAsync(Guid accountId);
        Task<TripDetailsDto> GetTripDetailsAsync(Guid tripId);

        Task<bool> AddMoreTripMember(AddTripMemberModel addTripMemberModel);

        Task<bool> UpdateTripMember(UpdateTripModel updateTripMemberModel, Guid tripId);


    }
}
