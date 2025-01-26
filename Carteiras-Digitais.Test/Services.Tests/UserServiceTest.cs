using AutoFixture;
using Carteiras_Digitais.Application.Helpers;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Moq;

namespace Carteiras_Digitais.Test.Services.Tests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepositories> userRepository;
        private readonly Mock<IWalletRepository> walletRepository;
        private readonly IPasswordService passwordService;
        private readonly Fixture fixture;

        public UserServiceTest()
        {
            userRepository = new Mock<IUserRepositories>();
            walletRepository = new Mock<IWalletRepository>();
            fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            passwordService = new PasswordService();
        }

        [Fact]
        public async Task ShouldBeReturnErrorExistsEmail()
        {
            var InputUserRegister = fixture.Create<User>();

            var WalletUser = fixture.Build<Wallet>().With(w => w.UserId, InputUserRegister.Id).Create();

            userRepository.Setup(r => r.FindUserByEmail(InputUserRegister.Email))
                .ReturnsAsync(new User { Email = InputUserRegister.Email });

            walletRepository.Setup(w => w.CreateWallet(WalletUser)).ReturnsAsync(WalletUser.Id);

            var userService = new UserService(userRepository.Object, walletRepository.Object);

            var InputError = fixture.Build<UserDto>()
                .With(u => u.Email, InputUserRegister.Email)
                .With(u => u.Password, InputUserRegister.Password)
                .With(u => u.Name, InputUserRegister.Name)
                .Create();


            Func<Task> action = async () => await userService.CreateUserAndWallet(InputError);

            await action.Should().ThrowAsync<KeyNotFoundException>();
        }
        [Fact]
        public async Task ShouldBeReturnUserInSuccessCase()
        {
            var InputUserRegister = fixture.Create<User>();

            var WalletUser = fixture.Build<Wallet>().With(w => w.UserId, InputUserRegister.Id)
                .Create();

            var InputSuccess = fixture.Build<UserDto>()
                .With(u => u.Email, InputUserRegister.Email)
                .With(u => u.Password, InputUserRegister.Password)
                .With(u => u.Name, InputUserRegister.Name)
                .Create();

            userRepository.Setup(r => r.FindUserByEmail(InputUserRegister.Email)).ReturnsAsync((User?)null);

            userRepository.Setup(r => r.CreateUserDatabase(It.IsAny<User>())).ReturnsAsync(InputUserRegister.Id);

            walletRepository.Setup(w => w.CreateWallet(WalletUser)).ReturnsAsync(WalletUser.Id);

            var userService = new UserService(userRepository.Object, walletRepository.Object);

            var action = await userService.CreateUserAndWallet(InputSuccess);

            action.Should().Be(InputUserRegister.Id);

        }
    }
}
