using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.DTO.WalletDTO;
using CryptoCurrency.Application.Interface;
using CryptoCurrency.Infrastructure.Services;
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
        

        public walletController(IWalletInterface walletService  )
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

        [HttpPost("createorder")]
        public IActionResult CreateOrder([FromBody] decimal amount)
        {
            var order = walletService.CreateOrder(amount);
            return Ok(order);
        }

        [HttpPost("verifypayment")]
        public IActionResult VerifyPayment(PaymentDTO dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);

            var isValid = walletService.VerifyPayment(
                dto.OrderId,
                dto.PaymentId,
                dto.Signature
            );

            if (!isValid)
                return BadRequest(new
                {
                    success = false,
                    message = "Payment failed"
                });

            walletService.AddMoney(new AddMoneyDTO
            {
                Amount = dto.Amount
            }, userId);

            return Ok(new
            {
                success = true,
                message = "Payment successful"
            });
        }

        [HttpGet("balance")]
        public IActionResult GetBalance()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);

            var data = walletService.GetWallet(userId);
            return ApiResponse.Success<object>(data, "Wallet Balance Featch Successfully");
        }

        [HttpGet("transactionHistory")]
        public IActionResult GetTransaction()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);
            var data = walletService.GetWalletHistory(userId);
            return ApiResponse.Success<object>(data, "Transaction History Featch Syccessfully");
        }
    }
}
