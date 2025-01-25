using AutoFixture;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Api.Controllers;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Moq;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task ShouldReturnCreateUserSuccessEmpty()
        {
            var InputNullPasswordField = fixture.Build<UserDto>()
                .Create();

            serviceMock.Setup(u => u.CreateUserAndWallet(InputNullPasswordField)).ReturnsAsync((Guid?)null);

            var userController = new UserController(serviceMock.Object);

            await userController.CreateUserAccount(InputNullPasswordField);

            userController.Should().NotBeNull();
        }
    }
}
