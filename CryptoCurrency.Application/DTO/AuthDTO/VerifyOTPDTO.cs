using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.AuthDTO
{
    public class VerifyOTPDTO
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
