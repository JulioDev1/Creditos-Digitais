using Carteiras_Digitais.Application.Helpers;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using Carteiras_Digitais.Shared.Dtos;

namespace Carteiras_Digitais.Application.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepositories userRepositories;
        private readonly IWallletService walletService;
        private readonly IPasswordService passwordService;


        public UserService(IUserRepositories userRepositories, IWallletService walletService)
        {
            this.userRepositories = userRepositories;
            this.passwordService = new PasswordService();
            this.walletService = walletService;
        }

        public async Task<Guid?> CreateUserAndWallet(UserDto userDto)
        {
            var userExists = await userRepositories.FindUserByEmail(userDto.Email);

            if (userExists is not null) {
                throw new UnauthorizedAccessException("user already exists");
            }

            userDto.Password = passwordService.Hasher(userDto.Password);

            var newUser = new User
            {
                Email = userDto.Email,
                Name = userDto.Name,
                Password = userDto.Password,
            };

            var createUser = await userRepositories.CreateUserDatabase(newUser);

            var createUserWallet = new Wallet
            {
                UserId = createUser
            };

            await walletService.CreateWallet(createUserWallet);

            return createUser;
        }

        public async Task<User> GetUserById(Guid Id)
        {
            return await userRepositories.GetUserById(Id);
        }
    }
}
