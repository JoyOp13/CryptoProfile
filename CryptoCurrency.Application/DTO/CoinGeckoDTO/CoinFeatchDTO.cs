using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.CoinGeckoDTO
{
    public class CoinFeatchDTO
    {
        public int CryptoCoinId { get; set; }
        public string CoinGeckoId { get; set; }
        public string CryptoCoinName { get; set; }
        public string CryptoIcon { get; set; }
        public string CryptoSymbol { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
