using Carteiras_Digitais.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Carteiras_Digitais.Core.Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> TransactionToBalanceToReceiver(TransactionDto transactionDto);
        Task<List<TransactionDto>> GetUserAllUserTransactionsWithFilter(FilterTransactionDto filterTransactionDto);
      
    }
}
