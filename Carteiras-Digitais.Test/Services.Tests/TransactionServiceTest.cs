using AutoFixture;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories;
using Carteiras_Digitais.Shared.Dtos;
using FluentAssertions;
using Moq;

namespace Carteiras_Digitais.Test.Services.Tests
{
    public class TransactionServiceTest
    {
        private readonly Mock<ITransactionRepository> transactionRepository;
        private readonly Mock<IWalletService> walletService;
        private readonly Fixture fixture;

        public TransactionServiceTest()
        {
            this.transactionRepository = new Mock<ITransactionRepository>();
            this.walletService = new Mock<IWalletService>();  
            fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task ShouldBeReturnNotFoundReceiverUserError()
        {
            var wallet = fixture.Create<Wallet>();

            var balance = fixture.Build<TransactionDto>()
                .With(x => x.ReceiverWalletId, wallet.UserId)
                .With(x => x.Amount, wallet.Balance)
                .Create();

            var userWalletNotFound = walletService.Setup((w=> w.GetUserBalanceWallet(wallet.Id))).ReturnsAsync((Wallet ? ) null);

            var service = new TransactionService(transactionRepository.Object, walletService.Object);

            Func<Task> action = async () => await service.TransactionToBalanceToReceiver(balance);

            await action.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
