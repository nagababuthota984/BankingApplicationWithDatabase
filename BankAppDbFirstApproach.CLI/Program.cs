﻿using BankAppDbFirstApproach.Models;


namespace BankAppDbFirstApproach.CLI
{

    public class Program
    {
        private AccountHolderPage accountHolderPage;
        private BankEmployeePage employeePage;
        public static IServiceProvider container;
        public static void Main()
        {
            Program p = new Program();
            p.InitializeUI();
        }

        private void InitializeUI()
        {
            accountHolderPage = Factory.GetService<AccountHolderPage>();
            employeePage = Factory.GetService<BankEmployeePage>();
            WelcomeMenu();
        }
        public void WelcomeMenu()
        {
            Console.WriteLine(Constant.welcomeMessage);
            try
            {
                switch (GetMainMenuByInput(UserInput.GetIntegerInput("choice")))
                {
                    case MainMenu.AccountHolder:
                        accountHolderPage.CustomerLoginInterface();
                        WelcomeMenu();
                        break;
                    case MainMenu.BankEmployee:
                        employeePage.EmployeeInterface();
                        WelcomeMenu();
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
