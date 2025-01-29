using Carteiras_Digitais.Core.Domain.Interfaces;
using Carteiras_Digitais.Core.Domain.Models;
using Carteiras_Digitais.Infrasctruture.Repositories;
using Carteiras_Digitais.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IWalletService walletService;
        private readonly ITransactionRepository transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository, IWalletService walletService)
        {
            this.transactionRepository = transactionRepository;
            this.walletService = walletService;
        }

        
        public async Task<List<TransactionDto>> GetUserAllUserTransactionsWithFilter(FilterTransactionDto filterTransaction)
        {

            var wallet = await walletService.GetUserBalanceWallet(filterTransaction.userId);
            
            if (wallet is null) {
                throw new KeyNotFoundException("wallet not found");
            }
            
            var transactionsUser =  await transactionRepository.GetAllTransactionsUserSender(filterTransaction);

            if (filterTransaction.InitialDate.HasValue)
            {
                transactionsUser = transactionsUser.Where(x => x.CreatedAt >= filterTransaction.InitialDate).ToList();
                  
            }
            if (filterTransaction.EndDate.HasValue)
            {
                transactionsUser = transactionsUser.Where(x => x.CreatedAt <= filterTransaction.EndDate).ToList();
            }

            return transactionsUser.ToList();
        }

        public async Task<TransactionDto> TransactionToBalanceToReceiver(TransactionDto transactionDto)
        {
            var findWalletUserReceiver = await walletService.GetUserBalanceWallet(transactionDto.ReceiverWalletId);

            if (findWalletUserReceiver == null) throw new KeyNotFoundException("user not exists");

            var verifyUserBalance = await walletService.GetUserBalanceWallet(transactionDto.SenderWalletId);

            if (verifyUserBalance!.Balance < transactionDto.Amount) throw new InvalidOperationException("value insuficient");

            var DecreaseUserSender = new BalanceDto(transactionDto.Amount, verifyUserBalance.UserId); 

            var DiscountUserSender = await walletService.DiscountBalanceToWallet(DecreaseUserSender);

            var IncreaseUserReceiver = new BalanceDto(transactionDto.Amount , findWalletUserReceiver.UserId);

            var DepositBalanceUserReceiver = await walletService.DepositBalanceToWallet(IncreaseUserReceiver);

            var createTransaction = new Transaction
            {
                Amount = transactionDto.Amount,
                Description = transactionDto.Description,
                ReceiverWalletId = DepositBalanceUserReceiver.UserId,
                SenderWalletId = DepositBalanceUserReceiver.UserId,
            };

            await transactionRepository.CreateTransaction(createTransaction);

            return transactionDto;
        }
    }
}
