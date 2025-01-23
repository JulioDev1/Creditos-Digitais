using Carteiras_Digitais.Api.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Infrasctruture.Repositories
{
    public class UserRepository : IUserRepositories
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context) 
        { 
            this.context = context; 
        }
        public async Task<Guid> CreateUserDatabase(User user)
        {
            context.users.Add(user);
            
            await context.SaveChangesAsync();
            
            return user.Id;
        }

        public async Task<User?> FindUserByEmail(string email)
        {
           return await context.users.FirstOrDefaultAsync(e=> e.Email == email);
        }
    }
}
