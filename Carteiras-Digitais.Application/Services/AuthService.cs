using Carteiras_Digitais.Application.Helpers;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using Carteiras_Digitais.Shared.Dtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepositories userRepositories;
        private readonly IPasswordService passwordHasher;


        public AuthService(IUserRepositories userRepositories, IPasswordService passwordHasher)
        {
            this.userRepositories = userRepositories;
            this.passwordHasher = passwordHasher;
        }

        public async Task<User?> AuthenticateUser(LoginDto login)
        {
            var userFound = await userRepositories.FindUserByEmail(login.Email);
            
            if(userFound is null)
            {
                throw new KeyNotFoundException("email not found");
            }
            var comparePassword = passwordHasher.Compare(login.Password, userFound.Password);

            if (!comparePassword)
            {
                throw new UnauthorizedAccessException("incorrectly Password");
            }

            return userFound;
        }

        private static ClaimsIdentity GenerateClaims(User user)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));

            return claims;
        }

        public string GenerateAuthToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(AuthSetting.PrivateKey);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = GenerateClaims(user)
            };

            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
    }
}
