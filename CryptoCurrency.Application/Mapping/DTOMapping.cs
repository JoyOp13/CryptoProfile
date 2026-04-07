using AutoMapper;
using CryptoCurrency.Application.DTO.Buy_SellDTO;
using CryptoCurrency.Application.DTO.BuyDTO;
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
                .ForMember(dest => dest.PaymentStatus,
                    y => y.MapFrom(_ => PaymentStatusEnum.Completed))
                .ForMember(dest => dest.PaymentMethod,
                    y => y.MapFrom(_ => "Wallet"));

                 // SELL Mapping
                 CreateMap<SellCoinDTO, Transaction>()
                .ForMember(x => x.Currency,
                    y => y.MapFrom(_ => "USD"))
                .ForMember(dest => dest.PaymentStatus,
                    y => y.MapFrom(_ => PaymentStatusEnum.Completed))
                .ForMember(dest => dest.PaymentMethod,
                    y => y.MapFrom(_ => "Wallet"));

                 // Transaction History
                 CreateMap<Transaction, TransactionHistoryDTO>()
                .ForMember(x => x.CoinName,
                    y => y.MapFrom(src => src.CryptoCoin.CryptoCoinName))
                .ForMember(dest => dest.Symbol,
                    y => y.MapFrom(src => src.CryptoCoin.CryptoSymbol))
                .ForMember(dest => dest.Amount,
                    y => y.MapFrom(src => src.TransactionAmt))
                .ForMember(dest => dest.Type,
                    y => y.MapFrom(src => src.TransactionType.ToString()))
                .ForMember(dest => dest.Date,
                    y => y.MapFrom(src => src.CreatedAt));

                // Portfolio Mapping

                 CreateMap<Portfolio, PortfolioDTO>()
                .ForMember(dest => dest.CoinName,
                    opt => opt.MapFrom(src => src.CryptoCoin.CryptoCoinName))
                .ForMember(dest => dest.Symbol,
                    opt => opt.MapFrom(src => src.CryptoCoin.CryptoSymbol))
                .ForMember(dest => dest.TotalInvestment,
                    opt => opt.MapFrom(src => src.Quantity * src.AvgBuyPrice));
        }
    }
}

