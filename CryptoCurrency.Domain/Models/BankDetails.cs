using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Domain.Models
{
    public class BankDetails
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(30)]
        public string BankName { get; set; }

        [Required]
        [MaxLength(30)]
        public string AccountHolderName { get; set; }

        [Required]
        [MaxLength(20)]
        public string AccountNumber { get; set; }

        [Required]
        [MaxLength(15)]
        public string IFSCCode { get; set; }

        [MaxLength(15)]
        public string AccountType { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public Users Users { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
