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
        [Fact]
        public async Task ShouldBeIncreaseBalanceAccount()
        {

            var context = AppDbContextFactory.CreateInMemoryDbContext();

            var WalletRepository = new WalletRepository(context);

            var UserRepository = new UserRepository(context);

            var userMock = fixture.Build<User>()
                .Without(w=> w.Id)
                .Create();
         
            var UserCreated = await UserRepository.CreateUserDatabase(userMock);

            var walletMock = fixture.Build<Wallet>()
                .With(w=> w.Id, UserCreated)
                .With(w=> w.Balance, 0)
                .With(w => w.UserId, userMock.Id)
                .Create();

            await WalletRepository.IncreaseBalanceWallet(walletMock);

            var walletBalanceIncrease = await WalletRepository.IncreaseBalanceWallet(walletMock);

            var getUserWalletBalance = await WalletRepository.GetUserWallet(UserCreated);

            walletBalanceIncrease.Id.Should().Be(getUserWalletBalance.Id);

            walletBalanceIncrease.Balance.Should().Be(userMock.wallet!.Balance);

            getUserWalletBalance.UserId.Should().Be(userMock.Id);

        }
        [Fact]
        public async Task ShouldBeDecreaseBalanceAccount()
        {

            var context = AppDbContextFactory.CreateInMemoryDbContext();

            var WalletRepository = new WalletRepository(context);

            var UserRepository = new UserRepository(context);

            var userMock = fixture.Build<User>()
                .Without(w => w.Id)
                .Create();

            var UserCreated = await UserRepository.CreateUserDatabase(userMock);

            var walletMock = fixture.Build<Wallet>()
                .Without(w => w.Id)
                .With(w => w.Balance, 0)
                .With(w => w.UserId, userMock.Id)
                .Create();

            var walletBalanceDecrease = await WalletRepository.DecreaseBalanceWallet(walletMock);
           
            var getUserWalletBalance = await WalletRepository.GetUserWallet(userMock.Id);

            walletBalanceDecrease.Id.Should().Be(getUserWalletBalance.Id);

            getUserWalletBalance.Balance.Should().Be(userMock.wallet!.Balance);
        
            walletBalanceDecrease.UserId.Should().Be(userMock.Id);
        }
    }
}
