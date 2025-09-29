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

        public async Task<List<Event>> GetByIdsAsync(List<Guid> eventIds)
        {
            if (eventIds == null || !eventIds.Any())
                return new List<Event>();

            return await _context.Events
                .Where(e => eventIds.Contains(e.EventId))
                .ToListAsync();
        }

        public Task<Event> GetByIdAsync(Guid eventId)
        {
            var evt = _context.Events.FirstOrDefault(e => e.EventId == eventId);
            return Task.FromResult(evt);
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

		public async Task<bool> UpdateEvent(Event evt)
		{
            var existingEvent = await _context.Events.FirstOrDefaultAsync(e => e.EventId == evt.EventId);
			if (existingEvent == null)
			{
				return false; // Event not found
			}
            var existingEventUse =  _context.Event_Uses.Where(e => e.EventId == evt.EventId);
            if(existingEventUse != null)
            {
                _context.Event_Uses.RemoveRange(existingEventUse);
                await _context.SaveChangesAsync();
            }
            existingEvent.EventName = evt.EventName;
			existingEvent.EventDescription = evt.EventDescription;
			existingEvent.EventDate = evt.EventDate;
            existingEvent.ReceiptUrl = evt.ReceiptUrl;
            existingEvent.PaidBy = evt.PaidBy;
            existingEvent.Currency = evt.Currency;
            existingEvent.AmountOriginal = evt.AmountOriginal;
            existingEvent.AmountInTripCurrency = evt.AmountInTripCurrency;
            existingEvent.ExchangeRate = evt.ExchangeRate;
            existingEvent.SplitType = evt.SplitType;
            existingEvent.Event_Use = evt.Event_Use;
            await _context.SaveChangesAsync();
            return true;
			
		}

		public async Task<bool> DeleteEvent(Guid eventId)
		{
			var existingEvent = await _context.Events.FirstOrDefaultAsync(e => e.EventId == eventId);
			if (existingEvent == null)
			{
				return false; // Event not found
			}
			var existingEventUse = _context.Event_Uses.Where(e => e.EventId == eventId);
			if (existingEventUse != null)
			{
				_context.Event_Uses.RemoveRange(existingEventUse);
                _context.Events.Remove(existingEvent);
				await _context.SaveChangesAsync();
                return true;
			}
            return false;
		}

		public async Task<Event?> GetEventById(Guid eventId)
		{
            return await _context.Events
                .Include(e => e.Account)
                .Include(e => e.Event_Use)
                .ThenInclude(eu => eu.Account)
                .FirstOrDefaultAsync(e => e.EventId == eventId);
		}
	}
}
