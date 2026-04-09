using AutoMapper;
using CryptoCurrency.Application.DTO.Buy_SellDTO;
using CryptoCurrency.Application.DTO.BuyDTO;
using CryptoCurrency.Application.DTO.FavoriteDTO;
using CryptoCurrency.Application.DTO.ProfileResDTO;
using CryptoCurrency.Application.DTO.TransactionDTO;
using CryptoCurrency.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CryptoCurrency.Domain.Models.Transaction;

namespace CryptoCurrency.Application.Mapping
{
    public class DTOMapping :Profile
    {
        public DTOMapping() {

                 // BUY Mapping
                 CreateMap<BuyCoinDTO, Transaction>()
                .ForMember(x => x.Currency,
                    y => y.MapFrom(_ => "USD"))
                .ForMember(y => y.PaymentStatus,
                    y => y.MapFrom(_ => PaymentStatusEnum.Completed))
                .ForMember(y => y.PaymentMethod,
                    y => y.MapFrom(_ => "Wallet"));

                 // SELL Mapping
                 CreateMap<SellCoinDTO, Transaction>()
                .ForMember(x => x.Currency,
                    y => y.MapFrom(_ => "USD"))
                .ForMember(y => y.PaymentStatus,
                    y => y.MapFrom(_ => PaymentStatusEnum.Completed))
                .ForMember(y => y.PaymentMethod,
                    y => y.MapFrom(_ => "Wallet"));

                 // Transaction History
                 CreateMap<Transaction, TransactionHistoryDTO>()
                .ForMember(x => x.CoinName,
                    y => y.MapFrom(y => y.CryptoCoin.CryptoCoinName))
                .ForMember(y => y.Symbol,
                    y => y.MapFrom(y => y.CryptoCoin.CryptoSymbol))
                .ForMember(y => y.Amount,
                    y => y.MapFrom(y => y.TransactionAmt))
                .ForMember(y => y.Type,
                    y => y.MapFrom(y => y.TransactionType.ToString()))
                .ForMember(y => y.Date,
                    y => y.MapFrom(y => y.CreatedAt));

                // Portfolio Mapping

                 CreateMap<Portfolio, PortfolioDTO>()
                .ForMember(x => x.CoinName,
                    y => y.MapFrom(y => y.CryptoCoin.CryptoCoinName))
                .ForMember(y => y.Symbol,
                    y => y.MapFrom(y => y.CryptoCoin.CryptoSymbol))
                .ForMember(y => y.TotalInvestment,
                    y => y.MapFrom(y => y.Quantity * y.AvgBuyPrice));

            // Favorite Mapping

                 CreateMap<Favorite, FavoriteResDTO>()
                .ForMember(x => x.CoinName,
                 y => y.MapFrom(y => y.CryptoCoin.CryptoCoinName))
                .ForMember(x => x.Symbol,
                 y => y.MapFrom(y => y.CryptoCoin.CryptoSymbol));

        }
    }
}

