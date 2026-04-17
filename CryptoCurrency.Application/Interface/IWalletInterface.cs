using CryptoCurrency.Application.DTO.TransactionDTO;
using CryptoCurrency.Application.DTO.WalletDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrency.Application.Interface
{
    public interface IWalletInterface
    {
        void AddMoney(AddMoneyDTO dto, int userId);
        void WithdrawMoney(WithDrawDTO dto, int userId);
        WalletDTO GetWallet(int userId);
        List<WalletTransactionDTO> GetWalletHistory(int userId);
        object CreateOrder(decimal amount);
        bool VerifyPayment(string orderId, string paymentId, string signature);

    }
}
