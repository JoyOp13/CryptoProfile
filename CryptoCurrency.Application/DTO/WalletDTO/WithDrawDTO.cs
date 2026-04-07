using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.WalletDTO
{
    public class WithDrawDTO
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
