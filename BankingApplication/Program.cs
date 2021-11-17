using BankingApplication.Services;
using System;
using BankingApplication.Models;
using System.Collections.Generic;

namespace BankingApplication.CLI
{

    class Program
    {

        public static void Main()
        {
            IDataProvider dataProvider = new JsonFileHelper();
            RBIStorage.banks = dataProvider.GetData<Bank>();
            WelcomeMenu();
        }

        public static void WelcomeMenu()
        {
            AccountHolderPage accountHolderPage = new AccountHolderPage();
            BankEmployeePage employeePage = new BankEmployeePage();
            Console.WriteLine(Constant.welcomeMessage);
            while (true)
            {
                try
                {
                    switch (GetMainMenuByInput(Convert.ToInt32(Console.ReadLine())))
                    {
                        case MainMenu.AccountHolder:
                            accountHolderPage.CustomerInterface();
                            break;
                        case MainMenu.BankEmployee:
                            employeePage.EmployeeInterface();
                            break;
                        case MainMenu.None:
                            Environment.Exit(0);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public static MainMenu GetMainMenuByInput(int value)
        {
            if (value == 1)
                return MainMenu.AccountHolder;
            else if (value == 2)
                return MainMenu.BankEmployee;
            else
                return MainMenu.None;
        }


    }


}
