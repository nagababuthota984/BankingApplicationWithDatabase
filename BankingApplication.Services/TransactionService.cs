using BankingApplication.Models;
using System;
using System.Linq;

namespace BankingApplication.Services
{
    public class TransactionService : ITransactionService
    {

        public void CreateTransaction(Account userAccount, TransactionType transtype, decimal transactionamount, Currency currency)
        {
            Transaction newTransaction = new Transaction(userAccount,userAccount, transtype,transactionamount, currency);
            userAccount.Transactions.Add(newTransaction);
        }
        public void CreateTransferTransaction(Account userAccount,Account receiverAccount,decimal transactionAmount,ModeOfTransfer mode, Currency currency)
        {
            Transaction senderTransaction = new Transaction(userAccount,receiverAccount, TransactionType.Transfer, transactionAmount, currency,mode);
            userAccount.Transactions.Add(senderTransaction);
            Transaction receiverTransaction = new Transaction(userAccount, receiverAccount, TransactionType.Transfer, transactionAmount, currency, mode);
            receiverTransaction.BalanceAmount = receiverAccount.Balance;
            receiverAccount.Transactions.Add(receiverTransaction);
            
        }
        public void CreateBankTransaction(Bank bank, Account userAccount, decimal charges,Currency currency)
        {
            Transaction newBankTransaction = new Transaction(userAccount, bank, TransactionType.ServiceCharge, charges, currency);
            bank.Transactions.Add(newBankTransaction);
        }
        public Transaction GetTransactionById(string transactionId)
        {
            Transaction transaction = null;
            if (transactionId.Length >=38)
            {
                string bankId = transactionId.Substring(3, 11);
                string accountId = transactionId.Substring(14, 11);
                Bank bank = RBIStorage.banks.FirstOrDefault(b => b.BankId.Equals(bankId,StringComparison.OrdinalIgnoreCase));
                if (bank != null)
                {
                    Account account = bank.Accounts.FirstOrDefault(b => b.AccountId.Equals(accountId,StringComparison.OrdinalIgnoreCase));
                    if (account != null)
                    {
                        transaction = account.Transactions.FirstOrDefault(t => t.TransId.Equals(transactionId,StringComparison.OrdinalIgnoreCase));
                        return transaction;
                    }
                }
            }
            return transaction;
        }

        
    }
}
