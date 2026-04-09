using CryptoCurrency.Application.DTO.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.Interface
{
    public interface IAuthInterface
    {
        void Register(RegisterDTO dto);
        LoginResDTO Login(LoginReqDTO dto);

        string GenerateQRCode(string email, out string key);

        TokenDTO VerifyOtp(VerifyOTPDTO dto);

    }
}
