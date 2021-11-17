using System;
using System.Collections.Generic;
using System.Text;
using BankingApplication.Models;
namespace BankingApplication.CLI
{
    public class UserInput
    {
        public static string GetInputValue(string property)
        {
            Console.WriteLine("Please Enter {0}", property);
            return Console.ReadLine();
        }
        public static AccountHolderMenu ShowAccountHolderMenu()
        {
            Console.WriteLine("\n================CUSTOMER MENU===================");
            Console.WriteLine(Constant.accountHolderOptions);
            Console.WriteLine("==================================================\n");
            return GetAccountHolderMenuByInteger(Convert.ToInt32(Console.ReadLine()));
        }
        private static AccountHolderMenu GetAccountHolderMenuByInteger(int value)
        {
            if (value == 1)
                return AccountHolderMenu.Deposit;
            else if (value == 2)
                return AccountHolderMenu.Withdraw;
            else if (value == 3)
                return AccountHolderMenu.Transfer;
            else if (value == 4)
                return AccountHolderMenu.PrintStatement;
            else if (value == 5)
                return AccountHolderMenu.CheckBalance;
            else if (value == 6)
                return AccountHolderMenu.LogOut;
            else
                return AccountHolderMenu.LogOut;
        }
    }
}
