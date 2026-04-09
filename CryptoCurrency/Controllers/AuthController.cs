using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.DTO.AuthDTO;
using CryptoCurrency.Application.Interface;
using CryptoCurrency.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoCurrencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface authInterface;
        public AuthController(IAuthInterface authInterface)
        {
            this.authInterface = authInterface;
        }

        [HttpPost("RegisterUser")]
        public IActionResult Register(RegisterDTO dto)
        {
            authInterface.Register(dto);
            return ApiResponse.Success<object>(null, "User Registered Successfully");
        }

        [HttpPost("LoginUser")]
        public IActionResult Login(LoginReqDTO dto)
        {
            var res = authInterface.Login(dto);
            return ApiResponse.Success<object>(res, "User Logged in Successfully");
        }

        [HttpGet("QRCode")]
        public IActionResult GenerateQR(string email)
        {
            var qr = authInterface.GenerateQRCode(email, out string key);

            return Ok(new
            {
                qrCode = qr,
            });
        }

        [HttpPost("Verify")]
        public IActionResult VerifyOtp(VerifyOTPDTO dto)
        {
            var result = authInterface.VerifyOtp(dto);
            return Ok(result);
        }

    }
}
