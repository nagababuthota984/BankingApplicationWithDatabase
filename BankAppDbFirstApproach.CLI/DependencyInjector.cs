using BankAppDbFirstApproach.Models;
using BankAppDbFirstApproach.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDbFirstApproach.CLI
{
    public class DependencyInjector
    {
        public static IServiceProvider Build()
        {

            var container = new ServiceCollection();
            container.AddTransient<BankStorageEntities>();
            container.AddTransient<ITransactionService, TransactionService>();
            container.AddTransient<IAccountService, AccountService>();
            container.AddTransient<IBankService, BankService>();
            return container.BuildServiceProvider();
        }   
    }
}
