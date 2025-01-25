using Carteiras_Digitais.Core.Domain.Interfaces;


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
