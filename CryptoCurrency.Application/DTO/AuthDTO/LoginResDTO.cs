using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.AuthDTO
{
    public class LoginResDTO
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string AuthenticatorKey { get; set; }
        public bool Is2FAEnabled { get; set; }
    }
}
