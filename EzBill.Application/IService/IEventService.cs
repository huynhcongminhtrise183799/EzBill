using EzBill.Application.DTO.Event;
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

    }
}
