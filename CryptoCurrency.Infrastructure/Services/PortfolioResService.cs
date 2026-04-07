using AutoMapper;
using CryptoCurrency.Application.DTO.ProfileResDTO;
using CryptoCurrency.Application.Interface;
using CryptoCurrency.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Infrastructure.Services
{
    public class PortfolioResService : IPortfolioInterface
    {
        private readonly IMapper mapper;
        ApplicationDbContext db;
        private readonly CoinGekoService service;
        public PortfolioResService(IMapper mapper, ApplicationDbContext db, CoinGekoService service)
        {
            this.db = db;
            this.mapper = mapper;
            this.service = service;
        }

        public async Task<List<PortfolioDTO>> GetProfileResponse()
        {
            var user = db.Users.FirstOrDefault(x => x.UserName == "jay");
            if (user == null) throw new Exception("User not found");

            var portfolioList = db.Portfolio
                .Where(x => x.UserId == user.UserId)
                .Include(x => x.CryptoCoin)
                .ToList();

            var result = new List<PortfolioDTO>();

            foreach (var item in portfolioList)
            {
                var currentPrice = await service.GetCoinPrice(item.CryptoCoin.CoinGeckoId);

                var dto = mapper.Map<PortfolioDTO>(item);

                dto.CurrentPrice = currentPrice;
                dto.CurrentValue = item.Quantity * currentPrice;
                dto.ProfitLoss = dto.CurrentValue - dto.TotalInvestment;

                result.Add(dto);
            }
            return result;
        }

    }
    
    
}
