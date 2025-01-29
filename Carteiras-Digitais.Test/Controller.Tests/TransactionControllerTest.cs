using AutoFixture;
using Carteiras_Digitais.Api.Controllers;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Carteiras_Digitais.Test.Controller.Tests
{
    public class TransactionControllerTest
    {
        private readonly Fixture fixture;
        private readonly Mock<ITransactionService> serviceMock;
        private readonly Mock<IUserRepositories> authServiceMock;
        private readonly Mock<IPasswordService> passwordServiceMock;


        public TransactionControllerTest()
        {
            this.serviceMock = new Mock<ITransactionService>();
            this.authServiceMock = new Mock<IUserRepositories>();
            this.passwordServiceMock = new Mock<IPasswordService>();
            this.fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        [Fact]
        public async Task ShouldBeReturnSuccessAndTransaction()
        {
            var login = new LoginDto
            {
                Email = "test@mail.com",
                Password = "test"

            };

            var user = fixture.Build<User>()
                .With(u => u.Email, login.Email)
                .With(u => u.Password, login.Password)
                .Create();

            var controller = new TransactionController(serviceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{ new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        }, "mock"))
                    }

                }
            };
            var transaction = fixture.Create<TransactionDto>();

            serviceMock.Setup(w => w.TransactionToBalanceToReceiver(It.IsAny<TransactionDto>()))
                .ReturnsAsync(transaction);


            var result = await controller.TransactionBetweenUsers(transaction);

            result.Should().BeOfType<OkObjectResult>();

            var resultAsObject = result as OkObjectResult;

            resultAsObject!.Value.Should().Be(transaction);
        }

        [Fact]
        public async Task ShouldBeInvalidOperationInterceptor()
        {
            var login = new LoginDto
            {
                Email = "test@mail.com",
                Password = "test"

            };

            var user = fixture.Build<User>()
                .With(u => u.Email, login.Email)
                .With(u => u.Password, login.Password)
                .Create();

            var controller = new TransactionController(serviceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{ new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        }, "mock"))
                    }

                }
            };
            
            serviceMock.Setup(w => w.TransactionToBalanceToReceiver(It.IsAny<TransactionDto>()))
                .ThrowsAsync(new InvalidOperationException("value insuficient"));

            var transaction = fixture.Create<TransactionDto>();

            var result = await controller.TransactionBetweenUsers(transaction);
           
            result.Should().BeOfType<UnauthorizedObjectResult>();

            var resultAsObject = result as UnauthorizedObjectResult;

            resultAsObject!.Value.Should().Be("value insuficient");
        }
        [Fact]
        public async Task ShouldBeErrorKeyNotFound()
        {
            var login = new LoginDto
            {
                Email = "test@mail.com",
                Password = "test"

            };

            var user = fixture.Build<User>()
                .With(u => u.Email, login.Email)
                .With(u => u.Password, login.Password)
                .Create();

            var controller = new TransactionController(serviceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]{ new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        }, "mock"))
                    }

                }
            };

            serviceMock.Setup(w => w.TransactionToBalanceToReceiver(It.IsAny<TransactionDto>()))
                .ThrowsAsync(new KeyNotFoundException("user not exists"));

            var transaction = fixture.Create<TransactionDto>();

            var result = await controller.TransactionBetweenUsers(transaction);

            result.Should().BeOfType<NotFoundObjectResult>();

            var resultAsObject = result as NotFoundObjectResult;

            resultAsObject!.Value.Should().Be("user not exists");

        }

    }
}
