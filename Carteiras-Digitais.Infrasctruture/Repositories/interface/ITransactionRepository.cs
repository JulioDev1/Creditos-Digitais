using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Infrasctruture.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateTransaction(Transaction transaction);
        Task<List<TransactionDto>> GetAllTransactionsUserSender(FilterTransactionDto transactionDto);
    }
}
