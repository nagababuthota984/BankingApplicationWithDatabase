using BankAppDbFirstApproach.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppDbFirstApproach.Services
{
    public interface ITransactionService
    {
        void CreateTransaction(Account userAccount, TransactionType transtype, decimal transactionamount, string currencyName);
        void CreateTransferTransaction(Account userAccount, Account receiverAccount, decimal transactionAmount, ModeOfTransfer mode, string currencyName);
        void CreateAndAddBankTransaction(Bank bank, Account userAccount, decimal charges, string currencyName);
        Transaction GetTransactionById(string transactionId);


    }
}