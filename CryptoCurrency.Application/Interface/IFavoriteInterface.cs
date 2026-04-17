using CryptoCurrency.Application.DTO.FavoriteDTO;
using CryptoCurrency.Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.Interface
{
    public interface IFavoriteInterface
    {
        Task<ServiceResponse> AddFavorite(AddFavoriteDTO dto, int userId);
        Task<List<FavoriteResDTO>> GetFavorites(int userId);
        Task<ServiceResponse> RemoveFavorite(int coinId, int userId);
    }
}
