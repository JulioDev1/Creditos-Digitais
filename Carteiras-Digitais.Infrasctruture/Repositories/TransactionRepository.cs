using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Database;
using Carteiras_Digitais.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Infrasctruture.Repositories
{

    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext context;

        public TransactionRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            context.transactions.Add(transaction);
            
            await context.SaveChangesAsync();
            return transaction;
        }

        public async  Task<List<TransactionDto>> GetAllTransactionsUserSender(FilterTransactionDto transactionDto)
        {

           var query =  context.transactions.AsQueryable();

            query = query.Where(t => t.SenderWalletId == transactionDto.SenderWalletId);

            if (transactionDto.InitialDate.HasValue)
            {
                query = query.Where(t=> t.CreatedAt >= transactionDto.InitialDate);
            }
            if (transactionDto.EndDate.HasValue)
            {
                query = query.Where(t => t.CreatedAt <= transactionDto.EndDate);

            }

            return await query.Select(x=> new TransactionDto
            {
                Amount = x.Amount,
                Description = x.Description,
                ReceiverWalletId = x.ReceiverWalletId,
                SenderWalletId = x.SenderWalletId
            }).ToListAsync();
        }
    }
}
