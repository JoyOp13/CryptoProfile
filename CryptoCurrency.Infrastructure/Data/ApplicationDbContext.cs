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

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<BankDetails> BankDetails { get; set; }

        public DbSet<CryptoCoin> CryptoCoins { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        public DbSet<Wishlist> Wishlists { get; set; }





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

            modelBuilder.Entity<WalletTransaction>()
             .HasOne(wt => wt.Wallets)
             .WithMany(w => w.WalletTransactions)
             .HasForeignKey(wt => wt.WalletId)
             .OnDelete(DeleteBehavior.Restrict);





            modelBuilder.Entity<BankDetails>(b =>
            {
                b.HasOne(x => x.Users)
                 .WithMany(u => u.BankDetails)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Wishlist>(w =>
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
