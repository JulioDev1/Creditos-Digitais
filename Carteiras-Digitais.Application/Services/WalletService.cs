using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using Carteiras_Digitais.Shared.Dtos;


namespace Carteiras_Digitais.Application.Services
{
    public class WalletService : IWalletService
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

        public async Task<Wallet> DepositBalanceToWallet(BalanceDto deposit)
        {
            var WalletDeposit = new Wallet
            {
                UserId = deposit.UserId,
                Balance = deposit.Balance,
            };

            var  updateBalance = await  walletRepository.IncreaseBalanceWallet(WalletDeposit);
            
            return updateBalance;
        }

        public async Task<Wallet> DiscountBalanceToWallet(BalanceDto discount)
        {
            var WalletDeposit = new Wallet
            {
                UserId = discount.UserId,
                Balance = discount.Balance,
            };

            await walletRepository.DecreaseBalanceWallet(WalletDeposit);

            return WalletDeposit;
        }

        public async Task<Wallet?> GetUserBalanceWallet(Guid Id)
        {
            return await walletRepository.GetUserWallet(Id);
        }
    }
}
