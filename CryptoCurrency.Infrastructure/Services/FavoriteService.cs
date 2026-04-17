using AutoMapper;
using CryptoCurrency.Application.DTO.FavoriteDTO;
using CryptoCurrency.Application.DTO.Response;
using CryptoCurrency.Application.Interface;
using CryptoCurrency.Domain.Models;
using CryptoCurrency.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Infrastructure.Services
{
    public class FavoriteService : IFavoriteInterface
    {
        ApplicationDbContext db;
        IMapper mapper;
        CoinGekoService coinGeko;
        public FavoriteService(ApplicationDbContext db, IMapper mapper, CoinGekoService coinGeko) { 
         this.db = db;
         this.mapper = mapper;
         this.coinGeko = coinGeko;
        }
        public async Task<ServiceResponse> AddFavorite(AddFavoriteDTO dto, int userId)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if ((user == null))
                return new ServiceResponse
                {
                    Message = "User Not Found"
                };

            var exists = db.Favorite.Any(x => x.UserId == user.UserId && x.CryptoCoinId == dto.CryptoCoinId
                            && x.DeletedAt == null);

            if (exists)
                return new ServiceResponse
                {
                    Message = "Already in favorites"
                };
            
            db.Favorite.Add(new Favorite
            {
                UserId = user.UserId,
                CryptoCoinId = dto.CryptoCoinId
            });

            await db.SaveChangesAsync();
            return new ServiceResponse { Success = true, Message = "Favorite added successfully" };
        }

        public async Task<List<FavoriteResDTO>> GetFavorites(int userId)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if ((user == null))
                throw new Exception("User not found");

            var Favdata = await db.Favorite.Where(x => x.UserId == user.UserId && x.DeletedAt== null)
                .Include(x => x.CryptoCoin)
                .ToListAsync();
            var result = new List<FavoriteResDTO>();

            foreach (var item in Favdata)
            {
                var dto = mapper.Map<FavoriteResDTO>(item);

                var price = db.CryptoCoin.Where(x => x.CryptoCoinId == dto.CryptoCoinId).Select(x => x.CurrentPrice).FirstOrDefault();

                dto.CurrentPrice = price;
                result.Add(dto);
            }

            return result;
        }

        public async Task<ServiceResponse> RemoveFavorite(int coinId, int userId)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
                return new ServiceResponse
                {
                    Message = "User Not Found"
                };

            var fav = await db.Favorite
                .FirstOrDefaultAsync(x => x.UserId == user.UserId && x.CryptoCoinId == coinId && x.DeletedAt == null); 

            if (fav == null)
                return new ServiceResponse
                {
                    Message = "Favorite not found"
                };
            fav.DeletedAt = DateTime.UtcNow;
            fav.DeletedBy = userId.ToString();

            db.Favorite.Update(fav);

            await db.SaveChangesAsync();
            return new ServiceResponse { Success = true, Message = "Favorite removed successfully" };
        }
    }
}
