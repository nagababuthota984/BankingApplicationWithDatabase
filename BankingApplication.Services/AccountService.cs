using BankingApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace BankingApplication.Services
{
    public class AccountService : IAccountService
    {
        private ITransactionService transService = null;
        private BankAppDbContext dbContext = null;
        public AccountService(ITransactionService transactionService,BankAppDbContext context)
        {
            transService = transactionService;
            dbContext = context;
        }

        

        public bool IsValidCustomer(string userName, string password)
        {
            Account acc = dbContext.account.ToList().FirstOrDefault(x => x.UserName.EqualInvariant(userName) && x.Password.EqualInvariant(password));
            if (acc == null)
                return false;
            else
            {
                PrepareCustomerSessionContext(acc);
                return true;
            }

        }
        private void PrepareCustomerSessionContext(Account acc)
        {

            SessionContext.Account = acc;
            SessionContext.Bank = dbContext.bank.ToList().FirstOrDefault(b=>b.BankId.EqualInvariant(acc.BankId)); 
        }
        public Account GetAccountByAccNumber(string accNumber)
        {
            return dbContext.account.ToList().FirstOrDefault(ac=>ac.AccountNumber.EqualInvariant(accNumber));
        }
        public Account GetAccountById(string accountId)
        {
            return dbContext.account.ToList().FirstOrDefault(ac=>ac.AccountId.EqualInvariant(accountId));
        }
        public void DepositAmount(Account userAccount, decimal amount, Currency currency)
        {
            amount *= currency.ExchangeRate;
            userAccount.Balance += amount;
            transService.CreateTransaction(userAccount, TransactionType.Credit, amount, currency.Name);
            dbContext.account.Update(userAccount);
            dbContext.SaveChanges();
        }
        public void WithdrawAmount(Account userAccount, decimal amount)
        {
            userAccount.Balance -= amount;
            transService.CreateTransaction(userAccount, TransactionType.Debit, amount, SessionContext.Bank.DefaultCurrencyName);
            dbContext.account.Update(userAccount);
            dbContext.SaveChanges();
        }
        public void TransferAmount(Account senderAccount, Bank senderBank, Account receiverAccount, decimal amount, ModeOfTransfer mode)
        {
            senderAccount.Balance -= amount;
            receiverAccount.Balance += amount;
            ApplyTransferCharges(senderAccount, senderBank, receiverAccount.BankId, amount, mode, SessionContext.Bank.DefaultCurrencyName);
            dbContext.account.Update(senderAccount);
            dbContext.account.Update(receiverAccount);
            transService.CreateTransferTransaction(senderAccount, receiverAccount, amount, mode, SessionContext.Bank.DefaultCurrencyName);
            dbContext.SaveChanges();

        }
        public void ApplyTransferCharges(Account senderAccount, Bank senderBank, string receiverBankId, decimal amount, ModeOfTransfer mode, string currencyName)
        {
            if (mode == ModeOfTransfer.RTGS)
            {
                //RTGS charge based on transfer to account within the same bank
                if (senderAccount.BankId.EqualInvariant(receiverBankId))
                {
                    decimal charges = (senderBank.SelfRTGS * amount) / 100;
                    senderAccount.Balance -= charges;
                    senderBank.Balance += charges;
                    transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
                }
                else
                {
                    decimal charges = (senderBank.OtherRTGS * amount) / 100;
                    senderAccount.Balance -= charges;
                    senderBank.Balance += charges;
                    transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
                }
            }
            else
            {
                if (senderAccount.BankId.EqualInvariant(receiverBankId))
                {
                    decimal charges = (senderBank.SelfIMPS * amount) / 100;
                    senderAccount.Balance -= charges;
                    senderBank.Balance += charges;
                    transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
                }
                else
                {
                    decimal charges = (senderBank.OtherIMPS * amount) / 100;
                    senderAccount.Balance -= charges;
                    senderBank.Balance += charges;
                    transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
                }
            }
            dbContext.bank.Update(senderBank);
            dbContext.account.Update(senderAccount);
            dbContext.SaveChanges();
        }
    

    }
}

