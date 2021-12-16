using BankAppDbFirstApproach.Models;
using BankAppDbFirstApproach.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppDbFirstApproach.CLI
{
    public static class Factory
    {
        public static IServiceProvider container;
        static Factory()
        {
            container = DependencyInjector.Build();
        }
        public static T GetService<T>()
        {
            return (T)container.GetService(typeof(T));
        }
    }
}