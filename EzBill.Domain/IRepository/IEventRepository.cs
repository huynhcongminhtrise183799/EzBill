using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
    public interface IEventRepository
    {
        Task AddEventAsync(Event evt);
        Task SaveChangesAsync();
        Task<List<Event>> GetByTripIdAsync(Guid tripId);
        Task<List<Event>> GetByIdsAsync(List<Guid> eventIds);
        Task<Event> GetByIdAsync(Guid eventId);

        Task<bool> UpdateEvent(Event evt);

        Task<bool> DeleteEvent(Guid eventId);

        Task<Event?> GetEventById(Guid eventId); 
    }
}
