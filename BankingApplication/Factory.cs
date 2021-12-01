using BankingApplication.Models;
using BankingApplication.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.CLI
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
       
        public static BankAppDbContext CreateBankAppDbContext()
        {
            return new BankAppDbContext();
        }

    }
}