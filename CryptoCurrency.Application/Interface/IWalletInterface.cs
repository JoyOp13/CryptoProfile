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
        void AddMoney(AddMoneyDTO dto, string userName);
        void WithdrawMoney(WithDrawDTO dto, string userName);
        WalletDTO GetWallet(string userName);
    }
}
