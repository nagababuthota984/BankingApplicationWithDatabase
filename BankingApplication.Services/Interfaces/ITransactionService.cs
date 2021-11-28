using BankingApplication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Services
{
    public interface ITransactionService
    {
        void CreateTransaction(Account userAccount, TransactionType transtype, decimal transactionamount, Currency currency);
        void CreateTransferTransaction(Account userAccount, Account receiverAccount, decimal transactionAmount, ModeOfTransfer mode, Currency currency);
        void CreateAndAddBankTransaction(Bank bank, Account userAccount, decimal charges, Currency currency);
        Transaction GetTransactionById(string transactionId);


    }
}