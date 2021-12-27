using BankAppDbFirstApproach.API;
using BankAppDbFirstApproach.Data;
using BankAppDbFirstApproach.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BankAppDbFirstApproach.CLI
{
    public class DependencyInjector
    {
        public static IServiceProvider Build()
        {
            var container = new ServiceCollection();
            container.AddTransient<Program>();
            container.AddTransient<AccountHolderPage>();
            container.AddTransient<BankEmployeePage>();
            container.AddTransient<BankStorageContext>();
            container.AddTransient<ITransactionService, TransactionService>();
            container.AddTransient<IAccountService, AccountService>();
            container.AddTransient<IBankService, BankService>();
            
            
            container.AddSingleton(Startup.mapper);
            return container.BuildServiceProvider();
        }
    }
}
