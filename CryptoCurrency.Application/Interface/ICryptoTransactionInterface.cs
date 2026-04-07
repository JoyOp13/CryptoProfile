using CryptoCurrency.Application.DTO.Buy_SellDTO;
using CryptoCurrency.Application.DTO.BuyDTO;
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
        Task BuyCoin(BuyCoinDTO dto);
        Task SellCoin(SellCoinDTO dto);
        List<TransactionHistoryDTO> GetTransactionHistory();
        
    }
}
