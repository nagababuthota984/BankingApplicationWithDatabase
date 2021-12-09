using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BankAppDbFirstApproach.Models;

namespace BankAppDbFirstApproach.Services
{
    public class BankService : IBankService
    {
        private IAccountService accountService;
        private BankStorageEntities dbContext;
        public BankService(IAccountService accService, BankStorageEntities context)
        {
            accountService = accService;
            dbContext = context;
        }
        public Bank CreateAndGetBank(string name, string branch, string ifsc)
        {
            Bank newBank = new Bank(name, branch, ifsc);
            Currency currency = new Currency( "INR", 1, newBank.bankId);
            Customer customer = new Customer("Admin", newBank.bankId);
            Employee employee = new Employee("Admin", "admin", newBank.bankId,customer.customerId);
            dbContext.Banks.Add(newBank);
            dbContext.Customers.Add(customer);
            dbContext.Currencies.Add(currency);
            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();
            return newBank;
        }
        public Employee CreateAndGetEmployee(Customer newCustomer,EmployeeDesignation role, Bank bank)
        {
            Employee employee = new Employee(newCustomer,role, bank);
            dbContext.Customers.Add(newCustomer);
            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();
            return employee;
        }
        public bool IsValidEmployee(string userName, string password)
        {
            Employee emp = dbContext.Employees.ToList().FirstOrDefault(e => e.username.EqualInvariant(userName) && e.password.EqualInvariant(password)); ;
            if (emp == null)
                return false;
            else
            {
                PrepareEmployeeSessionContext(emp);
                return true;
            }
        }

        private void PrepareEmployeeSessionContext(Employee emp)
        {
            SessionContext.Employee = emp;
            SessionContext.Bank = GetBankById(emp.bankId);
        }

        public void CreateAndAddAccount(Account newAccount,Customer newCustomer, Bank bank)
        {
            newAccount.bankId = bank.bankId;
            newAccount.customerId = newCustomer.customerId;
            dbContext.Accounts.Add(newAccount);
            dbContext.Customers.Add(newCustomer);
            dbContext.SaveChanges();
        }
        
        public Bank GetBankById(string bankId)
        {
            return dbContext.Banks.ToList().FirstOrDefault(b => b.bankId.EqualInvariant(bankId));
        }
        public List<Transaction> GetTransactionsByDate(DateTime date, Bank bank)
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions.AddRange(bank.Transactions.Where(tr => tr.transactionOn.Date == date && tr.bankId.Equals(bank.bankId)));
            return transactions;
        }
        public List<Transaction> GetAccountTransactions(string accountId)
        {
            return dbContext.Transactions.Where(tr=>tr.accountId.Equals(accountId)).ToList();
        }
        public List<Transaction> GetTransactions(Bank bank)
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions.AddRange(bank.Transactions);
            return transactions;
        }
        public bool AddNewCurrency(Bank bank, string newCurrencyName, decimal exchangeRate)
        {
            if (dbContext.Currencies.Any(c => c.bankId.Equals(bank.bankId)&& c.name.Equals(newCurrencyName)))
            {
                return false;
            }
            dbContext.Currencies.Add(new Currency(newCurrencyName, exchangeRate, bank.bankId));
            dbContext.SaveChanges();
            return true;
        }
       

        public bool DeleteAccount(Account userAccount)
        {
            userAccount.status = (int)AccountStatus.Closed;
            dbContext.SaveChanges();
            return true;
        }
        public bool ModifyServiceCharge(ModeOfTransfer mode, bool isSelfBankCharge, Bank bank, decimal newValue)
        {
            bool isModified;
            if (isSelfBankCharge)
            {
                if (mode == ModeOfTransfer.RTGS)
                {
                    bank.selfRTGS = newValue;
                }
                else
                {
                    bank.selfIMPS = newValue;
                }
                isModified = true;
            }
            else
            {
                if (mode == ModeOfTransfer.RTGS)
                {
                    bank.otherRTGS = newValue; 
                }
                else
                {
                    bank.otherIMPS = newValue;
                }
                isModified = true;
            }
            dbContext.SaveChanges();
            return isModified;
        }
       
        public bool RevertTransaction(Transaction transaction, Bank bank)
        {
            Account userAccount = accountService.GetAccountById(transaction.accountId);
            if (transaction.transactionType == (int)TransactionType.Credit)
            {
                accountService.WithdrawAmount(userAccount, transaction.transactionAmount);
            }
            else if (transaction.transactionType == (int)TransactionType.Debit)
            {
                accountService.DepositAmount(userAccount, transaction.transactionAmount, dbContext.Currencies.FirstOrDefault(c => c.name.Equals(bank.defaultCurrencyName)));
            }
            else if (transaction.transactionType == (int)TransactionType.Transfer)
            {
                Account receiverAccount = accountService.GetAccountById(transaction.accountId);
                accountService.WithdrawAmount(receiverAccount, transaction.transactionAmount);
                accountService.DepositAmount(userAccount, transaction.transactionAmount, dbContext.Currencies.FirstOrDefault(c => c.name.Equals(bank.defaultCurrencyName)));
            }
            dbContext.SaveChanges();
            return true;

        }
       
        
        
    }
}






