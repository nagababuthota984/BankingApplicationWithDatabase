using BankAppDbFirstApproach.Models;
using System;
using System.Linq;

namespace BankAppDbFirstApproach.Services
{
    public class TransactionService : ITransactionService
    {
        private BankStorageEntities dbContext;
        public TransactionService(BankStorageEntities context)
        {
            dbContext = context;    
        }

        public void CreateTransaction(Account userAccount, TransactionType transtype, decimal transactionamount, string currencyName)
        {
            Transaction newTransaction = new Transaction(userAccount, transtype, transactionamount, currencyName,false);
            dbContext.Transactions.Add(newTransaction);
            dbContext.SaveChanges();
        }
        public void CreateTransferTransaction(Account userAccount, Account receiverAccount, decimal transactionAmount, ModeOfTransfer mode, string currencyName)
        {
            Transaction senderTransaction = new Transaction(userAccount, receiverAccount, TransactionType.Transfer, transactionAmount, currencyName, mode);
            dbContext.Transactions.Add(senderTransaction);
            Transaction receiverTransaction = new Transaction(userAccount, receiverAccount, TransactionType.Transfer, transactionAmount, currencyName, mode);
            receiverTransaction.balance = receiverAccount.balance;
            dbContext.Transactions.Add(receiverTransaction);
            dbContext.SaveChanges();

        }
        public void CreateAndAddBankTransaction(Bank bank, Account userAccount, decimal charges, string currencyName)
        {
            Transaction userTransaction = new Transaction(userAccount, TransactionType.Debit, charges, currencyName,true);
            Transaction newBankTransaction = new Transaction(userAccount, bank, TransactionType.ServiceCharge, charges, currencyName);
            dbContext.Transactions.Add(newBankTransaction);
            dbContext.Transactions.Add(userTransaction);
            dbContext.SaveChanges();
        }
        public Transaction GetTransactionById(string transactionId)
        {
            return dbContext.Transactions.FirstOrDefault(tr=>tr.transId.Equals(transactionId));
        }


    }
}