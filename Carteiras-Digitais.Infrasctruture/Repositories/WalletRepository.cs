using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Database;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using Microsoft.EntityFrameworkCore;


namespace Carteiras_Digitais.Infrasctruture.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext context;

        public WalletRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> CreateWallet(Wallet wallet)
        {
            context.wallets.Add(wallet);
            
            await context.SaveChangesAsync();
            
            return wallet.Id;
        }

        public async Task<Wallet> GetUserWallet(Guid userId)
        {
            return await context.wallets.FirstAsync(w => w.UserId == userId);
        }

        public async Task<Wallet> DecreaseBalanceWallet(Wallet wallet)
        {
            var entities = context.wallets.Where(w => w.UserId == wallet.UserId).First();

            entities.Balance -= wallet.Balance;

            await context.SaveChangesAsync();

            return entities;

        }

        public async Task<Wallet> IncreaseBalanceWallet(Wallet wallet)
        {
            var entities = context.wallets.Where(w => w.UserId == wallet.UserId).First();

             entities.Balance += wallet.Balance;
            
            await context.SaveChangesAsync();
           
            return entities;
        
        }
    }
}
