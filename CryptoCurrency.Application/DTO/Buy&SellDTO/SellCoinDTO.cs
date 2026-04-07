using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.Buy_SellDTO
{
    public class SellCoinDTO
    {
        public int CryptoCoinId { get; set; }
        public decimal Quantity { get; set; }
    }
}
