using CryptoCurrency.Application.DTO.Buy_SellDTO;
using CryptoCurrency.Application.DTO.BuyDTO;
using CryptoCurrency.Application.DTO.Response;
using CryptoCurrency.Application.DTO.TransactionDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.Interface
{
    public interface ICryptoTransactionInterface
    {
        Task<ServiceResponse> BuyCoin(BuyCoinDTO dto, int userId);
        Task <ServiceResponse> SellCoin(SellCoinDTO dto, int userId);
        List<TransactionHistoryDTO> GetTransactionHistory(int userId);
        
    }
}
