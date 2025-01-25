using AutoFixture;
using Carteiras_Digitais.Api.Controllers;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Test.Controller.Tests
{
    public class AuthControllerTest
    {
        private readonly Fixture fixture;
        private readonly Mock<IAuthService> serviceMock;
        public AuthControllerTest()
        {
            this.serviceMock = new Mock<IAuthService>();
            this.fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task ShouldBeReturnNotFoundEmailError()
        {
            var InputSuccess = fixture.Create<LoginDto>();
            
            serviceMock.Setup(a => a.AuthenticateUser(InputSuccess)).ThrowsAsync(new KeyNotFoundException("email not found"));

            var controller = new AuthController(serviceMock.Object);
            
            var result = await controller.CreateUser(InputSuccess);


            result.Should().BeOfType<NotFoundObjectResult>();

            var resultAsObject = result as NotFoundObjectResult;
            
            resultAsObject.Should().NotBeNull(); 
            
            resultAsObject.Value.Should().Be("email not found");
        }
        [Fact]
        public async Task ShouldBeReturnErrorWhenPasswordIsWrong()
        {
            var InputSuccess = fixture.Create<LoginDto>();

            serviceMock.Setup(a => a.AuthenticateUser(InputSuccess)).ThrowsAsync(new UnauthorizedAccessException("incorrectly Password"));

            var controller = new AuthController(serviceMock.Object);

            var result = await controller.CreateUser(InputSuccess);

            result.Should().BeOfType<UnauthorizedObjectResult>();

            var resultAsObject = result as UnauthorizedObjectResult;

            resultAsObject.Should().NotBeNull();

            resultAsObject.Value.Should().Be("incorrectly Password");
        }
    }
}
