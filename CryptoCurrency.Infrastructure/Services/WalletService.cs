using CryptoCurrency.Application.DTO.WalletDTO;
using CryptoCurrency.Application.Interface;
using CryptoCurrency.Domain.Models;
using CryptoCurrency.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Infrastructure.Services
{
    public class WalletService : IWalletInterface
    {
            private readonly ApplicationDbContext db;

            public WalletService(ApplicationDbContext db)
            {
                this.db = db;
            }

            public void AddMoney(AddMoneyDTO dto, string userName)
            {
                //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
                 var user = db.Users.FirstOrDefault(x => x.UserName == "jay");
                
                if (user == null) throw new Exception("User not found");

                var wallet = db.Wallet.FirstOrDefault(x => x.UserId == user.UserId);

                if (wallet == null)
                {
                    wallet = new Wallet
                    {
                        UserId = user.UserId,
                        Balance = dto.Amount,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userName
                    };
                    db.Wallet.Add(wallet);
                db.SaveChanges();
            }
                else
                {
                    wallet.Balance += dto.Amount;
                    wallet.ModifiedAt = DateTime.Now;
                    wallet.ModifiedBy = userName;
                }

                // Saveing Wallet History 
                db.Set<WalletHistory>().Add(new WalletHistory
                {
                    UserId = user.UserId,
                    WalletId = wallet.WalletId,
                    WalletAction = "Deposit",
                    Amount = dto.Amount,
                    status = "Completed",
                    CreatedAt = DateTime.Now,
                    CreatedBy = userName
                });

                db.SaveChanges();
            }

            public void WithdrawMoney(WithDrawDTO dto, string userName)
            {
                //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
                var user = db.Users.FirstOrDefault(x => x.UserName == "jay");
                if (user == null) throw new Exception("User not found");

                var wallet = db.Wallet.FirstOrDefault(x => x.UserId == user.UserId);
                if (wallet == null) throw new Exception("Wallet not found");

                if (wallet.Balance < dto.Amount)
                    throw new Exception("Insufficient balance");

                wallet.Balance -= dto.Amount;
                wallet.ModifiedAt = DateTime.Now;
                wallet.ModifiedBy = userName;

                // Saveing Wallet History
                db.Set<WalletHistory>().Add(new WalletHistory
                {
                    UserId = user.UserId,
                    WalletId = wallet.WalletId,
                    WalletAction = "Withdraw",
                    Amount = dto.Amount,
                    status = "Completed",
                    CreatedAt = DateTime.Now,
                    CreatedBy = userName
                });

                db.SaveChanges();
            }

            public WalletDTO GetWallet(string userName)
            {
                //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserName == "jay");

            if (user == null) throw new Exception("User not found");

                var wallet = db.Wallet.FirstOrDefault(x => x.UserId == user.UserId);

                if (wallet == null)
                {
                    return new WalletDTO { Balance = 0 };
                }

                return new WalletDTO
                {
                    Balance = wallet.Balance
                };
            }
        }
    }

