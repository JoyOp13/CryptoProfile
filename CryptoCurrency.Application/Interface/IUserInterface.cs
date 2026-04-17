using CryptoCurrency.Application.DTO.Response;
using CryptoCurrency.Application.DTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.Interface
{
    public interface IUserInterface
    {
        Task<GetUserDTO> GetProfile(int userId);
        Task<ServiceResponse> UpdateProfile(UpdateUserDTO dto, int userId);
    }
}
