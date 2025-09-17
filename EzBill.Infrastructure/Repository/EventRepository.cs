using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly EzBillDbContext _context;

        public EventRepository(EzBillDbContext context)
        {
            _context = context;
        }

        public async Task AddEventAsync(Event evt)
        {
            await _context.Events.AddAsync(evt);
        }

        public async Task<List<Event>> GetByTripIdAsync(Guid tripId)
        {
            return await _context.Events
                .Include(e => e.Account)
				.Include(e => e.Event_Use)
                    .ThenInclude(eu => eu.Account)
                .Where(e => e.TripId == tripId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
