using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.DTO.PaginationDTOS;
using CryptoCurrency.Infrastructure.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CryptoCurrencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinGeckoController : ControllerBase
    {
        private readonly CoinGekoService service;

        public CoinGeckoController(CoinGekoService service)
        {
            this.service = service;
        }

        [HttpGet("GetCoins")]
        public async Task<IActionResult> GetMarket([FromQuery] CoinsPaginationDTO dto)
        {
            var data = await service.GetCoins(dto);
            return ApiResponse.Success<object>(data,"Crypto Coins Featch Syccessfully");
        }

        [HttpPost("SyncCoins")]
        public async Task<IActionResult> Sync()
        {
            await service.SyncCoins();
            return ApiResponse.Success<object>(null, "Crypto Coins Sync Syccessfully");
        }
    }
}
