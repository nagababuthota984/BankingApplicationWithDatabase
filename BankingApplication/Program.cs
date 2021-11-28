using BankingApplication.Services;
using System;
using BankingApplication.Models;

namespace BankingApplication.CLI
{

    class Program
    {
        private static AccountHolderPage accountHolderPage;
        private static BankEmployeePage employeePage;
        public static void Main()
        {
            RBIStorage.banks = JsonFileHelper.GetData<Bank>(Constant.filePath);
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
