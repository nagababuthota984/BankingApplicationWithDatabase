using BankAppDbFirstApproach.Models;
using BankAppDbFirstApproach.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppDbFirstApproach.CLI
{
    public static class Factory
    {
        public static IAccountService CreateAccountService()
        {
            return new AccountService(CreateTransactionService(),CreateBankAppDbContext());
        }
        public static IBankService CreateBankService()
        {
            return new BankService(CreateAccountService(),CreateBankAppDbContext());
        }
        public static ITransactionService CreateTransactionService()
        {
            return new TransactionService(CreateBankAppDbContext());
        }
       
        public static BankStorageEntities CreateBankAppDbContext()
        {
            return new BankStorageEntities();
        }

    }
}