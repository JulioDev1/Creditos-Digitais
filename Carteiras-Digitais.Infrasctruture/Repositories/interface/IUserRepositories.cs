using Carteiras_Digitais.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Infrasctruture.Repositories.@interface
{
    public interface IUserRepositories
    {
        Task<Guid> CreateUserDatabase(User user);
        Task<User?> FindUserByEmail(string email);
        Task<User> GetUserById(Guid id);

    }
}
