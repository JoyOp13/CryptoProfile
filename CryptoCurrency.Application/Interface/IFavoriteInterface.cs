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
        Task AddFavorite(AddFavoriteDTO dto, string userName);
        Task<List<FavoriteResDTO>> GetFavorites(string userName);
        Task RemoveFavorite(int coinId, string userName);
    }
}
