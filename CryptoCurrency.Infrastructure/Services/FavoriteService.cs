using AutoMapper;
using CryptoCurrency.Application.DTO.FavoriteDTO;
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
        public async Task AddFavorite(AddFavoriteDTO dto, int userId)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if ((user == null))
                throw new Exception("User not found");

            var exists = db.Favorite
            .Any(x => x.UserId == user.UserId && x.CryptoCoinId == dto.CryptoCoinId);

            if (exists)
                throw new Exception("Already in favorites");

            db.Favorite.Add(new Favorite
            {
                UserId = user.UserId,
                CryptoCoinId = dto.CryptoCoinId
            });

            await db.SaveChangesAsync();
        }

        public async Task<List<FavoriteResDTO>> GetFavorites(int userId)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if ((user == null))
                throw new Exception("User not found");

            var Favdata = db.Favorite.Where(x => x.UserId == user.UserId)
                .Include(x => x.CryptoCoin)
                .ToList();
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

        public async Task RemoveFavorite(int coinId, int userId)
        {
            //userName = "jay";
            //var user = db.Users.FirstOrDefault(x => x.UserName == userName);
            var user = db.Users.FirstOrDefault(x => x.UserId == userId);
            if (user == null) throw new Exception("User not found");

            var fav = db.Favorite
                .FirstOrDefault(x => x.UserId == user.UserId && x.CryptoCoinId == coinId);

            if (fav == null)
                throw new Exception("Favorite not found");

            db.Favorite.Remove(fav);

            await db.SaveChangesAsync(); ;
        }
    }
}
