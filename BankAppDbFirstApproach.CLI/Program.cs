using BankAppDbFirstApproach.Services;
using System;
using BankAppDbFirstApproach.Models;
using System.Linq;

namespace BankAppDbFirstApproach.CLI
{

    class Program
    {
        private  AccountHolderPage accountHolderPage;
        private  BankEmployeePage employeePage;
        public static void Main()
        {
            
            Program p = new Program();
            p.InitializeUI();
        }

        private void InitializeUI()
        {
            accountHolderPage = new AccountHolderPage();
            employeePage = new BankEmployeePage();
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
                        accountHolderPage.CustomerInterface();
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
