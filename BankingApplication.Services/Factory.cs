using BankingApplication.Models;
using BankingApplication.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Services
{
    public static class Factory
    {
        public static IAccountService CreateAccountService()
        {
            return new AccountService(CreateTransactionService());
        }
        public static IBankService CreateBankService()
        {
            return new BankService(CreateAccountService());
        }
        public static ITransactionService CreateTransactionService()
        {
            return new TransactionService();
        }
        public static JsonFileHelper CreateJsonFileHelperService()
        {
            return new JsonFileHelper();
        }

    }
}