using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoCurrencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class portfolioController : ControllerBase
    {
        private readonly IPortfolioInterface service;
        public portfolioController(IPortfolioInterface service) {
            this.service = service;
        }
        [HttpGet("getPortfolio")]
        public async Task<IActionResult> GetPortfolio()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);
            var data = await service.GetProfileResponse(userId);
            return ApiResponse.Success<object>(data, "Portfolio Data Featch Syccessfully");
        }
    }
}
