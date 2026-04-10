using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoCurrencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioInterface service;
        public PortfolioController(IPortfolioInterface service) {
            this.service = service;
        }
        [HttpGet("GetPortfolio")]
        public async Task<IActionResult> GetPortfolio()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var data = await service.GetProfileResponse(userName);
            return ApiResponse.Success<object>(data, "Portfolio Data Featch Syccessfully");
        }
    }
}
