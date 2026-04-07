using CryptoCurrency.Application.DTO.WalletDTO;
using CryptoCurrency.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoCurrencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletInterface walletService;

        public WalletController(IWalletInterface walletService)
        {
            this.walletService = walletService;
        }

        [HttpPost("AddMoney")]
        public IActionResult AddMoney(AddMoneyDTO dto)
        {
            var user = User.FindFirst(ClaimTypes.Name)?.Value ?? "jay";

            walletService.AddMoney(dto, user);
            return Ok(new { message = "Money added successfully" });
        }

        [HttpPost("WithDraw")]
        public IActionResult Withdraw(WithDrawDTO dto)
        {
            var user = User.FindFirst(ClaimTypes.Name)?.Value ?? "jay";

            walletService.WithdrawMoney(dto, user);
            return Ok(new { message = "Money withdrawn successfully" });
        }

        [HttpGet("Balance")]
        public IActionResult GetBalance()
        {
            var user = User.FindFirst(ClaimTypes.Name)?.Value ?? "demoUser";

            var data = walletService.GetWallet(user);
            return Ok(data);
        }
    }
}
