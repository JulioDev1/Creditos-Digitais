using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Database;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using Microsoft.EntityFrameworkCore;


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

        public async Task<User> GetUserById(Guid Id)
        {
            return await context.users.FirstAsync(u=> u.Id == Id);
        }
    }
}
