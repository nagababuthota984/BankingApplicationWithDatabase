using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BankingApplication.Models;

namespace BankingApplication.Services
{
    public class BankService : IBankService
    {
        IAccountService accountService = new AccountService();
        IDataProvider dataProvider = new JsonFileHelper();
        public Bank CreateAndGetBank(string name, string branch, string ifsc)
        {
            Bank newBank = new Bank(name, branch, ifsc);
            RBIStorage.banks.Add(newBank);
            return newBank;
        }

        public bool IsValidEmployee(string userName, string password)
        {
            foreach (var bank in RBIStorage.banks)
            {
                Employee employee = bank.Employees.FirstOrDefault(e => (e.UserName.EqualInvariant(userName)) && (e.Password.Equals(password)));
                if (employee != null)
                {
                    SessionContext.Bank = bank;
                    SessionContext.Employee = employee;
                    return true;
                }
            }
            return false;


        }
        public void CreateAccount(Account newAccount, Bank bank)
        {
            newAccount.BankId = bank.BankId;
            newAccount.BankName = bank.BankName;
            newAccount.Branch = bank.Branch;
            newAccount.Ifsc = bank.Ifsc;
            newAccount.AccountNumber = GenerateAccountNumber(bank.BankId);
            bank.Accounts.Add(newAccount);
            dataProvider.WriteData(RBIStorage.banks);
        }
        public string GenerateAccountNumber(string bankid)
        {
            string accNumber = null;
            do
            {
                accNumber = Utilities.GenerateRandomNumber(12).ToString();
            } while (Utilities.IsDuplicateAccountNumber(accNumber, bankid));
            return accNumber;
        }
        public Bank GetBankByBankId(string bankId)
        {
            Bank bank = RBIStorage.banks.FirstOrDefault(b => b.BankId.EqualInvariant(bankId));
            if (bank != null)
            {
                return bank;
            }
            else
            {
                throw new InvalidBankException("Bank Doesnt Exist.");
            }
        }
        public bool AddNewCurrency(Bank bank, string newCurrencyName, decimal exchangeRate)
        {
            if (bank.SupportedCurrency.Any(c => c.CurrencyName.EqualInvariant(newCurrencyName)))
            {
                return false;
            }
            bank.SupportedCurrency.Add(new Currency(newCurrencyName, exchangeRate));
            dataProvider.WriteData(RBIStorage.banks);
            return true;
        }

        public bool SetServiceCharge(ModeOfTransfer mode, bool isSelfBankCharge, Bank bank, decimal newValue)
        {
            bool isModified;
            if (isSelfBankCharge)
            {
                if (mode == ModeOfTransfer.RTGS)
                {
                    bank.SelfRTGS = newValue;
                }
                else
                {
                    bank.SelfIMPS = newValue;
                }
                isModified = true;
            }
            else
            {
                if (mode == ModeOfTransfer.RTGS)
                {
                    bank.OtherRTGS = newValue; isModified = true;
                }
                else
                {
                    bank.OtherIMPS = newValue; isModified = true;
                }
            }
            dataProvider.WriteData(RBIStorage.banks);
            return isModified;
        }
        public List<Transaction> GetAccountTransactions(string accountId)
        {
            return accountService.GetAccountById(accountId)?.Transactions;
        }
        public bool RevertTransaction(Transaction transaction, Bank bank)
        {
            Account userAccount = accountService.GetAccountById(transaction.SenderAccountId);
            if (transaction.Type == TransactionType.Credit)
            {
                accountService.WithdrawAmount(userAccount, transaction.TransactionAmount);
                userAccount.Transactions.Remove(transaction);
            }
            else if (transaction.Type == TransactionType.Debit)
            {
                accountService.DepositAmount(userAccount, transaction.TransactionAmount, bank.DefaultCurrency);
                userAccount.Transactions.Remove(transaction);
            }
            else if (transaction.Type == TransactionType.Transfer)
            {
                Account receiverAccount = accountService.GetAccountById(transaction.ReceiverAccountId);
                accountService.WithdrawAmount(receiverAccount, transaction.TransactionAmount);
                receiverAccount.Transactions.Remove(transaction);
                accountService.DepositAmount(userAccount, transaction.TransactionAmount, bank.DefaultCurrency);
                userAccount.Transactions.Remove(transaction);


            }
            dataProvider.WriteData(RBIStorage.banks);
            return true;

        }
        public Employee CreateAndGetEmployee(string name, string age, DateTime dob, Gender gender, EmployeeDesignation role, Bank bank)
        {
            Employee employee = new Employee(name, age, dob, gender, role, bank);
            bank.Employees.Add(employee);
            return employee;
        }
        public Bank FetchBankById(string bankid)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from bank where id=@bankid", conn);
                SqlParameter bankidParameter = new SqlParameter("@bankid", bankid);
                cmd.Parameters.Add(bankidParameter);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    Bank bank = new Bank
                    {
                        BankId = reader["id"].ToString(),
                        BankName = reader["name"].ToString(),
                        Ifsc = reader["ifsc"].ToString()
                    };
                    return bank;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}






