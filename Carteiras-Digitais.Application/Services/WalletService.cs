using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Application.Services
{
    public class WalletService : IWallletService
    {
        private readonly IWalletRepository walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            this.walletRepository = walletRepository;
        }

        public async Task<Guid> CreateWallet(Wallet wallet)
        {
            return await walletRepository.CreateWallet(wallet);
        }

        public Task<decimal> DepositBalanceToWallet(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
