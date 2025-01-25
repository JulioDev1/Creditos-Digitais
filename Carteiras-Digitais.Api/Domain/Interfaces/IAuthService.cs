using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Core.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<User?> AuthenticateUser(LoginDto login);
        string GenerateAuthToken(User user);

    }
}
