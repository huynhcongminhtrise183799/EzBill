using EzBill.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;


namespace EzBill.Infrastructure
{
    public class EzBillDbContext : DbContext
    {
        public EzBillDbContext(DbContextOptions<EzBillDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");
                entity.HasKey(e => e.AccountId);
                entity.Property(e => e.AccountId).ValueGeneratedOnAdd();
            });
        }
    }
}
