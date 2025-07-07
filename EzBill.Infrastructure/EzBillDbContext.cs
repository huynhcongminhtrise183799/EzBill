using EzBill.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;


namespace EzBill.Infrastructure
{
    public class EzBillDbContext : DbContext
    {
        public EzBillDbContext(DbContextOptions<EzBillDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Trip> trips { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TripMember> TripMembers { get; set; }
        public DbSet<Event_Use> Event_Uses { get; set; }
        public DbSet<Settlement> Settlements { get; set; }
        public DbSet<TaxRefund> TaxRefunds { get; set; }
        public DbSet<TaxRefund_Usage> TaxRefund_Usages { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");
                entity.HasKey(e => e.AccountId);
                entity.Property(e => e.AccountId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("Trip");
                entity.HasKey(e => e.TripId);
                entity.Property(e => e.TripId).ValueGeneratedOnAdd();
                entity.Property(e => e.Budget).IsRequired(false);

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.Trip)
                      .HasForeignKey(e => e.TripId)
                      .HasConstraintName("FK_TRIP_ACCOUNT");
            });

            modelBuilder.Entity<TripMember>(entity =>
            {
                entity.ToTable("TripMember");
                entity.HasKey(e => new { e.TripId, e.AccountId });
                entity.Property(e => e.Amount).IsRequired(false);

                entity.HasOne(e => e.Trip)
                      .WithMany(e => e.TripMembers)
                      .HasForeignKey(e => e.TripId)
                      .HasConstraintName("FK_TRIPMEMBER_TRIP");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.TripMembers)
                      .HasForeignKey(e => e.AccountId)
                      .HasConstraintName("FK_TRIPMEMBER_Account");

            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");
                entity.HasKey(e => e.EventId);
                entity.Property(e => e.EventId).ValueGeneratedOnAdd();
                entity.Property(e => e.ExchangeRate).IsRequired(false);
                entity.Property(e => e.ReceiptUrl).IsRequired(false);

                entity.HasOne(e => e.Trip)
                      .WithMany(e => e.Events)
                      .HasForeignKey(e => e.TripId)
                      .HasConstraintName("FK_TRIP_EVENT");

                entity.HasOne(e => e.Account)
                      .WithMany(e => e.Events)
                      .HasForeignKey(e => e.PaidBy)
                      .HasConstraintName("FK_EVENT_ACCOUNT");
            });

            modelBuilder.Entity<Event_Use>(entity =>
            {
                entity.ToTable("Event_Use");
                entity.HasKey(e => new { e.AccountId, e.EventId });

                entity.HasOne(e => e.Event)
                      .WithMany(e => e.Event_Use)
                      .HasForeignKey(e => e.EventId)
                      .HasConstraintName("FK_EVENT_USE_EVENT");

                entity.HasOne(e => e.Account)
                     .WithMany(e => e.Event_Use)
                     .HasForeignKey(e => e.AccountId)
                     .HasConstraintName("FK_EVENT_USE_Account");
            });

            modelBuilder.Entity<Settlement>(entity =>
            {
                entity.ToTable("Settlement");
                entity.HasKey(e => e.SettlementId);
                entity.Property(e => e.SettlementId).ValueGeneratedOnAdd();


                entity.HasOne(e => e.Trip)
                      .WithMany(e => e.Settlements)
                      .HasForeignKey(e => e.TripId)
                      .HasConstraintName("FK_Settlement_Trip");

                entity.HasOne(e => e.FromAccount)
                      .WithMany(e => e.SettlementsFrom)
                      .HasForeignKey(e => e.FromAccountId)
                      .HasConstraintName("FK_Settlement_From_Account");

                entity.HasOne(e => e.ToAccount)
                      .WithMany(e => e.SettlementsTo)
                      .HasForeignKey(e => e.ToAccountId)
                      .HasConstraintName("FK_Settlement_To_Account");
            });
            modelBuilder.Entity<TaxRefund>(entity =>
            {
                entity.ToTable("TaxRefund");
                entity.HasKey(e => e.TaxRefundId);
                entity.Property(e => e.TaxRefundId).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Event)
                     .WithMany(e => e.TaxRefunds)
                     .HasForeignKey(e => e.EventId)
                     .HasConstraintName("FK_TaxRefund_Event");

                entity.HasOne(e => e.Account)
                     .WithMany(e => e.TaxRefunds)
                     .HasForeignKey(e => e.RefundedBy)
                     .HasConstraintName("FK_TaxRefund_Account");
            });

            modelBuilder.Entity<TaxRefund_Usage>(entity =>
            {
                entity.ToTable("TaxRefund_Usage");
                entity.HasKey(e => new { e.AccountId, e.TaxRefundId });
                entity.Property(e => e.Ratio).IsRequired(false);

                entity.HasOne(e => e.Account)
                    .WithMany(e => e.TaxRefund_Usages)
                    .HasForeignKey(e => e.AccountId)
                    .HasConstraintName("FK_TaxRefund_Usage_Account");

                entity.HasOne(e => e.TaxRefund)
                    .WithMany(e => e.TaxRefund_Usages)
                    .HasForeignKey(e => e.TaxRefundId)
                    .HasConstraintName("FK_TaxRefund_Usage_TaxRefund");

            });
            modelBuilder.Entity<PaymentHistory>(entity =>
            {
                entity.ToTable("PaymentHistory");
                entity.HasKey(e => e.PaymentHistoryId);
                entity.Property(e => e.PaymentHistoryId).ValueGeneratedOnAdd();
                entity.Property(e => e.PaymentUrlBill).IsRequired(false);
                entity.Property(e => e.RelatedTaxRefundId).IsRequired(false);



                entity.HasOne(e => e.Trip)
                     .WithMany(e => e.PaymentHistories)
                     .HasForeignKey(e => e.TripId)
                     .HasConstraintName("FK_PaymentHistory_Trip");

                entity.HasOne(e => e.FromAccount)
                      .WithMany(e => e.PaymentHistoriesFrom)
                      .HasForeignKey(e => e.FromAccountId)
                      .HasConstraintName("FK_PaymentHistory_From_Account");

                entity.HasOne(e => e.ToAccount)
                      .WithMany(e => e.PaymentHistoriesTo)
                      .HasForeignKey(e => e.ToAccountId)
                      .HasConstraintName("FK_PaymentHistory_To_Account");
            });
        }
    }
}
