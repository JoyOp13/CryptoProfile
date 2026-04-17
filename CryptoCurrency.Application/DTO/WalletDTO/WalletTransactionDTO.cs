using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.WalletDTO
{
    public class WalletTransactionDTO
    {
        public int WalletTransactionId { get; set; }
        public string WalletAction { get; set; }
        public decimal Amount { get; set; }
        public string status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
