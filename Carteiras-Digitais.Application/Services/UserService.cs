using Carteiras_Digitais.Api.Domain.Interfaces;
using Carteiras_Digitais.Api.Domain.Models;
using Carteiras_Digitais.Application.Helpers;
using Carteiras_Digitais.Infrasctruture.Repositories;
using Carteiras_Digitais.Shared.Dtos;

namespace Carteiras_Digitais.Application.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepositories userRepositories;
        private readonly IWalletRepository walletRepository;
        private readonly IPasswordService passwordService;


        public UserService(IUserRepositories userRepositories, IWalletRepository walletRepository)
        {
            this.userRepositories = userRepositories;
            this.walletRepository = walletRepository;
            this.passwordService = new PasswordService();
        }

        public async Task<Guid> CreateUserAndWallet(UserDto userDto)
        {
            var userExists = await userRepositories.FindUserByEmail(userDto.Email);

            if (userExists is not null) {
                throw new Exception("user already exists");
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

            await walletRepository.CreateWallet(createUserWallet);

            return createUser;
        }
    }
}
