using BankAppDbFirstApproach.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDbFirstApproach.CLI
{
    public class DependencyInjector
    {
        public  IServiceProvider Build()
        {
            var container = new ServiceCollection();
            container.AddTransient<IAccountService, AccountService>();
            container.AddTransient<IBankService, BankService>();
            container.AddTransient<ITransactionService, TransactionService>();
            return container.BuildServiceProvider();
        }
    }
}
