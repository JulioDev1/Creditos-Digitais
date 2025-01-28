using AutoFixture;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories;
using Carteiras_Digitais.Test.Repositories.Tests.Database;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Test.Repositories.Tests
{
   
    public class TransactionsRepositoryTest
    {
        private readonly Fixture fixture;

        public TransactionsRepositoryTest()
        {
            fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task ShouldBeCreateTransactionsAndAttributteToUser()
        {
            var context = AppDbContextFactory.CreateInMemoryDbContext();

            var TransactionRepository = new TransactionRepository(context);

            var transaction = fixture.Create<Transaction>();

            var transactionRepo = await TransactionRepository.CreateTransaction(transaction);

            transactionRepo.Should().Be(transaction);
        }

        [Fact]
        public async Task ShouldBeTransactionArray()
        {
            var context = AppDbContextFactory.CreateInMemoryDbContext();

            var transactionRepository = new TransactionRepository(context);

            var guid = Guid.NewGuid();
                
            var transactionRepo = await transactionRepository.GetAllTransactionsUserSender(guid);

            transactionRepo.Should().BeOfType<List<Transaction>>();

        }
    }
}
