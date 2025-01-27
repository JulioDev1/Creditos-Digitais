using Carteiras_Digitais.Core.Domain.Models;

namespace Carteiras_Digitais.Infrasctruture.Repositories.@interface
{
    public interface IWalletRepository
    {
        Task<Guid> CreateWallet(Wallet wallet);
        Task<Wallet> IncreaseBalanceWallet(Wallet wallet);
        Task<Wallet> GetUserWallet(Guid userId);
        Task<Wallet> DecreaseBalanceWallet(Wallet wallet);

    }
}
