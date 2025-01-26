using Carteiras_Digitais.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Core.Domain.Interfaces
{
    public interface IWallletService
    {
        Task<Guid> CreateWallet(Wallet wallet);
        Task<decimal> DepositBalanceToWallet(Guid Id);
    }
}
