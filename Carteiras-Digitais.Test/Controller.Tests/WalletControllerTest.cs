using AutoFixture;
using Carteiras_Digitais.Api.Controllers;
using Carteiras_Digitais.Core.Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;
using System;
using System.Security.Claims;

namespace Carteiras_Digitais.Test.Controller.Tests
{
    public class WalletControllerTest
    {
        private readonly Fixture fixture;
        private readonly Mock<IWalletService> serviceMock;
   
        public WalletControllerTest()
        {
            this.serviceMock = new Mock<IWalletService>();
            this.fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
           
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
    }
}
