using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.DTO.FavoriteDTO;
using CryptoCurrency.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CryptoCurrencyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class favoriteController : ControllerBase
    {
        private readonly IFavoriteInterface favoriteInterface;

        public favoriteController(IFavoriteInterface favoriteInterface)
        {
            this.favoriteInterface = favoriteInterface;
        }
        [Authorize]
        [HttpPost("addToFav")]
        public async Task<IActionResult> AddFav(AddFavoriteDTO dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);
            await favoriteInterface.AddFavorite(dto, userId);
            return ApiResponse.Success<object>(null, "Added To Favorites ");

        }
        [Authorize]
        [HttpGet("getFav")]
        public async Task<IActionResult> GetFav()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);
            var data = await favoriteInterface.GetFavorites(userId);
            return ApiResponse.Success<object>(data, "Favorites retrieved successfully");
        }
        [Authorize]
        [HttpDelete("removeFav/{coinId}")]
        public async Task<IActionResult> RemoveFav(int coinId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);
            await favoriteInterface.RemoveFavorite(coinId,userId);
            return ApiResponse.Success<object>(null,"Removed From Favorites ");
        }

    }
}
