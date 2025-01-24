using BCrypt.Net;
using Carteiras_Digitais.Api.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Application.Helpers
{
    public class PasswordService : IPasswordService
    {
        public bool Compare(string password, string comparePassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, comparePassword);
        }

        public string Hasher(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
