using BankAppDbFirstApproach.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppDbFirstApproach.Services
{
    public interface IBankService
    {
        Bank CreateAndGetBank(string name, string branch, string ifsc);
        bool IsValidEmployee(string userName, string password);
        void CreateAndAddAccount(Account newAccount, Customer customer,Bank bank);
        bool DeleteAccount(Account userAccount);
        bool AddNewCurrency(Bank bank, string newName, decimal exchangeRate);
        bool ModifyServiceCharge(ModeOfTransfer mode, bool isSelfBankCharge, Bank bank, decimal newValue);
        List<Transaction> GetAccountTransactions(string accountId);
        bool RevertTransaction(Transaction transaction, Bank bank);
        Employee CreateAndGetEmployee(Customer customer, EmployeeDesignation role, Bank bank);
        List<Transaction> GetTransactionsByDate(DateTime date, Bank bank);
        List<Transaction> GetTransactions(Bank bank);
        Bank GetBankById(string bankid);
    }
}