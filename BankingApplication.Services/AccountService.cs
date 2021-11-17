using BankingApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace BankingApplication.Services
{
    public class AccountService : IAccountService
    {
        ITransactionService transService = new TransactionService();
        IDataProvider dataProvider = new JsonFileHelper();

        public bool IsValidCustomer(string userName, string password)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Account where username=@username and password=@password", conn);
                SqlParameter userNameParameter = new SqlParameter("username", userName);
                SqlParameter passwordParameter = new SqlParameter("password", password);
                cmd.Parameters.Add(userNameParameter);
                cmd.Parameters.Add(passwordParameter);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    prepareSessionContext(reader);
                    return true;
                }
                else
                    return false;

            }
        }

        private void prepareSessionContext(SqlDataReader reader)
        {
            Account userAccount = new Account
            {
                AccountId = reader["accountId"].ToString(),
                AccountNumber = reader["accountNumber"].ToString(),
                UserName = reader["username"].ToString(),
                Balance = Convert.ToDecimal(reader["balance"]),
                BankId = reader["bankid"].ToString()
            };
            userAccount.SetPassword(reader["password"].ToString());
            SessionContext.Account = userAccount;
        }

        public Account GetAccountByAccNumber(string accNumber)
        {
            foreach (var bank in RBIStorage.banks)
            {
                Account account = bank.Accounts.FirstOrDefault(a => (a.AccountNumber.EqualInvariant(accNumber)) && (a.Status != AccountStatus.Closed));
                if (account != null) return account;
            }
            return null;
        }

        public Account GetAccountById(string accountId)
        {
            foreach (Bank bank in RBIStorage.banks)
            {
                Account account = bank.Accounts.FirstOrDefault(a => a.AccountId.EqualInvariant(accountId) && (a.Status != AccountStatus.Closed));
                if (account != null) return account;
            }
            return null;
        }

        public void UpdateAccount(Account userAccount)
        {
            dataProvider.WriteData(RBIStorage.banks);
        }

        public bool DeleteAccount(Account userAccount)
        {
            userAccount.Status = AccountStatus.Closed;
            dataProvider.WriteData(RBIStorage.banks);
            return true;
        }
        public void DepositAmount(Account userAccount, decimal amount, Currency currency)
        {
            amount *= currency.ExchangeRate;
            userAccount.Balance += amount;
            transService.CreateTransaction(userAccount, TransactionType.Credit, amount, currency);
            dataProvider.WriteData(RBIStorage.banks);
        }
        public void WithdrawAmount(Account userAccount, decimal amount)
        {
            amount *= SessionContext.Bank.DefaultCurrency.ExchangeRate;
            userAccount.Balance -= amount;
            transService.CreateTransaction(userAccount, TransactionType.Debit, amount, SessionContext.Bank.DefaultCurrency);
            dataProvider.WriteData(RBIStorage.banks);
        }
        public void TransferAmount(Account senderAccount, Bank senderBank, Account receiverAccount, decimal amount, ModeOfTransfer mode)
        {
            amount *= SessionContext.Bank.DefaultCurrency.ExchangeRate;
            senderAccount.Balance -= amount;
            receiverAccount.Balance += amount;
            ApplyTransferCharges(senderAccount, senderBank, receiverAccount.BankId, amount, mode, SessionContext.Bank.DefaultCurrency);
            transService.CreateTransferTransaction(senderAccount, receiverAccount, amount, mode, SessionContext.Bank.DefaultCurrency);
            dataProvider.WriteData(RBIStorage.banks);
        }
        public void ApplyTransferCharges(Account senderAccount, Bank senderBank, string receiverBankId, decimal amount, ModeOfTransfer mode, Currency currency)
        {
            if (mode == ModeOfTransfer.RTGS)
            {
                //RTGS charge based on transfer to account within the same bank
                if (senderAccount.BankId.EqualInvariant(receiverBankId))
                {
                    decimal charges = (senderBank.SelfRTGS * amount) / 100;
                    senderAccount.Balance -= charges;
                    senderBank.Balance += charges;
                    transService.CreateBankTransaction(senderBank, senderAccount, charges, currency);
                }
                else
                {
                    decimal charges = (senderBank.OtherRTGS * amount) / 100;
                    senderAccount.Balance -= charges;
                    senderBank.Balance += charges;
                    transService.CreateBankTransaction(senderBank, senderAccount, charges, currency);
                }
            }
            else
            {
                if (senderAccount.BankId.EqualInvariant(receiverBankId))
                {
                    decimal charges = (senderBank.SelfIMPS * amount) / 100;
                    senderAccount.Balance -= charges;
                    senderBank.Balance += charges;
                    transService.CreateBankTransaction(senderBank, senderAccount, charges, currency);
                }
                else
                {
                    decimal charges = (senderBank.OtherIMPS * amount) / 100;
                    senderAccount.Balance -= charges;
                    senderBank.Balance += charges;
                    transService.CreateBankTransaction(senderBank, senderAccount, charges, currency);
                }
            }
        }

    }
}

