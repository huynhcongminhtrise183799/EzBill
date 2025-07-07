using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
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
    }
}
