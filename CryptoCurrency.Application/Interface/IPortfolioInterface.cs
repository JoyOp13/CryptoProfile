using CryptoCurrency.Application.DTO.ProfileResDTO;
using CryptoCurrency.Application.DTO.TransactionDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.Interface
{
    public interface IPortfolioInterface
    {
        Task<List<PortfolioDTO>> GetProfileResponse();
    }
}
