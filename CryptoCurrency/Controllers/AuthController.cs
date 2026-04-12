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
    public class authController : ControllerBase
    {
        private readonly IAuthInterface authInterface;
        public authController(IAuthInterface authInterface)
        {
            this.authInterface = authInterface;
        }

        [HttpPost("registerUser")]
        public IActionResult Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            authInterface.Register(dto);
            return ApiResponse.Success<object>(null, "User Registered Successfully");
        }

        [HttpPost("loginUser")]
        public IActionResult Login(LoginReqDTO dto)
        {
            var res = authInterface.Login(dto);
            return ApiResponse.Success<object>(res, "User Logged in Successfully");
        }

        [HttpGet("qrCode")]
        public IActionResult GenerateQR(string email)
        {
            var qr = authInterface.GenerateQRCode(email);

            return Ok(new
            {
                qrCode = qr,
            });
        }

        [HttpPost("verifyOTP")]
        public IActionResult VerifyOtp(VerifyOTPDTO dto)
        {
            var result = authInterface.VerifyOtp(dto);
            return Ok(result);
        }

    }
}
