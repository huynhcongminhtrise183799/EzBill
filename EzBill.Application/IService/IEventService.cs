using EzBill.Application.DTO.Event;
using EzBill.Application.ServiceModel.Event;
using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
    public interface IEventService
    {
        Task<CreateEventResponse> CreateEventAsync(CreateEventRequest request);
        Task<List<EventDto>> GetEventsByTripAsync(Guid tripId);
		Task<bool> UpdateEvent(UpdateEventModel request);

		Task<bool> DeleteEvent(Guid eventId);
		
		Task<EventDto?> GetEventById(Guid eventId);
	}
}
