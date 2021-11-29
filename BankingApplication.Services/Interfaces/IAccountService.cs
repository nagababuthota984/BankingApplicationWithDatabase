using BankingApplication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Services
{
    public interface IAccountService
    {
        bool IsValidCustomer(string userName, string password);
        Account GetAccountByAccNumber(string accNumber);
        Account GetAccountById(string accountId);
        void DepositAmount(Account userAccount, decimal amount, Currency currency);
        void WithdrawAmount(Account userAccount, decimal amount);
        void TransferAmount(Account senderAccount, Bank senderBank, Account receiverAccount, decimal amount, ModeOfTransfer mode);
        void ApplyTransferCharges(Account senderAccount, Bank senderBank, string receiverBankId, decimal amount, ModeOfTransfer mode, string currencyName);

    }
}