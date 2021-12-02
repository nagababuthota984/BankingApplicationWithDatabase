using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BankingApplication.Models;

namespace BankingApplication.Services
{
    public class BankService : IBankService
    {
        private IAccountService accountService = null;
        private BankAppDbContext dbContext = null;
        public BankService(IAccountService accService,BankAppDbContext context)
        {
            accountService = accService;
            dbContext = context;
        }
        public Bank CreateAndGetBank(string name, string branch, string ifsc)
        {
            Bank newBank = new Bank(name, branch, ifsc);
            dbContext.bank.Add(newBank);
            dbContext.SaveChanges();
            return newBank;
        }
        public Employee CreateAndGetEmployee(Customer newCustomer,EmployeeDesignation role, Bank bank)
        {
            Employee employee = new Employee(newCustomer,role, bank);
            dbContext.customer.Add(newCustomer);
            dbContext.employee.Add(employee);
            dbContext.SaveChanges();
            return employee;
        }
        public bool IsValidEmployee(string userName, string password)
        {
            Employee emp = dbContext.employee.ToList().FirstOrDefault(e=>e.UserName.EqualInvariant(userName) && e.Password.EqualInvariant(password));
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
            SessionContext.Bank = GetBankById(emp.BankId);
        }

        public void CreateAndAddAccount(Account newAccount, Bank bank)
        {
            newAccount.BankId = bank.BankId;
            dbContext.account.Add(newAccount);
            dbContext.customer.Add(newAccount.Customer);
            dbContext.SaveChanges();
        }
        
        public Bank GetBankById(string bankId)
        {
            return dbContext.bank.ToList().FirstOrDefault(b => b.BankId.EqualInvariant(bankId));
        }
        public List<Transaction> GetTransactionsByDate(DateTime date, Bank bank)
        {
            List<Transaction> transactions = new List<Transaction>();
            foreach (Account account in bank.Accounts)
            {
                transactions.AddRange(account.Transactions.FindAll(tr => tr.On.Date == date));
            }
            transactions.AddRange(bank.Transactions.FindAll(tr => tr.On.Date == date));
            return transactions;
        }
        public List<Transaction> GetAccountTransactions(string accountId)
        {
            return dbContext.transaction.ToList().FindAll(tr=>tr.SenderAccountId.EqualInvariant(accountId) || tr.ReceiverAccountId.EqualInvariant(accountId));
        }
        public List<Transaction> GetTransactions(Bank bank)
        {
            List<Transaction> transactions = new List<Transaction>();
            foreach (Account account in bank.Accounts)
            {
                transactions.AddRange(account.Transactions);
            }
            transactions.AddRange(bank.Transactions);
            return transactions;
        }
        public bool AddNewCurrency(Bank bank, string newCurrencyName, decimal exchangeRate)
        {
            if (dbContext.currency.ToList().Any(c => c.BankId.EqualInvariant(bank.BankId)&& c.Name.EqualInvariant(newCurrencyName)))
            {
                return false;
            }
            dbContext.currency.Add(new Currency(newCurrencyName, exchangeRate,bank.BankId));
            dbContext.SaveChanges();
            return true;
        }
        public void UpdateAccount(Customer customer)
        {
            dbContext.customer.Update(customer);
            dbContext.SaveChanges();
        }

        public bool DeleteAccount(Account userAccount)
        {
            userAccount.Status = AccountStatus.Closed;
            dbContext.account.Update(userAccount);
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
                    bank.OtherRTGS = newValue; 
                }
                else
                {
                    bank.OtherIMPS = newValue;
                }
                isModified = true;
            }
            dbContext.bank.Update(bank);
            dbContext.SaveChanges();
            return isModified;
        }
       
        public bool RevertTransaction(Transaction transaction, Bank bank)
        {
            Account userAccount = accountService.GetAccountById(transaction.SenderAccountId);
            if (transaction.Type == TransactionType.Credit)
            {
                accountService.WithdrawAmount(userAccount, transaction.TransactionAmount);
            }
            else if (transaction.Type == TransactionType.Debit)
            {
                accountService.DepositAmount(userAccount, transaction.TransactionAmount, dbContext.currency.ToList().FirstOrDefault(c => c.Name.EqualInvariant(bank.DefaultCurrencyName)));
            }
            else if (transaction.Type == TransactionType.Transfer)
            {
                Account receiverAccount = accountService.GetAccountById(transaction.ReceiverAccountId);
                accountService.WithdrawAmount(receiverAccount, transaction.TransactionAmount);
                accountService.DepositAmount(userAccount, transaction.TransactionAmount, dbContext.currency.ToList().FirstOrDefault(c => c.Name.EqualInvariant(bank.DefaultCurrencyName)));
            }
            dbContext.SaveChanges();
            return true;

        }
       
        
        
    }
}






