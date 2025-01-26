using AutoFixture;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories.@interface;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Test.Services.Tests
{
    public class WalletServicesTests
    {
        private readonly Mock<IWalletRepository> walletRepository;
        private readonly Fixture fixture;


        public WalletServicesTests()
        {
            fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            walletRepository = new Mock<IWalletRepository>();
        }

        [Fact]
        public async Task ShouldReturnUserWallet()
        {
            var Wallet = fixture.Create<Wallet>();

            walletRepository.Setup(w=> w.GetUserWallet(Wallet.Id)).ReturnsAsync(Wallet);

            var walletService = new WalletService(walletRepository.Object);

            var action = await walletService.GetUserBalanceWallet(Wallet.Id);  

            action.Should().Be(Wallet);

        }
    }
}
