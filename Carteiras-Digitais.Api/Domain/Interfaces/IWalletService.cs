using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Core.Domain.Interfaces
{
    public interface IWalletService
    {
        Task<Guid> CreateWallet(Wallet wallet);
        Task<Wallet> DepositBalanceToWallet(BalanceDto deposit);
        Task<Wallet> DiscountBalanceToWallet(BalanceDto deposit);
        Task<Wallet?> GetUserBalanceWallet(Guid Id);
    }
}
