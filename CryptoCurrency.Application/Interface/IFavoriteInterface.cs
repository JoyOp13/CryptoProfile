using CryptoCurrency.Application.DTO.FavoriteDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.Interface
{
    public interface IFavoriteInterface
    {
        Task AddFavorite(AddFavoriteDTO dto);
        Task<List<FavoriteResDTO>> GetFavorites();
        Task RemoveFavorite(int coinId);
    }
}
