using AutoFixture;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Api.Controllers;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Moq;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Carteiras_Digitais.Core.Domain.Models;

namespace Carteiras_Digitais.Test.Controller.Tests
{
    public class UserControllerTest
    {
        private readonly Fixture fixture;
        private readonly Mock<IUserService> serviceMock;
        public UserControllerTest() 
        {
            this.serviceMock = new Mock<IUserService>();
            this.fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        [Fact]
        public async Task ShouldReturnCreateUserSuccess()
        {
            var InputNullPasswordField = fixture.Build<UserDto>()
                .Create();

            var IdUser = Guid.NewGuid();

            serviceMock.Setup(u => u.CreateUserAndWallet(It.IsAny<UserDto>())).ReturnsAsync((IdUser));

            var userController = new UserController(serviceMock.Object);

            var action = await userController.CreateUserAccount(InputNullPasswordField);

            action.Should().BeOfType<OkObjectResult>();

            var result = action as OkObjectResult;

            result.Should().NotBeNull();

            result.Value.Should().Be(IdUser);
        }
        [Fact]
        public async Task ShouldReturnUserAlreadyExists()
        {
            var InputNullPasswordField = fixture.Build<UserDto>()
                .Create();

            var IdUser = Guid.NewGuid();

            serviceMock.Setup(u => u.CreateUserAndWallet(It.IsAny<UserDto>())).ThrowsAsync(new UnauthorizedAccessException("user already exists"));

            var userController = new UserController(serviceMock.Object);

            var result = await userController.CreateUserAccount(InputNullPasswordField);

            result.Should().BeOfType<UnauthorizedObjectResult>();

            var UnauthorizedError = result as UnauthorizedObjectResult;

            UnauthorizedError.Should().NotBeNull();

            UnauthorizedError.Value.Should().Be("user already exists");
        }

    }
}
