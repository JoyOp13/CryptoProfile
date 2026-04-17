using CryptoCurrency.Application.DTO.UserDTO;
using CryptoCurrency.Application.Interface;
using CryptoCurrency.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoCurrencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly IUserInterface userInterface;

        public userController(IUserInterface userInterface)
        {
            this.userInterface = userInterface;
        }

        [HttpGet("getProfile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);
            var data = await userInterface.GetProfile(userId);

            return Ok(new
            {
                success = true,
                data
            });
        }

        [HttpPut("updateProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateUserDTO dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);
            await userInterface.UpdateProfile(dto, userId);
            return Ok(new
            {
                success = true,
                message = "Profile updated successfully"
            });
        }
    }
}
