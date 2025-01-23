﻿using Carteiras_Digitais.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Infrasctruture.Repositories
{
    public interface IUserRepositories
    {
        Task<Guid> CreateUserDatabase(User user);
        
    }
}
