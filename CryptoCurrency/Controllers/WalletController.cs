using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.DTO.WalletDTO;
using CryptoCurrency.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoCurrencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class walletController : ControllerBase
    {
        private readonly IWalletInterface walletService;

        public walletController(IWalletInterface walletService)
        {
            this.walletService = walletService;
        }

        [HttpPost("addMoney")]
        public IActionResult AddMoney(AddMoneyDTO dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);

            walletService.AddMoney(dto, userId);
            return ApiResponse.Success<object>(null, "Money Added successfully");
        }

        [HttpPost("withDraw")]
        public IActionResult Withdraw(WithDrawDTO dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);

            walletService.WithdrawMoney(dto, userId);
            return ApiResponse.Success<object>(null, "Money Withdraw successfully");
        }

        [HttpGet("balance")]
        public IActionResult GetBalance()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);

            var data = walletService.GetWallet(userId);
            return ApiResponse.Success<object>(data, "Wallet Balance Featch Successfully");
        }
    }
}
