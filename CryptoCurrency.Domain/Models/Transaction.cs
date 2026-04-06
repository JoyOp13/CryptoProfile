using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Domain.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public Users Users { get; set; }

        [ForeignKey("CryptoCoin")]
        public int CryptoCoinId { get; set; }

        public CryptoCoin CryptoCoin { get; set; }



        [ForeignKey("Wallet")]
        public int WalletId { get; set; }

        public Wallet Wallet { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TransactionAmt { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; }

        [MaxLength(10)]
        public string TransactionType { get; set; }

        [MaxLength(15)]
        public string PaymentStatus { get; set; }

        [MaxLength(15)]
        public string PaymentMethod { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
