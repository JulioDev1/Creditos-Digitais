using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Database;
using Carteiras_Digitais.Shared.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Infrasctruture.Repositories
{
    public class DataSeeding
    {
        private readonly AppDbContext context;
        private readonly PasswordSave password;

        public DataSeeding(AppDbContext context)
        {
            this.context = context;
            this.password = new PasswordSave();
        }
        public void Seed()
        {
            if(IsDatabaseConnected())
            {

                SeedUsers();
                SeedWallets();
                SeedTransactions();
            }
        }

        private bool IsDatabaseConnected()
        {
            try
            {
                return context.Database.CanConnect();
            }
            catch
            {
                return false;
            }
        }

        public void SeedUsers()
        {
            if (!context.users.Any())
            {
                IEnumerable<User> users = new List<User>()
                {
                    new User()
                    {
                        Id = Guid.Parse("60935b2a-f96e-412e-bdab-025a97dfe67f"),
                        Name = "testName",
                        Email = "test@mail.com",
                        Password = password.Hasher("test") 
                    },
                    new User()
                    {
                        Id = Guid.Parse("8f653fb3-f868-4fe3-b22e-8d68c659d2d8"),
                        Name = "userName",
                        Email = "user@mail.com",
                        Password = password.Hasher("test") 
                    }
                };

                context.users.AddRange(users);
                context.SaveChanges();
            }
        }

        public void SeedWallets()
        {
            if (!context.wallets.Any())
            {
                IEnumerable<Wallet> wallets = new List<Wallet>()
                {
                    new Wallet()
                    {
                        Id = Guid.Parse("bbfe0b99-4d14-4b69-a533-25be66975947"),
                        UserId = Guid.Parse("60935b2a-f96e-412e-bdab-025a97dfe67f"),
                        Balance = 100,
                    },
                    new Wallet()
                    {
                        Id = Guid.Parse("7f9efaa3-f0a3-4364-aa8a-816ab961f462"),
                        UserId = Guid.Parse("8f653fb3-f868-4fe3-b22e-8d68c659d2d8"),
                        Balance = 100,
                    }
                };

                context.wallets.AddRange(wallets);
                context.SaveChanges();
            }
        }

        public void SeedTransactions()
        {
            if (!context.transactions.Any())
            {
                IEnumerable<Transaction> transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Id = Guid.Parse("e2c20c25-36f6-4c64-ac7b-34e016fa2bd0"),
                        SenderWalletId = Guid.Parse("bbfe0b99-4d14-4b69-a533-25be66975947"),
                        ReceiverWalletId = Guid.Parse("7f9efaa3-f0a3-4364-aa8a-816ab961f462"),
                        Amount = 100,
                        Description = "testSender quantity 100$ to userTest",
                       
                    },
                    new Transaction()
                    {
                        Id = Guid.Parse("2c8822d8-425e-44f2-85f9-8ad17566848a"),
                        SenderWalletId = Guid.Parse("7f9efaa3-f0a3-4364-aa8a-816ab961f462"),
                        ReceiverWalletId = Guid.Parse("bbfe0b99-4d14-4b69-a533-25be66975947"),
                        Amount = 100,
                        Description = "userSender quantity 100$ to userTest",
                    }
                };

                context.transactions.AddRange(transactions);
                context.SaveChanges();
            }
        }
    }
}
    

