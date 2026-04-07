using CryptoCurrency.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }

        public DbSet<Transaction> Transaction { get; set; }

        public DbSet<CryptoCoin> CryptoCoin { get; set; }

        public DbSet<Wallet> Wallet { get; set; }

        public DbSet<WalletHistory> WalletHistory { get; set; }

        public DbSet<Favorite> Favorite { get; set; }

        public DbSet<Portfolio> Portfolio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Transaction>(t =>
            {
                t.HasOne(x => x.Users)
                 .WithMany(u => u.Transactions)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

                t.HasOne(x => x.CryptoCoin)
                 .WithMany()
                 .HasForeignKey(x => x.CryptoCoinId)
                 .OnDelete(DeleteBehavior.Restrict);

                t.HasOne(x => x.Wallet)
                 .WithMany()
                 .HasForeignKey(x => x.WalletId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WalletHistory>()
             .HasOne(wt => wt.Wallets)
             .WithMany(w => w.WalletHistory)
             .HasForeignKey(wt => wt.WalletId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Favorite>(w =>
            {
                w.HasOne(x => x.Users)
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

                w.HasOne(x => x.CryptoCoin)
                 .WithMany()
                 .HasForeignKey(x => x.CryptoCoinId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Transaction>()
                .Property(t => t.TransactionType)
                    .HasConversion<string>();

                   modelBuilder.Entity<Transaction>()
                .Property(t => t.PaymentStatus)
                .HasConversion<string>();

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Employee>(e =>
        //    {
        //        e.HasOne(x => x.AddDepartments)
        //        .WithMany()
        //        .HasForeignKey(x => x.DepartmentId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //        e.HasOne(x => x.AddDesignation)
        //        .WithMany()
        //        .HasForeignKey(x => x.DesignationId)
        //        .OnDelete(DeleteBehavior.Restrict);

        //        e.HasOne(x => x.AddRole)
        //        .WithMany()
        //        .HasForeignKey(x => x.RoleId)
        //        .OnDelete(DeleteBehavior.Restrict);
        //    });
    }
}
