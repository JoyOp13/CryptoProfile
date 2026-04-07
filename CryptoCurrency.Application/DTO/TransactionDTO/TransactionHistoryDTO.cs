using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.TransactionDTO
{
    public class TransactionHistoryDTO
    {
        public string CoinName { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
    }
}
