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
    public class coinGeckoController : ControllerBase
    {
        private readonly CoinGekoService service;

        public coinGeckoController(CoinGekoService service)
        {
            this.service = service;
        }

        [HttpGet("getCoins")]
        public async Task<IActionResult> GetMarket([FromQuery] CoinsPaginationDTO dto)
        {
            var data = await service.GetCoins(dto);
            return ApiResponse.Success<object>(data,"Crypto Coins Featch Syccessfully");
        }

        [HttpPost("syncCoins")]
        public async Task<IActionResult> Sync()
        {
            await service.SyncCoins();
            return ApiResponse.Success<object>(null, "Crypto Coins Sync Syccessfully");
        }

        [HttpGet("GetCoinsData")]
        public async Task<IActionResult> GetCoins()
        {
            var data = await service.GetCoinsData(); 
            return ApiResponse.Success<object>(data, "Crypto Coins Featch Syccessfully");
        }
    }
}
