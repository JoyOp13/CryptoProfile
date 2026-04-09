using CryptoCurrency.Application.ApiResoponseHelper;
using CryptoCurrency.Application.DTO.FavoriteDTO;
using CryptoCurrency.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            await favoriteInterface.AddFavorite(dto);
            return ApiResponse.Success<object>(null, "Added To Favorites ");

        }

        [HttpGet("GetFav")]
        public async Task<IActionResult> GetFav()
        {
            var data = await favoriteInterface.GetFavorites();
            return ApiResponse.Success<object>(data, "Favorites retrieved successfully");
        }

        [HttpDelete("RemoveFav{coinId}")]
        public async Task<IActionResult> RemoveFav(int coinId)
        {
            await favoriteInterface.RemoveFavorite(coinId);
            return ApiResponse.Success<object>(null,"Removed From Favorites ");
        }

    }
}
