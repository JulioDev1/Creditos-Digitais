using AutoFixture;
using Carteiras_Digitais.Api.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Test.Repositories.Database
{
    public class AppDbContextFactory
    {

        public static AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Banco único para cada teste
                .Options;

            var context = new AppDbContext(options);

            // Opcional: Configure dados iniciais, se necessário
            SeedDatabase(context);

            return context;
        }

        private static void SeedDatabase(AppDbContext context)
        {
            context.users.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Email = "testuser@example.com",
                Password = "password123",                
            });

            context.SaveChanges();
        }
    }
}
