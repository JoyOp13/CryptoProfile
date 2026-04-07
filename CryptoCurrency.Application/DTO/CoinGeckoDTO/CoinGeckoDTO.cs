using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.CoinGeckoDTO
{
    public class CoinGeckoDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string image { get; set; }
        public decimal current_price { get; set; }
    }
}
