using AutoFixture;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories;
using Carteiras_Digitais.Test.Repositories.Tests.Database;
using FluentAssertions;


namespace Carteiras_Digitais.Test.Repositories.Tests
{
    public class WalletRepositoryTests
    {
        private readonly Fixture fixture;

        public WalletRepositoryTests()
        {
            fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]

        public async Task ShouldBeCreateWalletAndAttriButteToUser()
        {
            var context = AppDbContextFactory.CreateInMemoryDbContext();

            var WalletRepository = new WalletRepository(context);

            var UserRepository = new UserRepository(context);

            var userMock = fixture.Create<User>();

            var UserCreated = await UserRepository.CreateUserDatabase(userMock);

            var walletMock = fixture.Build<Wallet>().With(w => w.UserId, userMock.Id).Create();

            var walletCreated = await WalletRepository.CreateWallet(walletMock);

            walletCreated.Should().Be(walletMock.Id);
        }
    }
}
