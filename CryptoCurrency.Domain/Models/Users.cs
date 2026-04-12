using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Domain.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(150)]
        public string Password { get; set; }
        public string? Role { get; set; }
        public string? AuthenticatorKey { get; set; }
        public bool Is2FAEnabled { get; set; }
        public Wallet? Wallet { get; set; }
        public List<Transaction> Transactions { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
