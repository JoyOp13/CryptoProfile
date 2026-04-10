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
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteInterface favoriteInterface;

        public FavoriteController(IFavoriteInterface favoriteInterface)
        {
            this.favoriteInterface = favoriteInterface;
        }

        [HttpPost("AddToFav")]
        public async Task<IActionResult> AddFav(AddFavoriteDTO dto)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            await favoriteInterface.AddFavorite(dto, userName);
            return ApiResponse.Success<object>(null, "Added To Favorites ");

        }
        [Authorize]
        [HttpGet("GetFav")]
        public async Task<IActionResult> GetFav()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var data = await favoriteInterface.GetFavorites(userName);
            return ApiResponse.Success<object>(data, "Favorites retrieved successfully");
        }

        [HttpDelete("RemoveFav{coinId}")]
        public async Task<IActionResult> RemoveFav(int coinId)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            await favoriteInterface.RemoveFavorite(coinId,userName);
            return ApiResponse.Success<object>(null,"Removed From Favorites ");
        }

    }
}
