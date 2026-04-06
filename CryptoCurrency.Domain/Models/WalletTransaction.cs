using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Domain.Models
{
    public class WalletTransaction
    {
        [Key]
        public int WalletTransactionId { get; set; }

        //[ForeignKey("User")]
        //public int UserId { get; set; }

        //public User User { get; set; }



        [ForeignKey("BankDetails")]
        public int AccountId { get; set; }

        public BankDetails BankDetails { get; set; }

        [ForeignKey("Wallets")]
        public int WalletId { get; set; }
        public Wallet Wallets { get; set; }


        public string WalletAction { get; set; }

        [Column(TypeName = "decimal(11,2)")]
        public decimal Amount { get; set; }

        public string status { get; set; }


        public string? ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
