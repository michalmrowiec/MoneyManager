﻿using Microsoft.EntityFrameworkCore;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Entities.CryptoAssets;

namespace MoneyManager.Infractructure
{
    public class MoneyManagerContext : DbContext
    {
        public MoneyManagerContext(DbContextOptions<MoneyManagerContext> dbContextOptions) : base(dbContextOptions)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RecurringRecord> RecurringRecords { get; set; }
        public DbSet<PlannedBudget> PlannedBudgets { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<CryptoAsset> CryptoAssets { get; set; }
        public DbSet<CryptocurrencySimpleData> CryptoSimpleDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CryptocurrencySimpleData>()
                .Property(p => p.Price)
                .HasPrecision(22, 10);

            modelBuilder.Entity<CryptoAsset>(eb =>
            {
                eb.HasOne(c => c.User)
                .WithMany(u => u.CryptoAssets)
                .HasForeignKey(c => c.UserId);

                eb.Property(p => p.Amount)
                .HasPrecision(22, 10);
            });

            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Record>(eb =>
            {
                eb.HasOne(r => r.User)
                    .WithMany(u => u.Records)
                    .HasForeignKey(r => r.UserId);

                eb.HasOne(r => r.Category)
                    .WithMany()
                    .HasForeignKey(r => r.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                eb.Property(r => r.Amount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<RecurringRecord>(eb =>
            {
                eb.HasOne(r => r.User)
                    .WithMany(u => u.RecurringRecords)
                    .HasForeignKey(r => r.UserId);

                eb.HasOne(r => r.Category)
                    .WithMany()
                    .HasForeignKey(r => r.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                eb.Property(rr => rr.Amount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<PlannedBudget>(eb =>
            {
                eb.HasOne(r => r.User)
                    .WithMany(u => u.PlannedBudgets)
                    .HasForeignKey(r => r.UserId);

                eb.HasOne(r => r.Category)
                    .WithMany()
                    .HasForeignKey(r => r.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                eb.Property(pb => pb.Amount).HasPrecision(18, 2);
                eb.Property(pb => pb.FilledAmount).HasPrecision(18, 2);
            });
        }
    }
}
