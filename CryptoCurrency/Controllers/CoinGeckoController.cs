using CryptoCurrency.Application.DTO.PaginationDTOS;
using CryptoCurrency.Infrastructure.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(data);
        }

        [HttpPost("SyncCoins")]
        public async Task<IActionResult> Sync()
        {
            await service.SyncCoins();
            return Ok("Coins synced successfully");
        }
    }
}
