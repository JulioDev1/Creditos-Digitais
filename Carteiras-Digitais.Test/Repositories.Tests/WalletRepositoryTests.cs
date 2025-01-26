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

            var userMock = fixture.Create<User>();
         
            var walletMock = fixture.Build<Wallet>()
                .With(w=> w.Id, userMock.wallet!.Id)
                .With(w => w.UserId, userMock.Id)
                .With(w=> w.Balance, 300).Create();

            var UserCreated = await UserRepository.CreateUserDatabase(userMock);

        
            var walletBalanceIncrease = await WalletRepository.IncreaseBalanceWallet(walletMock);

            var getUserWalletBalance = await WalletRepository.GetUserWallet(userMock.Id);

            walletBalanceIncrease.Id.Should().Be(walletMock.Id);
            
            getUserWalletBalance.Balance.Should().Be(300);

            getUserWalletBalance.UserId.Should().Be(userMock.Id);

        }
        [Fact]
        public async Task ShouldBeDecreaseBalanceAccount()
        {

            var context = AppDbContextFactory.CreateInMemoryDbContext();

            var WalletRepository = new WalletRepository(context);

            var UserRepository = new UserRepository(context);

            var userMock = fixture.Create<User>();

            var walletMock = fixture.Build<Wallet>()
                .With(w => w.Id, userMock.wallet!.Id)
                .With(w => w.UserId, userMock.Id)
                .With(w => w.Balance, 300).Create();

            var walletMockDecrease = fixture.Build<Wallet>()
                .With(w => w.Id, userMock.wallet!.Id)
                .With(w => w.UserId, userMock.Id)
                .With(w => w.Balance, 0).Create();

            var UserCreated = await UserRepository.CreateUserDatabase(userMock);

            var walletBalanceIncrease = await WalletRepository.IncreaseBalanceWallet(walletMock);

            var walletBalanceDecrease = await WalletRepository.DecreaseBalanceWallet(walletMock);
           
            var getUserWalletBalance = await WalletRepository.GetUserWallet(userMock.Id);

            walletBalanceDecrease.Id.Should().Be(walletMock.Id);

            getUserWalletBalance.Balance.Should().Be(300);

            walletBalanceDecrease.UserId.Should().Be(userMock.Id);

        }
    }
}
