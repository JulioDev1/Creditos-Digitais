using Carteiras_Digitais.Api.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
