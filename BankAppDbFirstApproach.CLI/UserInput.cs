using System;
using System.Collections.Generic;
using System.Text;
using BankAppDbFirstApproach.Models;
namespace BankAppDbFirstApproach.CLI
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
            Console.WriteLine(Constant.customerMenuHeader);
            Console.WriteLine(Constant.accountHolderOptions);
            Console.WriteLine("==================================================\n");
            return GetAccountHolderMenuByInteger(Convert.ToInt32(Console.ReadLine()));
        }

        internal static string GetPassword()
        {
            Console.WriteLine("Please enter your password");
            string password = Console.ReadLine();
            if (string.IsNullOrEmpty(password) || password.Length < 5)
            {
                Console.WriteLine(Constant.invalidPassword);
                return GetPassword();
            }
            else return password;
        }

        internal static string GetUserName()
        {
            Console.WriteLine("Please enter your username");
            string username = Console.ReadLine();
            if (string.IsNullOrEmpty(username) || username.Length < 5)
            {
                Console.WriteLine(Constant.invalidUserName);
                return GetUserName();
            }
            else return username;
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

        internal static decimal GetDecimalInput(string message)
        {
            Console.WriteLine($"Enter {message}:");
            if (decimal.TryParse(Console.ReadLine(), out decimal result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("\nPlease enter a valid decimal value.\n");
                return GetDecimalInput(message);
            }


        }

        internal static long GetLongInput(string message)
        {
            Console.WriteLine($"Enter {message}:");
            if (long.TryParse(Console.ReadLine(), out long result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("\nPlease enter a valid 12-digit value.\n");
                return GetLongInput(message);
            }
        }

        internal static int GetIntegerInput(string message)
        {
            Console.WriteLine($"Enter {message}:");
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("\nPlease enter a valid Integer value.\n");
                return GetIntegerInput(message);
            }
        }


    }
}