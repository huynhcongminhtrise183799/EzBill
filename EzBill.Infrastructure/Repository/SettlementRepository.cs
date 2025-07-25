﻿using EzBill.Domain.Entity;
using EzBill.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzBill.Infrastructure.Repository
{
    public class SettlementRepository : ISettlementRepository
    {
        private readonly EzBillDbContext _context;

        public SettlementRepository(EzBillDbContext context)
        {
            _context = context;
        }

        public async Task<Settlement?> GetByIdAsync(Guid settlementId)
        {
            return await _context.Settlements
                .FirstOrDefaultAsync(s => s.SettlementId == settlementId);
        }

        public async Task<List<Settlement>> GetByTripIdAsync(Guid tripId)
        {
            return await _context.Settlements
                .Include(s => s.FromAccount)
                .Include(s => s.ToAccount)
                .Include(s => s.Trip)
                .Where(s => s.TripId == tripId)
                .ToListAsync();
        }

        public async Task AddRangeAsync(List<Settlement> settlements)
        {
            await _context.Settlements.AddRangeAsync(settlements);
        }

        public async Task UpdateAsync(Settlement settlement)
        {
            _context.Settlements.Update(settlement);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Settlement>?> GetByDebtorIdAsync(Guid debtorId)
        {
            return await _context.Settlements.Where(s => s.FromAccountId == debtorId && s.Status.ToLower() == SettlementStatus.UNPAID.ToString().ToLower()).ToListAsync();
        }
    }
}
