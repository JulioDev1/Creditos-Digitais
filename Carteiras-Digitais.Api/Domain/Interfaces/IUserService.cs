using Carteiras_Digitais.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Api.Domain.Interfaces
{
    public interface IUserService
    {
        Task<Guid> CreateUserAndWallet(UserDto userDto);
    }
}
