using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<bool> AddEvent(Event @event)
        {
            try
            {
                await _context.Events.AddAsync(@event);
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
