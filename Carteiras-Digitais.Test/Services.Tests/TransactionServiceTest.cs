using AutoFixture;
using Carteiras_Digitais.Application.Services;
using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Database;
using Carteiras_Digitais.Infrasctruture.Repositories;
using Carteiras_Digitais.Shared.Dtos;
using Carteiras_Digitais.Test.Repositories.Tests.Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

            walletService.Setup((w => w.GetUserBalanceWallet(wallet.Id))).ReturnsAsync((Wallet?)null);

            var service = new TransactionService(transactionRepository.Object, walletService.Object);

            Func<Task> action = async () => await service.TransactionToBalanceToReceiver(balance);

            await action.Should().ThrowAsync<KeyNotFoundException>();
        }
        [Fact]
        public async Task ShouldBeReturnFilterUser()
        {
            var context = AppDbContextFactory.CreateInMemoryDbContext();
            var transactionDto = fixture.Create<FilterTransactionDto>();

            var wallet = new Wallet
            { 
                Id = Guid.NewGuid(),
                Balance = 0,
                UserId = transactionDto.userId,
            };

            var transactions = new List<TransactionDto>
            {
                new TransactionDto { SenderWalletId = wallet.Id, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new TransactionDto{ SenderWalletId = wallet.Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                new TransactionDto { SenderWalletId = wallet.Id, CreatedAt = DateTime.UtcNow }
            };


            walletService.Setup((w=> w.GetUserBalanceWallet(transactionDto.userId))).ReturnsAsync((wallet));
            
            transactionRepository.Setup((w=> w.GetAllTransactionsUserSender(transactionDto)))
                .ReturnsAsync(transactions.ToList());

            var service = new TransactionService(transactionRepository.Object, walletService.Object);

            var action = await service.GetUserAllUserTransactionsWithFilter(transactionDto);

            action.Should().BeOfType<List<TransactionDto>>();
        }

        [Fact]
        public void ShouldBeReturnWalletNotFoundError()
        {
            var wallet = fixture.Create<FilterTransactionDto>();

            walletService.Setup((w=> w.GetUserBalanceWallet(wallet.userId))).ReturnsAsync((Wallet?) null);

            var service = new TransactionService(transactionRepository.Object, walletService.Object);

            Func<Task> action = async () => await service.GetUserAllUserTransactionsWithFilter(wallet);

            action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task ShouldBeBalanceMoreExpensive()
        {
            var receiverWallet =
                new Wallet
                {
                    Id = Guid.NewGuid(),
                    Balance = 0,
                    UserId = Guid.NewGuid(),

                };
            var senderWallet =
                new Wallet
                {
                    Id = Guid.NewGuid(),
                    Balance = 0,
                    UserId = Guid.NewGuid(),
                };
            var balance =
                new TransactionDto
                {
                    Amount = 300,
                    ReceiverWalletId = receiverWallet.Id,
                    SenderWalletId = senderWallet.Id,
                };

            walletService.Setup((w => w.GetUserBalanceWallet(receiverWallet.Id))).ReturnsAsync(receiverWallet);

            walletService.Setup((w => w.GetUserBalanceWallet(senderWallet.Id))).ReturnsAsync(senderWallet);

            var service = new TransactionService(transactionRepository.Object, walletService.Object);

            Func<Task> action = async () => await service.TransactionToBalanceToReceiver(balance);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }
        [Fact]
        public async Task ShouldBeSuccessAndSendMoneyToReceiver()
        {
            var receiverWallet = new Wallet
            {
                Id = Guid.NewGuid(),
                Balance = 0,
                UserId = Guid.NewGuid(),

            };

            var senderWallet = new Wallet
            {
                Id = Guid.NewGuid(),
                Balance = 600,
                UserId = Guid.NewGuid(),
            };

            var receiverWalletResult = new Wallet
            {
                Id = receiverWallet.Id,
                Balance = 300,
                UserId = receiverWallet.UserId,

            };

            var senderWalletResult = new Wallet
            {
                Id = senderWallet.Id,
                Balance = 300,
                UserId = senderWallet.UserId,
            };

            var balance = new TransactionDto
            {
                Amount = 300,
                ReceiverWalletId = receiverWallet.Id,
                SenderWalletId = senderWallet.Id,
            };

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = balance.Amount,
                ReceiverWalletId = balance.ReceiverWalletId,
                SenderWalletId = balance.SenderWalletId,
                CreatedAt = DateTime.UtcNow,
            };

            var decreaseUser = new BalanceDto(senderWallet.Balance, senderWallet.Id);

            var increaseUser = new BalanceDto(receiverWallet.Balance, receiverWallet.Id);
            
            walletService.Setup((w => w.GetUserBalanceWallet(receiverWallet.Id))).ReturnsAsync(receiverWallet);

            walletService.Setup((w => w.GetUserBalanceWallet(senderWallet.Id))).ReturnsAsync(senderWallet);

            walletService.Setup((w=> w.DiscountBalanceToWallet(It.IsAny<BalanceDto>()))).ReturnsAsync(senderWalletResult);

            walletService.Setup((w => w.DepositBalanceToWallet(It.IsAny<BalanceDto>()))).ReturnsAsync(receiverWalletResult);

            transactionRepository.Setup((t=> t.CreateTransaction(transaction))).ReturnsAsync(transaction);

            var service = new TransactionService(transactionRepository.Object, walletService.Object);

            var action = await service.TransactionToBalanceToReceiver(balance);

            action.Should().Be(balance);
        }
    }
}
