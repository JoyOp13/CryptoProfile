using AutoMapper;
using CryptoCurrency.Application.DTO.Buy_SellDTO;
using CryptoCurrency.Application.DTO.BuyDTO;
using CryptoCurrency.Application.DTO.TransactionDTO;
using CryptoCurrency.Application.Interface;
using CryptoCurrency.Application.Mapping;
using CryptoCurrency.Domain.Models;
using CryptoCurrency.Infrastructure.Data;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static CryptoCurrency.Domain.Models.Transaction;
using static System.Net.WebRequestMethods;

namespace CryptoCurrency.Infrastructure.Services
{
    public class CryptoTransactionService : ICryptoTransactionInterface
    {
        private readonly ApplicationDbContext db;
        private readonly CoinGekoService service;
        private readonly IMapper mapper;

        public CryptoTransactionService(ApplicationDbContext db, CoinGekoService service, IMapper mapper)
        {
            this.db = db;
            this.service = service;
            this.mapper = mapper;

        }


        public async Task BuyCoin(BuyCoinDTO dto, string userName)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
                throw new Exception("User not found");

            // Wallet For User actual ballance
            var wallet = db.Wallet.FirstOrDefault(x => x.UserId == user.UserId);
            if (wallet == null)
                throw new Exception("Wallet not found");

            var coin = db.CryptoCoin.FirstOrDefault(x => x.CryptoCoinId == dto.CryptoCoinId);
            if (coin == null) throw new Exception("Coin not found");


            var price = await service.GetCoinPrice(coin.CoinGeckoId);
            var totalAmount = dto.Quantity * price;

            // Checking Balance
            if (wallet.Balance < totalAmount)
                throw new Exception("Insufficient balance");

            // subtracting amount from wallet
            wallet.Balance -= totalAmount;
            wallet.ModifiedAt = DateTime.Now;
            wallet.ModifiedBy = "jay";

            // updating user Portfolio
            var portfolio = db.Portfolio
                .FirstOrDefault(x => x.UserId == user.UserId && x.CryptoCoinId == dto.CryptoCoinId);

            if (portfolio == null)
            {
                portfolio = new Portfolio
                {
                    UserId = user.UserId,
                    CryptoCoinId = dto.CryptoCoinId,
                    Quantity = dto.Quantity,
                };

                db.Portfolio.Add(portfolio);
            }
            else
            {
                var totalCost = (portfolio.Quantity * portfolio.AvgBuyPrice) + totalAmount;

                portfolio.Quantity += dto.Quantity;
                portfolio.AvgBuyPrice = totalCost / portfolio.Quantity;
            }

            var transaction = mapper.Map<Transaction>(dto);

            transaction.UserId = user.UserId;
            transaction.WalletId = wallet.WalletId;
            transaction.TransactionAmt = totalAmount;
            transaction.TransactionType = TransactionTypeEnum.Buy;
            transaction.CreatedAt = DateTime.Now;
            transaction.CreatedBy = "jay";
            db.Transaction.Add(transaction);
         

            db.SaveChanges();
        }
        public async Task SellCoin(SellCoinDTO dto, string userName)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            if (user == null) throw new Exception("User not found");

            var wallet = db.Wallet.FirstOrDefault(x => x.UserId == user.UserId);
            if (wallet == null) throw new Exception("Wallet not found");

            var coin = db.CryptoCoin.FirstOrDefault(x => x.CryptoCoinId == dto.CryptoCoinId);
            if (coin == null) throw new Exception("Coin not found");

            var portfolio = db.Portfolio
                .FirstOrDefault(x => x.UserId == user.UserId && x.CryptoCoinId == dto.CryptoCoinId);

            if (portfolio == null)
                throw new Exception("You don’t own this coin");

            if (portfolio.Quantity < dto.Quantity)
                throw new Exception("Not enough quantity to sell");

            var price = await service.GetCoinPrice(coin.CoinGeckoId);

            var totalAmount = dto.Quantity * price;

            var profit = (price - portfolio.AvgBuyPrice) * dto.Quantity;

            portfolio.Quantity -= dto.Quantity;

            if (portfolio.Quantity == 0)
            {
                db.Portfolio.Remove(portfolio);
               
            }
            wallet.Balance += totalAmount;
            wallet.ModifiedAt = DateTime.Now;
            wallet.ModifiedBy = "jay";

            var transaction = mapper.Map<Transaction>(dto);

            transaction.UserId = user.UserId;
            transaction.WalletId = wallet.WalletId;
            transaction.TransactionAmt = totalAmount;
            transaction.TransactionType = TransactionTypeEnum.Sell;
            transaction.CreatedAt = DateTime.Now;
            transaction.CreatedBy = "jay";
            db.Transaction.Add(transaction);

            await db.SaveChangesAsync();
        }

        public List<TransactionHistoryDTO> GetTransactionHistory(string userName)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
                throw new Exception("User Nahi mil Raha Bhai");

            var trasaction = db.Transaction.Where(x => x.UserId == user.UserId)
                .Include(x => x.CryptoCoin)
                .OrderByDescending(x => x.CreatedAt).ToList();

            var result = mapper.Map<List<TransactionHistoryDTO>>(trasaction);
            return result;
        }

    }
}
