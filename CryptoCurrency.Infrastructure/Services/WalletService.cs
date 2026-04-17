using AutoMapper;
using CryptoCurrency.Application.DTO.TransactionDTO;
using CryptoCurrency.Application.DTO.WalletDTO;
using CryptoCurrency.Application.Interface;
using CryptoCurrency.Domain.Models;
using CryptoCurrency.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Infrastructure.Services
{
    public class WalletService : IWalletInterface
    {
            private readonly ApplicationDbContext db;
            private readonly IMapper mapper;
            private readonly IConfiguration config;
            public WalletService(ApplicationDbContext db, IMapper mapper, IConfiguration config)
            {
                this.db = db;
                this.mapper = mapper;
                this.config = config;
            }  

            public object CreateOrder(decimal amount)
                {
            var key = config["Razorpay:Key"];
            var secret = config["Razorpay:Secret"];
            var client = new RazorpayClient(key,secret);

                    Dictionary<string, object> options = new Dictionary<string, object>();

                    options.Add("amount", amount * 100); 
                    options.Add("currency", "INR");
                    options.Add("receipt", Guid.NewGuid().ToString());

                    Order order = client.Order.Create(options);

            return new
            {
                id = order["id"]?.ToString(),
                amount = Convert.ToInt32(order["amount"]),
                currency = order["currency"]?.ToString()
            };
        }

        public bool VerifyPayment(string orderId, string paymentId, string signature)
        {
            var secret = config["Razorpay:Secret"];
            string payload = orderId + "|" + paymentId;
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            using (var hmac = new HMACSHA256(secretBytes))
            {
                var hash = hmac.ComputeHash(payloadBytes);
                var generatedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();
                return generatedSignature == signature;
            }
        }


        public void AddMoney(AddMoneyDTO dto, int userId)
            {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);

            if (user == null) throw new Exception("User not found");

                var wallet = db.Wallet.FirstOrDefault(x => x.UserId == user.UserId);

                if (wallet == null)
                {
                    wallet = new Wallet
                    {
                        UserId = user.UserId,
                        Balance = dto.Amount,
                        CreatedAt = DateTime.Now,
                        CreatedBy = user.UserName
                    };
                    db.Wallet.Add(wallet);
                db.SaveChanges();
            }
                else
                {
                    wallet.Balance += dto.Amount;
                    wallet.ModifiedAt = DateTime.Now;
                    wallet.ModifiedBy = user.UserName;
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
                    CreatedBy = user.UserName
                });

                db.SaveChanges();
            }

            public void WithdrawMoney(WithDrawDTO dto, int userId)
            {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null) throw new Exception("User not found");

                var wallet = db.Wallet.FirstOrDefault(x => x.UserId == user.UserId);
                if (wallet == null) throw new Exception("Wallet not found");

                if (wallet.Balance < dto.Amount)
                    throw new Exception("Insufficient balance");

                wallet.Balance -= dto.Amount;
                wallet.ModifiedAt = DateTime.Now;
                wallet.ModifiedBy = user.UserName;

                // Saveing Wallet History
                db.Set<WalletHistory>().Add(new WalletHistory
                {
                    UserId = user.UserId,
                    WalletId = wallet.WalletId,
                    WalletAction = "Withdraw",
                    Amount = dto.Amount,
                    status = "Completed",
                    CreatedAt = DateTime.Now,
                    CreatedBy = user.UserName
                });

                db.SaveChanges();
            }

            public WalletDTO GetWallet(int userId)
            {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);

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

        public List<WalletTransactionDTO> GetWalletHistory(int userId)
        {
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
                throw new Exception("User Nahi mil Raha Bhai");

            var trasaction = db.WalletHistory.Where(x => x.UserId == user.UserId)
                .OrderByDescending(x => x.CreatedAt).ToList();

            var result = mapper.Map <List< WalletTransactionDTO>>(trasaction);
            return result;
        }
    }
    }

