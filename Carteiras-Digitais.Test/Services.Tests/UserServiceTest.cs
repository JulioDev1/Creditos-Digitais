using AutoFixture;
using Carteiras_Digitais.Api.Domain.Interfaces;
using Carteiras_Digitais.Api.Domain.Models;
using Carteiras_Digitais.Application.Helpers;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Infrasctruture.Repositories;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Test.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepositories> userRepository;
        private readonly Mock<IWalletRepository> walletRepository;
        private readonly IPasswordService passwordService;
        private readonly Fixture fixture;

        public UserServiceTest()
        {
            this.userRepository = new Mock<IUserRepositories>();
            this.walletRepository = new Mock<IWalletRepository>();  
            this.fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            this.passwordService = new PasswordService();
        }

        [Fact]
        public async Task ShouldBeReturnErrorExistsEmail()
        {
            var InputUserRegister = fixture.Create<User>();

            var WalletUser = fixture.Build<Wallet>()
                .With(w => w.UserId, InputUserRegister.Id)
                .Create();

            userRepository.Setup(r => r.FindUserByEmail(InputUserRegister.Email)).ReturnsAsync((User?) null);

            walletRepository.Setup(w => w.CreateWallet(WalletUser)).ReturnsAsync(WalletUser.Id);

            var userService = new UserService(userRepository.Object, walletRepository.Object);

            var InputError = fixture.Build<UserDto>()
                .With(u => u.Email, InputUserRegister.Email)
                .With(u => u.Password, InputUserRegister.Password)
                .With(u => u.Name, InputUserRegister.Name)
                .Create();


            Func<Task> action = async ()=> await userService.CreateUserAndWallet(InputError);

            await action.Should().ThrowAsync<Exception>();
        }
    }
}
