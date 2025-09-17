using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
    public class TripRepository : ITripRepository
    {
        private readonly EzBillDbContext _context;

        public TripRepository(EzBillDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddTrip(Trip trip)
        {
            try
            {
                await _context.trips.AddAsync(trip);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {


                return false;
            }
        }
        public async Task<List<TripMember>> GetTripMembersAsync(Guid tripId)
        {
            return await _context.TripMembers.Include(t => t.Account)
                .Where(tm => tm.TripId == tripId)
                .ToListAsync();
        }
        public async Task<Trip?> GetByIdAsync(Guid tripId)
        {
            return await _context.trips
                .Include(t => t.TripMembers)
                .FirstOrDefaultAsync(t => t.TripId == tripId);
        }
        public async Task<List<Trip>> GetTripsByAccountIdAsync(Guid accountId)
        {
            var trips = await _context.trips
                .Include(t => t.TripMembers)               
                    .ThenInclude(tm => tm.Account)         
                .Where(t => t.CreatedBy == accountId ||
                            t.TripMembers.Any(tm => tm.AccountId == accountId))
                .ToListAsync();

            return trips;
        }
        public async Task<Trip> GetTripDetailsByIdAsync(Guid tripId)
        {
            var trip = await _context.trips
                .Include(t => t.TripMembers)
                    .ThenInclude(tm => tm.Account)                 
                .Include(t => t.Events)
                    .ThenInclude(e => e.Event_Use)                 
                .Include(t => t.Events)
                    .ThenInclude(e => e.Account)                   
                .Include(t => t.TaxRefunds)
                    .ThenInclude(tr => tr.TaxRefund_Usages)        
                .Include(t => t.TaxRefunds)
                    .ThenInclude(tr => tr.Account)                 
                .FirstOrDefaultAsync(t => t.TripId == tripId);

            return trip;
        }

    }
}
