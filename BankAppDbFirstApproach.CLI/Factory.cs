using BankAppDbFirstApproach.Models;
using BankAppDbFirstApproach.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppDbFirstApproach.CLI
{
    public static class Factory
    {
        public static T GetService<T>()
        {
            return (T)Program.container.GetService(typeof(T));
        }
        public static BankStorageEntities CreateBankAppDbContext()
        {
            return new BankStorageEntities();
        }




    }
}