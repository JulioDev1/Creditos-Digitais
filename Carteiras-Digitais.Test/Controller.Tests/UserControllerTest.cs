using AutoFixture;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Controllers;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Test.Controller.Tests
{
    public class UserControllerTest
    {
        private readonly Fixture fixture;
        private readonly Mock<UserService> serviceMock;
        public UserControllerTest() 
        {
            this.serviceMock = new Mock<UserService>();
            this.fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        [Fact]
        public async Task ShouldReturnErrorWhenFieldsEmpty()
        {
            var InputNullEmailField = fixture.Build<UserDto>()
                .With(u => u.Email, string.Empty)
                .Create();

            serviceMock.Setup(u => u.CreateUserAndWallet(InputNullEmailField)).ReturnsAsync((Guid?)null);

            var userController = new UserController(serviceMock.Object);
           
            Func<Task> controllerAction = async () => await userController.CreateUserAccount(InputNullEmailField);
           
            await controllerAction.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
