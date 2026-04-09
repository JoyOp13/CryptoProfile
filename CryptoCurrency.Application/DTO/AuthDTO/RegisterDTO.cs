using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.DTO.AuthDTO
{
    public class RegisterDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public string Role { get; set; }
    }
}
