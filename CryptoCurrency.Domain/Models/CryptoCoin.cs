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

        [Required]
        [MaxLength(30)]
        public string CryptoCoinName { get; set; }

        [Required]
        [MaxLength(50)]
        public string CryptoIcon { get; set; }

        [Required]
        [MaxLength(30)]
        public string CrytoSymbol { get; set; }

        [Required]
        [Column(TypeName = "decimal(20,8)")]
        public decimal CryptoPrice { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
