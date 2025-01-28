using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Shared.Helper
{
    public class PasswordSave
    {
        public string Hasher(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
