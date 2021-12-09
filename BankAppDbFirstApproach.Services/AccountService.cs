using BankAppDbFirstApproach.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace BankAppDbFirstApproach.Services
{
    public class AccountService : IAccountService
    {
        private ITransactionService transService;
        private BankStorageEntities dbContext;
        public AccountService(ITransactionService transactionService,BankStorageEntities context)
        {
            transService = transactionService;
            dbContext = context;
        }

        

        public bool IsValidCustomer(string userName, string password)
        {
            Account acc = dbContext.Accounts.FirstOrDefault(acc => acc.username.Equals(userName) && acc.password.Equals(password) && acc.status == (int)AccountStatus.Active);
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
            SessionContext.Bank = dbContext.Banks.FirstOrDefault(b=>b.bankId.Equals(acc.bankId)); 
        }
        public Account GetAccountByAccNumber(string accNumber)
        {
            return dbContext.Accounts.FirstOrDefault(ac=>ac.accountNumber.Equals(accNumber));
        }
        public Account GetAccountById(string accountId)
        {
            return dbContext.Accounts.FirstOrDefault(ac=>ac.accountId.Equals(accountId));
        }
        public void DepositAmount(Account userAccount, decimal amount, Currency currency)
        {
            amount *= currency.exchangeRate;
            userAccount.balance += amount;
            transService.CreateTransaction(userAccount, TransactionType.Credit, amount, currency.name);
            dbContext.SaveChanges();
        }
        public void WithdrawAmount(Account userAccount, decimal amount)
        {
            userAccount.balance -= amount;
            transService.CreateTransaction(userAccount, TransactionType.Debit, amount, SessionContext.Bank.defaultCurrencyName);
            dbContext.SaveChanges();
        }
        public void TransferAmount(Account senderAccount, Bank senderBank, Account receiverAccount, decimal amount, ModeOfTransfer mode)
        {
            senderAccount.balance -= amount;
            receiverAccount.balance += amount;
            ApplyTransferCharges(senderAccount, senderBank, receiverAccount.bankId, amount, mode, SessionContext.Bank.defaultCurrencyName);
            //dbContext.Accounts.Update(senderAccount);
            //dbContext.Accounts.Update(receiverAccount);
            transService.CreateTransferTransaction(senderAccount, receiverAccount, amount, mode, SessionContext.Bank.defaultCurrencyName);
            dbContext.SaveChanges();

        }
        public void ApplyTransferCharges(Account senderAccount, Bank senderBank, string receiverBankId, decimal amount, ModeOfTransfer mode, string currencyName)
        {
            if (mode == ModeOfTransfer.RTGS)
            {
                //RTGS charge based on transfer to account within the same bank
                if (senderAccount.bankId.Equals(receiverBankId))
                {
                    decimal charges = (senderBank.selfRTGS * amount) / 100;
                    senderAccount.balance -= charges;
                    senderBank.balance += charges;
                    transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
                }
                else
                {
                    decimal charges = (senderBank.otherRTGS * amount) / 100;
                    senderAccount.balance -= charges;
                    senderBank.balance += charges;
                    transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
                }
            }
            else
            {
                if (senderAccount.bankId.Equals(receiverBankId))
                {
                    decimal charges = (senderBank.selfIMPS * amount) / 100;
                    senderAccount.balance -= charges;
                    senderBank.balance += charges;
                    transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
                }
                else
                {
                    decimal charges = (senderBank.otherIMPS * amount) / 100;
                    senderAccount.balance -= charges;
                    senderBank.balance += charges;
                    transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
                }
            }
            //dbContext.Banks.Update(senderBank);
            //dbContext.account.Update(senderAccount);
            dbContext.SaveChanges();
        }
    

    }
}

