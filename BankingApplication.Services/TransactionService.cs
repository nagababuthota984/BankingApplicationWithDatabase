using BankingApplication.Models;
using System;
using System.Linq;

namespace BankingApplication.Services
{
    public class TransactionService : ITransactionService
    {
        private BankAppDbContext dbContext;
        public TransactionService(BankAppDbContext context)
        {
            dbContext = context;    
        }

        public void CreateTransaction(Account userAccount, TransactionType transtype, decimal transactionamount, string currencyName)
        {
            Transaction newTransaction = new Transaction(userAccount, userAccount, transtype, transactionamount, currencyName);
            dbContext.transaction.Add(newTransaction);
            dbContext.SaveChanges();
        }
        public void CreateTransferTransaction(Account userAccount, Account receiverAccount, decimal transactionAmount, ModeOfTransfer mode, string currencyName)
        {
            Transaction senderTransaction = new Transaction(userAccount, receiverAccount, TransactionType.Transfer, transactionAmount, currencyName, mode);
            dbContext.transaction.Add(senderTransaction);
            Transaction receiverTransaction = new Transaction(userAccount, receiverAccount, TransactionType.Transfer, transactionAmount, currencyName, mode);
            receiverTransaction.BalanceAmount = receiverAccount.Balance;
            dbContext.transaction.Add(receiverTransaction);
            dbContext.SaveChanges();

        }
        public void CreateAndAddBankTransaction(Bank bank, Account userAccount, decimal charges, string currencyName)
        {
            Transaction newBankTransaction = new Transaction(userAccount, bank, TransactionType.ServiceCharge, charges, currencyName);
            dbContext.transaction.Add(newBankTransaction);
            dbContext.SaveChanges();
        }
        public Transaction GetTransactionById(string transactionId)
        {
            return dbContext.transaction.ToList().FirstOrDefault(tr=>tr.TransId.EqualInvariant(transactionId));
        }


    }
}