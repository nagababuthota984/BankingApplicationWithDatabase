using System;
using System.Collections.Generic;
using System.Linq;
using BankingApplication.Models;
using BankingApplication.Services;
namespace BankingApplication.CLI
{

    public class BankEmployeePage
    {
        private readonly IBankService bankService = new BankService();
        private readonly IAccountService accountService = new AccountService();
        private readonly ITransactionService transactionService = new TransactionService();
        Bank bank;
        Employee currentWorkingEmployee;

        public void EmployeeInterface()
        {
            Console.WriteLine("\n===================BANK EMPLOYEE LOGIN===================\n");
            string userName = UserInput.GetInputValue("Username");
            string password = UserInput.GetInputValue("Password");
            Console.WriteLine();

            if (!bankService.IsValidEmployee(userName, password))
            {
                UserOutput.ShowMessage("Invalid Credentials. Please try again or enter 0 for Main menu\n");
                if (Console.ReadLine() == "0")
                    Program.WelcomeMenu();
                else
                    EmployeeInterface();
            }
            else
            {
                try
                {
                    bank = SessionContext.Bank;
                    currentWorkingEmployee = SessionContext.Employee;
                    while (true)
                    {
                        Console.WriteLine("\n========================BANK STAFF MENU==========================\n");
                        switch (GetBankEmployeeMenuByInteger(Convert.ToInt32(UserInput.GetInputValue(Constant.employeeMenu))))
                        {
                            case BankEmployeeMenu.CreateAccount:
                                Console.WriteLine("\t-------Account Creation-------\n");
                                string name = AskName();
                                string age = AskAge();
                                Gender gender = GetGenderByInteger(Convert.ToInt32(UserInput.GetInputValue("Gender:\n1.Male\n2.Female\n3.Prefer Not to say")));
                                DateTime dob = Convert.ToDateTime(UserInput.GetInputValue("Date of Birth"));
                                string contactNumber = UserInput.GetInputValue("Contact Number");
                                string aadharNumber = UserInput.GetInputValue("Aadhar Number");
                                string panNumber = UserInput.GetInputValue("PAN Number");
                                string address = UserInput.GetInputValue("Address");
                                AccountType accountType = (AccountType)Convert.ToInt32(UserInput.GetInputValue("Account Type(1.Savings/2.Current)"));
                                Customer newCustomer = new Customer(name, age, gender, dob, contactNumber, aadharNumber, panNumber, address);
                                Account newAccount = new Account(newCustomer, accountType);
                                bankService.CreateAccount(newAccount, bank);
                                UserOutput.ShowMessage($"Account has been created!\nCredentials:Username - {newAccount.UserName}\nPassword - {newAccount.Password}\nAccount Number - {newAccount.AccountNumber}\n");
                                break;
                            case BankEmployeeMenu.AddBank:
                                string bankName = UserInput.GetInputValue("Name of the bank");
                                string branch = UserInput.GetInputValue("Branch");
                                string ifsc = UserInput.GetInputValue("IFSC");
                                Bank newBank = bankService.CreateAndGetBank(bankName, branch, ifsc);
                                if (newBank == null)
                                    UserOutput.ShowMessage("Bank not created! Try again.");
                                else
                                    UserOutput.ShowMessage($"Bank created with bank id - {newBank.BankId}");
                                break;
                            case BankEmployeeMenu.UpdateAccount:
                                string accountId = UserInput.GetInputValue("Account Id");
                                Account userAccount = accountService.GetAccountById(accountId);
                                if (userAccount != null)
                                {
                                    UpdateAccountHandler(userAccount);
                                    accountService.UpdateAccount(userAccount);

                                }

                                else
                                {
                                    UserOutput.ShowMessage(Constant.accountNotFoundError);
                                }
                                break;
                            case BankEmployeeMenu.DeleteAccount:
                                accountId = UserInput.GetInputValue("Account Id");
                                userAccount = accountService.GetAccountById(accountId);
                                if (userAccount != null)
                                {
                                    if (accountService.DeleteAccount(userAccount))
                                    {
                                        UserOutput.ShowMessage("Account Deleted");
                                    }
                                    else
                                    {
                                        UserOutput.ShowMessage("Account was not deleted. Try again later.");
                                    }
                                }
                                else
                                {
                                    UserOutput.ShowMessage(Constant.accountNotFoundError);
                                }
                                break;
                            case BankEmployeeMenu.AddNewEmployee:
                                Console.WriteLine("\n-----------Add New Employee-----------\n");
                                name = AskName();
                                age = AskAge();
                                dob = Convert.ToDateTime(UserInput.GetInputValue("Employee Date of Birth"));
                                gender = GetGenderByInteger(Convert.ToInt32(UserInput.GetInputValue("Employee Gender")));
                                EmployeeDesignation role = (EmployeeDesignation)Convert.ToInt32(UserInput.GetInputValue("Employee Designation"));
                                Employee newEmployee = bankService.CreateAndGetEmployee(name, age, dob, gender, role, bank);
                                UserOutput.ShowMessage($"Employee {newEmployee.Name} has been added! Credentials:\n{newEmployee.UserName}\n{newEmployee.Password}\n");
                                break;
                            case BankEmployeeMenu.AddNewCurrency:
                                string newCurrency = UserInput.GetInputValue("new currency type");
                                decimal exchangeRate = Convert.ToDecimal(UserInput.GetInputValue("exchange rate"));
                                if (exchangeRate > 0)
                                {
                                    if (bankService.AddNewCurrency(bank, newCurrency, exchangeRate))
                                        UserOutput.ShowMessage("New Currency Added!");
                                    else
                                        UserOutput.ShowMessage("Currency Already Exists!");
                                }
                                else
                                {
                                    UserOutput.ShowMessage("Invalid exchange rate");
                                }
                                break;
                            case BankEmployeeMenu.SetServiceCharge:
                                ModeOfTransfer mode = (ModeOfTransfer)Convert.ToInt32(UserInput.GetInputValue("Change service charge:\n1.RTGS\n2.IMPS"));
                                bool isSelfBankTransfer = (Convert.ToInt32(UserInput.GetInputValue("Charge type:\n1.Money Transfer Within bank.\n2.Money transfer to other banks")).Equals(1)) ? true : false;
                                decimal value = Convert.ToDecimal(UserInput.GetInputValue("New Charge Value:"));
                                if (bankService.SetServiceCharge(mode, isSelfBankTransfer, bank, value))
                                {
                                    UserOutput.ShowMessage("Updation success");
                                }
                                else
                                {
                                    UserOutput.ShowMessage("Cannot update. Try again.");
                                }
                                break;
                            case BankEmployeeMenu.ViewTransactions:
                                accountId = UserInput.GetInputValue("Account Id");
                                List<Transaction> transactions = bankService.GetAccountTransactions(accountId);
                                if (transactions != null)
                                {
                                    UserOutput.ShowTransactions(transactions);
                                }
                                else
                                {
                                    UserOutput.ShowMessage(Constant.accountNotFoundError);
                                }
                                break;
                            case BankEmployeeMenu.RevertTransaction:
                                string transactionId = UserInput.GetInputValue("Transaction Id");
                                Transaction transaction = transactionService.GetTransactionById(transactionId);
                                if (transaction != null)
                                {
                                    if (transaction.SenderBankId.Equals(transaction.ReceiverBankId, StringComparison.OrdinalIgnoreCase))
                                    {
                                        Console.WriteLine("Are you sure you want to revert the transaction(Y/N)?\n");
                                        if (Console.ReadLine().Equals("y", StringComparison.OrdinalIgnoreCase))
                                        {
                                            if(bankService.RevertTransaction(transaction, bank))
                                            {
                                                Console.WriteLine("Reverted Successfully!\n");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Could not revert the transaction. Please try again\n");
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cannot revert transaction involving other bank.");
                                    }
                                }
                                else
                                {
                                    UserOutput.ShowMessage("No such transaction found!\n");
                                }
                                break;
                            case BankEmployeeMenu.Logout:
                                SessionContext.Employee = null;
                                SessionContext.Bank = null;
                                Program.WelcomeMenu();
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        private bool UpdateAccountHandler(Account userAccount)
        {

            while (true)
            {
                Console.WriteLine("\n--------------UPDATE MENU----------------");
                Console.WriteLine(Constant.customerPropertiesMenu);
                switch (GetCustomerPropertyByInteger(Convert.ToInt32(Console.ReadLine())))
                {
                    case CustomerProperties.Name:
                        Console.WriteLine($"[Existing : {userAccount.Customer.Name}]");
                        userAccount.Customer.Name = AskName();
                        break;
                    case CustomerProperties.Age:
                        Console.WriteLine($"[Existing : {userAccount.Customer.Age}]");
                        userAccount.Customer.Age = AskAge();
                        break;
                    case CustomerProperties.Dob:
                        Console.WriteLine($"[Existing : {userAccount.Customer.Dob}]");
                        userAccount.Customer.Dob = Convert.ToDateTime(UserInput.GetInputValue("Date of Birth"));
                        break;
                    case CustomerProperties.AadharNumber:
                        Console.WriteLine($"[Existing : {userAccount.Customer.AadharNumber}]");
                        userAccount.Customer.AadharNumber = UserInput.GetInputValue("Aadhar number");
                        break;
                    case CustomerProperties.PanNumber:
                        Console.WriteLine($"[Existing : {userAccount.Customer.PanNumber}]");
                        userAccount.Customer.PanNumber = UserInput.GetInputValue("Pan number");
                        break;
                    case CustomerProperties.ContactNumber:
                        Console.WriteLine($"[Existing : {userAccount.Customer.ContactNumber}]");
                        userAccount.Customer.ContactNumber = UserInput.GetInputValue("Contact number");
                        break;
                    case CustomerProperties.Address:
                        Console.WriteLine($"[Existing : {userAccount.Customer.Address}]");
                        userAccount.Customer.Address = UserInput.GetInputValue("Address");
                        break;
                    default:
                        return true;

                }
            }
        }


        private CustomerProperties GetCustomerPropertyByInteger(int v)
        {
            if (v == 1)
                return CustomerProperties.Name;
            else if (v == 2)
                return CustomerProperties.Age;
            else if (v == 3)
                return CustomerProperties.Gender;
            else if (v == 4)
                return CustomerProperties.Dob;
            else if (v == 5)
                return CustomerProperties.AadharNumber;
            else if (v == 6)
                return CustomerProperties.PanNumber;
            else if (v == 7)
                return CustomerProperties.ContactNumber;
            else if (v == 8)
                return CustomerProperties.Address;
            else
                return CustomerProperties.None;


        }

        private Gender GetGenderByInteger(int v)
        {
            if (v == 1)
            {
                return Gender.Male;
            }
            else if (v == 2)
            {
                return Gender.Female;
            }
            else
            {
                return Gender.PreferNotToSay;
            }
        }
        private string AskName()
        {
            Console.WriteLine("Please enter customer's name:");
            string name = Console.ReadLine();
            while (name.Any(Char.IsDigit))
            {
                Console.WriteLine("Name should not have digits in it.Please enter the name again:\n");
                name = Console.ReadLine();
            }
            return name;
        }
        private string AskAge()
        {
            Console.WriteLine("Please enter customer's age");
            int age = Convert.ToInt32(Console.ReadLine());
            while (age <= 0 || age > 100)
            {
                Console.WriteLine("Please enter valid age. Re-enter age:");
                age = Convert.ToInt32(Console.ReadLine());
            }
            return age.ToString();
        }
        private BankEmployeeMenu GetBankEmployeeMenuByInteger(int v)
        {
            if (v == 1)
                return BankEmployeeMenu.CreateAccount;
            else if (v == 2)
                return BankEmployeeMenu.AddBank;
            else if (v == 3)
                return BankEmployeeMenu.UpdateAccount;
            else if (v == 4)
                return BankEmployeeMenu.DeleteAccount;
            else if (v == 5)
                return BankEmployeeMenu.AddNewEmployee;
            else if (v == 6)
                return BankEmployeeMenu.AddNewCurrency;
            else if (v == 7)
                return BankEmployeeMenu.SetServiceCharge;
            else if (v == 8)
                return BankEmployeeMenu.ViewTransactions;
            else if (v == 9)
                return BankEmployeeMenu.RevertTransaction;
            else
                return BankEmployeeMenu.Logout;
        }


    }
}
