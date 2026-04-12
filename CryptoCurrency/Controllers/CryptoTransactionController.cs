using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.DTO.Buy_SellDTO;
using CryptoCurrency.Application.DTO.BuyDTO;
using CryptoCurrency.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoCurrencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class cryptoTransactionController : ControllerBase
    {
        private readonly ICryptoTransactionInterface service;

        public cryptoTransactionController(ICryptoTransactionInterface service)
        {
            this.service = service;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy(BuyCoinDTO dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);
        
            await service.BuyCoin(dto, userId);
            return ApiResponse.Success<object>(null, "Coin Kharid Diya");
            //if (int.TryParse(userIdString, out int userId))
            //{
            //    await service.BuyCoin(dto, userId);
            //    return ApiResponse.Success<object>(null, "Coin Kharid Diya");
            //}

            //return ApiResponse.Failure("Invalid User ID");
        }

        [HttpPost("sell")]
        public async Task<IActionResult> Sell(SellCoinDTO dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);
            await service.SellCoin(dto, userId);
            return ApiResponse.Success<object>(null, "Coin Bech Diya");
        }

        [HttpGet("transactionHistory")]
        public IActionResult GetTransaction()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);
            var data = service.GetTransactionHistory(userId);
            return ApiResponse.Success<object>(data, "Transaction History Featch Syccessfully");
        }
    }
}
