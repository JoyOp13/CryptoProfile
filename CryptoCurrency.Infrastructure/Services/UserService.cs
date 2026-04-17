using AutoMapper;
using CryptoCurrency.Application.DTO.Response;
using CryptoCurrency.Application.DTO.UserDTO;
using CryptoCurrency.Application.Interface;
using CryptoCurrency.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Infrastructure.Services
{
    public class UserService : IUserInterface
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        public UserService(ApplicationDbContext db, IMapper mapper) {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<GetUserDTO> GetProfile(int userId)
        {
            var user = await db.Users
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("User not found");

            var result = mapper.Map<GetUserDTO>(user);

            return result;
        }

        public async Task<ServiceResponse> UpdateProfile(UpdateUserDTO dto, int userId)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
                throw new Exception("User not found");
            if (!string.IsNullOrWhiteSpace(dto.UserName))
                user.UserName = dto.UserName;

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                user.PhoneNumber = dto.PhoneNumber;

            user.ModifiedAt = DateTime.UtcNow;
            user.ModifiedBy = user.UserName;

            await db.SaveChangesAsync();

            return new ServiceResponse
            {
                Success = true,
                Message = "Profile updated successfully"
            };
        }
    }
}
