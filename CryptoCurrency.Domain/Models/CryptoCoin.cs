using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Domain.Models
{
    public class CryptoCoin
    {
        [Key]
        public int CryptoCoinId { get; set; }
        public string CoinGeckoId { get; set; }
        [Required]
        [MaxLength(200)]
        public string CryptoCoinName { get; set; }

        [Required]
        public string CryptoIcon { get; set; }

        [Required]
        [MaxLength(45)]
        public string CryptoSymbol { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(20,8)")]
        public decimal? Quantity { get; set; } = 0;
        public DateTime LastUpdated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
