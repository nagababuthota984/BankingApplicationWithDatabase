using BankingApplication.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Services
{
    public interface IBankService
    {
        Bank CreateAndGetBank(string name, string branch, string ifsc);
        bool IsValidEmployee(string userName, string password);
        void CreateAccount(Account newAccount, Bank bank);
        string GenerateAccountNumber(string bankid);
        Bank GetBankByBankId(string bankId);
        bool AddNewCurrency(Bank bank, string newCurrencyName, decimal exchangeRate);
        bool SetServiceCharge(ModeOfTransfer mode, bool isSelfBankCharge, Bank bank, decimal newValue);
        List<Transaction> GetAccountTransactions(string accountId);
        bool RevertTransaction(Transaction transaction, Bank bank);
        Employee CreateAndGetEmployee(string name, string age, DateTime dob, Gender gender, EmployeeDesignation role, Bank bank);

    }
}
