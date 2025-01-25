using Carteiras_Digitais.Core.Domain.Models;

namespace Carteiras_Digitais.Infrasctruture.Repositories.@interface
{
    public interface IWalletRepository
    {
        Task<Guid> CreateWallet(Wallet wallet);
    }
}
