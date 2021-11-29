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
        public BankService(IAccountService accService)
        {
            accountService = Factory.CreateAccountService();
        }
        public Bank CreateAndGetBank(string name, string branch, string ifsc)
        {
            Bank newBank = new Bank(name, branch, ifsc);
            using (SqlConnection conn = new SqlConnection(SqlHelper.connectionString))
            {
                conn.Open();
                SqlCommand createBankCommand = new SqlCommand("insert into bank values(@bankid,@name,@branch,@ifsc,@defualtcurrency)",conn);
                createBankCommand.Parameters.Add("@bankid",SqlDbType.VarChar).Value = newBank.BankId;
                createBankCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = newBank.BankName;
                createBankCommand.Parameters.Add("@branch", SqlDbType.VarChar).Value = newBank.Branch;
                createBankCommand.Parameters.Add("@ifsc", SqlDbType.VarChar).Value = newBank.Ifsc;
                createBankCommand.Parameters.Add("@defualtcurrency", SqlDbType.VarChar).Value = newBank.DefaultCurrencyName;
                if(createBankCommand.ExecuteNonQuery()!=-1)
                {
                    return newBank;
                }
                else
                {
                    return null;
                }
            }
        }
        public Employee CreateAndGetEmployee(string name, int age, DateTime dob, Gender gender, EmployeeDesignation role, Bank bank)
        {
            Employee employee = new Employee(name, age, dob, gender, role, bank);
            bank.Employees.Add(employee);
            JsonFileHelper.WriteData(RBIStorage.banks);
            return employee;
        }
        public bool IsValidEmployee(string userName, string password)
        {
            BankAppDbContext dbContext = new BankAppDbContext();
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
            SessionContext.Bank = GetBankByBankId(emp.BankId);
        }

        public void CreateAndAddAccount(Account newAccount, Bank bank)
        {
            newAccount.BankId = bank.BankId;
            newAccount.AccountNumber = GenerateAccountNumber(bank.BankId);
            using (SqlConnection conn = new SqlConnection(SqlHelper.connectionString))
            {
                conn.Open();
                SqlCommand createAccountCommand = new SqlCommand($"insert into account values(@accountId,@bankId,@accountNumber,@username,@password,@accountType,@balance,@status)", conn);
                createAccountCommand.Parameters.Add("@accountId", SqlDbType.VarChar).Value = newAccount.AccountId;
                createAccountCommand.Parameters.Add("@bankId", SqlDbType.VarChar).Value = newAccount.BankId;
                createAccountCommand.Parameters.Add("@accountNumber", SqlDbType.VarChar).Value = newAccount.AccountNumber;
                createAccountCommand.Parameters.Add("@username", SqlDbType.VarChar).Value = newAccount.UserName;
                createAccountCommand.Parameters.Add("@password", SqlDbType.VarChar).Value = newAccount.Password;
                createAccountCommand.Parameters.Add("@accountType", SqlDbType.VarChar).Value = newAccount.AccountType;
                createAccountCommand.Parameters.Add("@balance", SqlDbType.Decimal).Value = newAccount.Balance;
                createAccountCommand.Parameters.Add("@status", SqlDbType.VarChar).Value = newAccount.Status;
                if(createAccountCommand.ExecuteNonQuery()!=-1)
                {
                    SqlCommand createCustomerCommand = new SqlCommand($"insert into customer values(@customerId,@name,@age,@dob,@contactNumber,@aadharNumber,@panNumber,@address,@accountId)", conn);
                    createCustomerCommand.Parameters.Add("@customerId", SqlDbType.VarChar).Value = newAccount.AccountId;
                    createCustomerCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = newAccount.Customer.Name;
                    createCustomerCommand.Parameters.Add("@age", SqlDbType.Int).Value = newAccount.Customer.Age;
                    createCustomerCommand.Parameters.Add("@dob", SqlDbType.DateTime).Value = newAccount.Customer.Dob;
                    createCustomerCommand.Parameters.Add("@contactNumber", SqlDbType.VarChar).Value = newAccount.Customer.ContactNumber;
                    createCustomerCommand.Parameters.Add("@aadharNumber", SqlDbType.VarChar).Value = newAccount.Customer.AadharNumber;
                    createCustomerCommand.Parameters.Add("@panNumber", SqlDbType.VarChar).Value = newAccount.Customer.PanNumber;
                    createCustomerCommand.Parameters.Add("@address", SqlDbType.VarChar).Value = newAccount.Customer.Address;
                    createCustomerCommand.Parameters.Add("@accountId", SqlDbType.VarChar).Value = newAccount.AccountId;
                    createCustomerCommand.ExecuteNonQuery();

                }
            }
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
            using(SqlConnection conn = new SqlConnection(SqlHelper.connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from bank where id=@bankid",conn);
                cmd.Parameters.AddWithValue("bankid", bankId);
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    Bank bank = new Bank
                    {
                        BankId = reader["id"].ToString(),
                        BankName = reader["name"].ToString(),
                        Branch = reader["branch"].ToString(),
                        Ifsc = reader["ifsc"].ToString()
                    };
                    return bank;
                }
                return null;
            }
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
            return accountService.GetAccountById(accountId)?.Transactions;
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
            if (bank.SupportedCurrency.Any(c => c.Name.EqualInvariant(newCurrencyName)))
            {
                return false;
            }
            bank.SupportedCurrency.Add(new Currency(newCurrencyName, exchangeRate,bank.BankId));
            JsonFileHelper.WriteData(RBIStorage.banks);
            return true;
        }
        public void UpdateAccount(Account userAccount)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.connectionString))
            {
                conn.Open();
                SqlCommand updateCustomer = new SqlCommand("update customer set name=@name,age=@age,dob=@dob,contactNumber=@contactNumber,aadharNumber=@aadharNumber,panNumber=@panNumber,address=@address where accountId=@accountId", conn);
                updateCustomer.Parameters.Add("@name", SqlDbType.VarChar).Value = userAccount.Customer.Name;
                updateCustomer.Parameters.Add("@age", SqlDbType.Int).Value = userAccount.Customer.Age;
                updateCustomer.Parameters.Add("@dob", SqlDbType.DateTime).Value = userAccount.Customer.Dob;
                updateCustomer.Parameters.Add("@contactNumber", SqlDbType.VarChar).Value = userAccount.Customer.ContactNumber;
                updateCustomer.Parameters.Add("@aadharNumber", SqlDbType.VarChar).Value = userAccount.Customer.AadharNumber;
                updateCustomer.Parameters.Add("@panNumber", SqlDbType.VarChar).Value = userAccount.Customer.PanNumber;
                updateCustomer.Parameters.Add("@address", SqlDbType.VarChar).Value = userAccount.Customer.Address;
                updateCustomer.Parameters.Add("@accountId", SqlDbType.VarChar).Value = userAccount.Customer.AccountId;
                updateCustomer.ExecuteNonQuery();
            }
        }

        public bool DeleteAccount(Account userAccount)
        {
            userAccount.Status = AccountStatus.Closed;
            JsonFileHelper.WriteData(RBIStorage.banks);
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
            JsonFileHelper.WriteData(RBIStorage.banks);
            return isModified;
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
                accountService.DepositAmount(userAccount, transaction.TransactionAmount, bank.SupportedCurrency.FirstOrDefault(c => c.Name.EqualInvariant(bank.DefaultCurrencyName)));
                userAccount.Transactions.Remove(transaction);
            }
            else if (transaction.Type == TransactionType.Transfer)
            {
                Account receiverAccount = accountService.GetAccountById(transaction.ReceiverAccountId);
                accountService.WithdrawAmount(receiverAccount, transaction.TransactionAmount);
                receiverAccount.Transactions.Remove(transaction);
                accountService.DepositAmount(userAccount, transaction.TransactionAmount, bank.SupportedCurrency.FirstOrDefault(c=>c.Name.EqualInvariant(bank.DefaultCurrencyName)));
                userAccount.Transactions.Remove(transaction);


            }
            JsonFileHelper.WriteData(RBIStorage.banks);
            return true;

        }
       
        public Bank GetBankById(string bankid)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from bank where id=@bankid", conn);
                SqlParameter bankidParameter = new SqlParameter("@bankid", bankid);
                cmd.Parameters.Add(bankidParameter);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
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






