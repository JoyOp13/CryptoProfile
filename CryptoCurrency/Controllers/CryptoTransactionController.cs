using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.DTO.Buy_SellDTO;
using CryptoCurrency.Application.DTO.BuyDTO;
using CryptoCurrency.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoCurrencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoTransactionController : ControllerBase
    {
        private readonly ICryptoTransactionInterface service;

        public CryptoTransactionController(ICryptoTransactionInterface service)
        {
            this.service = service;
        }

        [HttpPost("Buy")]
        public async Task<IActionResult> Buy(BuyCoinDTO dto)
        {
            await service.BuyCoin(dto);
            return ApiResponse.Success<object>(null, "Coin Kharid Diya");
        }

        [HttpPost("Sell")]
        public async Task<IActionResult> Sell(SellCoinDTO dto)
        {
            await service.SellCoin(dto);
            return ApiResponse.Success<object>(null, "Coin Bech Diya");
        }

        [HttpGet("TransactionHistory")]
        public IActionResult GetTransaction()
        {
            var data = service.GetTransactionHistory();
            return ApiResponse.Success<object>(data, "Transaction History Featch Syccessfully");
        }
    }
}
