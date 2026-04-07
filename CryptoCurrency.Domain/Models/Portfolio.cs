using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Domain.Models
{
    public class Portfolio
    {
        [Key]
        public int PortfolioId { get; set; }
        public int UserId { get; set; }
        public int CryptoCoinId { get; set; }

        [Column(TypeName = "decimal(20,8)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(20,8)")]
        public decimal AvgBuyPrice { get; set; }
        public Users User { get; set; }
        public CryptoCoin CryptoCoin { get; set; }
    }
}
