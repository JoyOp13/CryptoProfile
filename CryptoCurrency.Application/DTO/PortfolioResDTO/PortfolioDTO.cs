using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.ProfileResDTO
{
    public class PortfolioDTO
    {
        public string CoinName { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal AvgBuyPrice { get; set; }

        public decimal CurrentPrice { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ProfitLoss { get; set; }
    }
}
