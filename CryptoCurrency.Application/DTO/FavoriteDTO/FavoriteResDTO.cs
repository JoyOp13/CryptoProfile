using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.FavoriteDTO
{
    public class FavoriteResDTO
    {
        public int CryptoCoinId { get; set; }
        public string CoinName { get; set; }
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
