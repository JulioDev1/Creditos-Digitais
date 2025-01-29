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
using System;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Carteiras_Digitais.Test.Controller.Tests
{
    public class WalletControllerTest
    {
        private readonly Fixture fixture;
        private readonly Mock<IWalletService> serviceMock;
        private readonly Mock<IUserRepositories> authServiceMock;
        private readonly Mock<IPasswordService> passwordServiceMock;


        public WalletControllerTest()
        {
            this.serviceMock = new Mock<IWalletService>();
            this.fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            this.authServiceMock = new Mock<IUserRepositories>();
            this.passwordServiceMock = new Mock<IPasswordService>();    
        }

        [Fact]

        public async Task ShouldReturnUnauthorizedRoute()
        {
            var controller = new WalletController(serviceMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal()
                }
            };

            var result = await controller.DepositBalanceInUserAccount(100);

            result.Should().BeOfType<UnauthorizedObjectResult>();
            
            var UnauthorizedError = result as UnauthorizedObjectResult;

            UnauthorizedError!.Value.Should().Be("user not logged");
        }

        [Fact]
        public async Task ShouldReturnBalanceWithSucces()
        {
            var login = new LoginDto
            {
                Email = "test@mail.com",
                Password ="test"

            };

            var user = fixture.Build<User>()
                .With(u => u.Email, login.Email)
                .With(u => u.Password, login.Password)
                .Create();

            

            var balance = new BalanceDto(100,user.Id);

            var controller = new WalletController(serviceMock.Object)
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

            var wallet = new Wallet
            {
                UserId = user.Id,
                Balance = balance.Balance
            };

            serviceMock.Setup(s => s.DepositBalanceToWallet(It.IsAny<BalanceDto>())).ReturnsAsync(wallet);

            var result = await controller.DepositBalanceInUserAccount(100);

            result.Should().BeOfType<OkObjectResult>();

            var resultObject = result as OkObjectResult;

            resultObject.Should().NotBeNull();
            
            resultObject.Value.Should().Be(wallet);
        }
       
    }
}
