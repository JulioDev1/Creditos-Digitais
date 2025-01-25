using AutoFixture;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Moq;

namespace Carteiras_Digitais.Test.Services.Tests
{
    public class AuthencationServiceTests
    {
        private readonly Mock<IUserRepositories> userRepository;
        private readonly Mock<IPasswordService> passwordService;
        private readonly Fixture fixture;

        public AuthencationServiceTests()
        {
            this.userRepository = new Mock<IUserRepositories>();
            this.passwordService = new Mock<IPasswordService>();
            this.fixture = new Fixture();
            
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task ShouldBeReturnErrorNotFoundedEmail()
        {
            var InputEmailNotFound = fixture.Create<LoginDto>();

            userRepository.Setup(r => r.FindUserByEmail(InputEmailNotFound.Email))
                .ReturnsAsync((User?)null);
            
            var authenticationService = new AuthService(userRepository.Object, passwordService.Object);

            Func<Task> action = async () => await authenticationService.AuthenticateUser(InputEmailNotFound);

            await action.Should().ThrowAsync<KeyNotFoundException>();
        }
        [Fact]
        public async Task ShouldBeReturnErrorWhenPasswordNotMatch()
        {
            var InputPasswordIncorrect = fixture.Create<LoginDto>();
            
            var userWasFound = fixture.Build<User>()
                .With(u => u.Email, InputPasswordIncorrect.Email)
                .Create();

            userRepository.Setup(r => r.FindUserByEmail(InputPasswordIncorrect.Email))
                .ReturnsAsync((userWasFound));

            passwordService.Setup(p => p.Compare(InputPasswordIncorrect.Password, userWasFound.Password)).Returns(false);
            
            var authenticationService = new AuthService(userRepository.Object, passwordService.Object);

            Func<Task> action = async () => await authenticationService.AuthenticateUser(InputPasswordIncorrect);

            await action.Should().ThrowAsync<UnauthorizedAccessException>("incorrectly Password");
        }
        [Fact]
        public async Task ShouldReturnSuccesAndLoginUser()
        {
            var InputSuccess = fixture.Create<LoginDto>();

            var userWasFound = fixture.Build<User>()
                .With(u => u.Email, InputSuccess.Email)
                .With(u => u.Password, InputSuccess.Password)
                .Create();

            userRepository.Setup(r => r.FindUserByEmail(InputSuccess.Email))
                .ReturnsAsync((userWasFound));

            passwordService.Setup(p => p.Compare(InputSuccess.Password, userWasFound.Password)).Returns(true);

            var authenticationService = new AuthService(userRepository.Object, passwordService.Object);

            var userLogged = await authenticationService.AuthenticateUser(InputSuccess);

            userLogged.Should().Be(userWasFound);
        }
    }
}
