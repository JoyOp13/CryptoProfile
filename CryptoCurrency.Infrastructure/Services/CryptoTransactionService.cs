using AutoMapper;
using CryptoCurrency.Application.DTO.Buy_SellDTO;
using CryptoCurrency.Application.DTO.BuyDTO;
using CryptoCurrency.Application.DTO.Response;
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


        public async Task<ServiceResponse> BuyCoin(BuyCoinDTO dto, int userId)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
                return new ServiceResponse
                {
                    Message = "User Not Found"
                };

            // Wallet For User actual ballance
            var wallet = db.Wallet.FirstOrDefault(x => x.UserId == user.UserId);
            if (wallet == null)
                return new ServiceResponse
                {
                    Message = "Wallet Not Found"
                };

            var coin = db.CryptoCoin.FirstOrDefault(x => x.CryptoCoinId == dto.CryptoCoinId);
            if (coin == null)
                return new ServiceResponse
                {
                    Message = "Coin Not Found"
                };

            var portfolioList = db.Portfolio
                .Where(x => x.UserId == user.UserId)
                .Include(x => x.CryptoCoin)
                .ToList();

            var price = db.CryptoCoin.Where(x => x.CryptoCoinId == dto.CryptoCoinId).Select(x => x.CurrentPrice).FirstOrDefault();
            //var price = await service.GetCoinPrice(coin.CoinGeckoId);
            var totalAmount = dto.Quantity * price;

            // Checking Balance
            if (wallet.Balance < totalAmount)
                return new ServiceResponse { Message = "Wallet Not Found" };

            // subtracting amount from wallet
            wallet.Balance -= totalAmount;
            wallet.ModifiedAt = DateTime.Now;
            wallet.ModifiedBy = user.UserName;

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
                    AvgBuyPrice = price
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
            transaction.CreatedBy = user.UserName;
            db.Transaction.Add(transaction);
            db.SaveChanges();

            return new ServiceResponse { Success= true ,
            Message="Coin Purchased Successfully"
            };
        }
        public async Task <ServiceResponse> SellCoin(SellCoinDTO dto, int userId)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null) 
                return new ServiceResponse { 
                    Message = "User Not Found" 
                };

            var wallet = db.Wallet.FirstOrDefault(x => x.UserId == user.UserId);
            if (wallet == null) 
                return new ServiceResponse 
                { 
                    Message = "Wallet Not Found" 
                };

            var coin = db.CryptoCoin.FirstOrDefault(x => x.CryptoCoinId == dto.CryptoCoinId);
            if (coin == null) 
                return new ServiceResponse 
                { 
                    Message = "Crypto Coin Not Found" 
                };

            var portfolio = db.Portfolio
                .FirstOrDefault(x => x.UserId == user.UserId && x.CryptoCoinId == dto.CryptoCoinId);

            if (portfolio == null)
                return new ServiceResponse 
                { 
                    Message = "Portfolio Not Found" 
                };

            if (portfolio.Quantity < dto.Quantity)
                return new ServiceResponse 
                { 
                    Message = "Insufficient Quantity in Portfolio" 
                };

            // var price = await service.GetCoinPrice(coin.CoinGeckoId);
            var price = db.CryptoCoin.Where(x => x.CryptoCoinId == dto.CryptoCoinId).Select(x => x.CurrentPrice).FirstOrDefault();

            var totalAmount = dto.Quantity * price;

            var profit = (price - portfolio.AvgBuyPrice) * dto.Quantity;

            portfolio.Quantity -= dto.Quantity;

            if (portfolio.Quantity == 0)
            {
                db.Portfolio.Remove(portfolio);
               
            }
            wallet.Balance += totalAmount;
            wallet.ModifiedAt = DateTime.Now;
            wallet.ModifiedBy = user.UserName;

            var transaction = mapper.Map<Transaction>(dto);

            transaction.UserId = user.UserId;
            transaction.WalletId = wallet.WalletId;
            transaction.TransactionAmt = totalAmount;
            transaction.TransactionType = TransactionTypeEnum.Sell;
            transaction.CreatedAt = DateTime.Now;
            transaction.CreatedBy = user.UserName;
            db.Transaction.Add(transaction);

            await db.SaveChangesAsync();

            return new ServiceResponse
            {
                Success = true,
                Message = "Coin Sell Successfully"
            };
        }

        public List<TransactionHistoryDTO> GetTransactionHistory(int userId)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);
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
