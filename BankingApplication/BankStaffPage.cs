using System;
using System.Collections.Generic;
using System.Linq;
using BankingApplication.Models;
using BankingApplication.Services;
namespace BankingApplication.CLI
{

    public class BankEmployeePage
    {
        private readonly IBankService bankService;
        private readonly IAccountService accountService;
        private readonly ITransactionService transactionService;
        private readonly Program program;
        private readonly BankAppDbContext dbContext;
        public BankEmployeePage()
        {
            bankService = Factory.CreateBankService();
            accountService = Factory.CreateAccountService();
            transactionService = Factory.CreateTransactionService();
            program = new Program();
            dbContext = Factory.CreateBankAppDbContext();
        }
        public void EmployeeInterface()
        {
            Console.WriteLine(Constant.employeeInterfaceHeader);
            string userName = UserInput.GetUserName();
            string password = UserInput.GetPassword();
            Console.WriteLine();

            if (!bankService.IsValidEmployee(userName, password))
            {
                UserOutput.ShowMessage(Constant.invalidCredentialsError);
                if (Console.ReadLine() == "0")
                    program.WelcomeMenu();
                else
                    EmployeeInterface();
            }
            else
            {
                try
                {
                    EmployeeActions();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private void EmployeeActions()
        {
            Console.WriteLine(Constant.employeeMenuHeader);
            switch (GetBankEmployeeMenuByInput(UserInput.GetIntegerInput(Constant.employeeMenu)))
            {
                case BankEmployeeMenu.CreateAccount:
                    CreateAccountInterface();
                    break;
                case BankEmployeeMenu.AddBank:
                    AddBankInterface();
                    break;
                case BankEmployeeMenu.UpdateAccount:
                    UpdateAccountInterface();
                    break;
                case BankEmployeeMenu.DeleteAccount:
                    DeleteAccountInterface();
                    break;
                case BankEmployeeMenu.AddNewEmployee:
                    AddNewEmployeeInterface();
                    break;
                case BankEmployeeMenu.AddNewCurrency:
                    AddNewCurrencyInterface();
                    break;
                case BankEmployeeMenu.ModifyServiceCharge:
                    ModifyServiceChargeInterface();
                    break;
                case BankEmployeeMenu.ViewTransactionsForAccount:
                    ViewTransactionsForAccountInterface();
                    break;
                case BankEmployeeMenu.ViewTransactions:
                    ViewTransactionsInterface();
                    break;
                case BankEmployeeMenu.RevertTransaction:
                    RevertTransactionInterface();
                    break;
                case BankEmployeeMenu.Logout:
                    SessionContext.Employee = null;
                    SessionContext.Bank = null;
                    program.WelcomeMenu();
                    return;
            }
            EmployeeActions();
        }

        private void ViewTransactionsInterface()
        {
            Console.WriteLine(Constant.viewTransactionsHeader);
            switch (UserInput.GetIntegerInput(Constant.viewTransactionsOptions))
            {
                case 1:
                    DateTime date = GetDateTimeInput(UserInput.GetInputValue("Date DD-MM-YYYY"));
                    UserOutput.ShowTransactions(bankService.GetTransactionsByDate(date, SessionContext.Bank));
                    break;
                case 2:
                    UserOutput.ShowTransactions(bankService.GetTransactions(SessionContext.Bank));
                    break;
                default:
                    return;
            }
        }

        private void CreateAccountInterface()
        {
            Console.WriteLine(Constant.accountCreationHeader);
            Customer newCustomer = GetDetailsOfNewCustomer();
            AccountType accountType = (AccountType)UserInput.GetIntegerInput(Constant.accountTypeOptions);
            Account newAccount = new Account(newCustomer, accountType,SessionContext.Bank, dbContext.account.ToList());
            bankService.CreateAndAddAccount(newAccount, SessionContext.Bank);
            newCustomer.CustomerId = newAccount.AccountId;
            UserOutput.ShowMessage($"Account has been created!\nCredentials:Username - {newAccount.UserName}\nPassword - {newAccount.Password}\nAccount Number - {newAccount.AccountNumber}\n");
        }

        private Customer GetDetailsOfNewCustomer()
        {
            string name = GetName();
            int age = GetAge();
            Gender gender = GetGenderByInput(UserInput.GetIntegerInput(Constant.genderOptions));
            DateTime dob = GetDateTimeInput(UserInput.GetInputValue(Constant.dateOfBirth));
            string contactNumber = UserInput.GetInputValue(Constant.contactNumber);
            long aadharNumber = UserInput.GetLongInput(Constant.aadharNumber);
            string panNumber = GetPanNumber();
            string address = UserInput.GetInputValue(Constant.address);
            Customer newCustomer = new Customer(name, age, gender, dob, contactNumber, aadharNumber, panNumber, address);
            return newCustomer;
        }

        private void AddBankInterface()
        {
            Console.WriteLine(Constant.addBankHeader);
            string bankName = GetName();
            if (!dbContext.bank.ToList().Any(bank => bank.BankName.EqualInvariant(bankName)))
            {
                string branch = UserInput.GetInputValue(Constant.branch);
                string ifsc = UserInput.GetInputValue(Constant.Ifsc);
                Bank newBank = bankService.CreateAndGetBank(bankName, branch, ifsc);
                if (newBank == null)
                    UserOutput.ShowMessage(Constant.bankNotCreated);
                else
                    UserOutput.ShowMessage($"Bank created with bank id - {newBank.BankId}");
            }
            else
            {
                UserOutput.ShowMessage(Constant.bankAlreadyExists);
            }
        }
        private void UpdateAccountInterface()
        {
            string accountId = UserInput.GetInputValue(Constant.accountId);
            Account userAccount = dbContext.account.ToList().FirstOrDefault(acc => acc.AccountId.EqualInvariant(accountId));
            if (userAccount != null)
            {
                Customer customer = dbContext.customer.ToList().FirstOrDefault(cust => cust.AccountId.EqualInvariant(accountId));
                if (customer!=null)
                {
                    UpdateAccountHandler(customer);
                    Console.WriteLine(Constant.updateConfirmation);
                    if (Console.ReadLine().EqualInvariant("y"))
                    {
                        bankService.UpdateAccount(customer);
                        Console.WriteLine(Constant.updateSuccess);
                    }
                    else
                        Console.WriteLine(Constant.updateFail); 
                }
            }
            else
                UserOutput.ShowMessage(Constant.accountNotFoundError);
        }
        private void UpdateAccountHandler(Customer customer)
        {
            Console.WriteLine(Constant.updateMenuHeader);
            Console.WriteLine(Constant.customerPropertiesMenu);
            switch (GetCustomerPropertyByInteger(Convert.ToInt32(Console.ReadLine())))
            {
                case CustomerProperties.Name:
                    Console.WriteLine($"[Existing : {customer.Name}]");
                    customer.Name = GetName();
                    break;
                case CustomerProperties.Age:
                    Console.WriteLine($"[Existing : {customer.Age}]");
                    customer.Age = GetAge();
                    break;
                case CustomerProperties.Dob:
                    Console.WriteLine($"[Existing : {customer.Dob}]");
                    customer.Dob = GetDateTimeInput(UserInput.GetInputValue("Date of Birth"));
                    break;
                case CustomerProperties.AadharNumber:
                    Console.WriteLine($"[Existing : {customer.AadharNumber}]");
                    customer.AadharNumber = UserInput.GetLongInput("Aadhar number");
                    break;
                case CustomerProperties.PanNumber:
                    Console.WriteLine($"[Existing : {customer.PanNumber}]");
                    customer.PanNumber = UserInput.GetInputValue("Pan number");
                    break;
                case CustomerProperties.ContactNumber:
                    Console.WriteLine($"[Existing : {customer.ContactNumber}]");
                    customer.ContactNumber = UserInput.GetInputValue("Contact number");
                    break;
                case CustomerProperties.Address:
                    Console.WriteLine($"[Existing : {customer.Address}]");
                    customer.Address = UserInput.GetInputValue("Address");
                    break;
                default:
                    return;

            }
            UpdateAccountHandler(customer);
        }
        private void DeleteAccountInterface()
        {
            string accountId = UserInput.GetInputValue(Constant.accountId);
            Account userAccount = dbContext.account.ToList().FirstOrDefault(acc => acc.AccountId.EqualInvariant(accountId));
            if (userAccount != null)
            {
                Console.WriteLine(Constant.deleteAccountConfirmation);
                if (Console.ReadLine().Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    if (bankService.DeleteAccount(userAccount))
                    {
                        UserOutput.ShowMessage(Constant.accountDeleteSuccess);
                    }
                    else
                    {
                        UserOutput.ShowMessage(Constant.accountDeleteFail);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                UserOutput.ShowMessage(Constant.accountNotFoundError);
            }
        }
        private void AddNewEmployeeInterface()
        {
            Console.WriteLine(Constant.addNewEmployeeHeader);
            Customer newCustomer = GetDetailsOfNewCustomer();
            EmployeeDesignation role = (EmployeeDesignation)UserInput.GetIntegerInput(Constant.designationOptions);
            Employee newEmployee = bankService.CreateAndGetEmployee(newCustomer, role, SessionContext.Bank);
            UserOutput.ShowMessage($"Employee {newEmployee.customer.Name} has been added! Credentials:\n{newEmployee.UserName}\n{newEmployee.Password}\n");
        }
        private void AddNewCurrencyInterface()
        {
            string newCurrencyName = UserInput.GetInputValue(Constant.newCurrencyName);
            decimal exchangeRate = UserInput.GetDecimalInput(Constant.newExchangeRate);
            if (exchangeRate > 0)
            {
                if (bankService.AddNewCurrency(SessionContext.Bank, newCurrencyName, exchangeRate))
                    UserOutput.ShowMessage(Constant.currencyAdded);
                else
                    UserOutput.ShowMessage(Constant.currencyAlreadyExists);
            }
            else
            {
                UserOutput.ShowMessage(Constant.invalidExchangeRate);
            }
        }
        private void ModifyServiceChargeInterface()
        {
            ModeOfTransfer mode = (ModeOfTransfer)Convert.ToInt32(UserInput.GetInputValue(Constant.transferModeOptions));
            bool isSelfBankTransfer = UserInput.GetIntegerInput(Constant.selfOrOtherOptions).Equals(1);
            decimal value = UserInput.GetDecimalInput(Constant.newChargeValue);
            Console.WriteLine(Constant.updateConfirmation);
            if (Console.ReadLine().EqualInvariant("y"))
            {
                if (bankService.ModifyServiceCharge(mode, isSelfBankTransfer, SessionContext.Bank, value))
                {
                    UserOutput.ShowMessage(Constant.updateSuccess);
                }
                else
                {
                    UserOutput.ShowMessage(Constant.updateFail);
                }
            }
        }
        private void ViewTransactionsForAccountInterface()
        {
            string accountId = UserInput.GetInputValue(Constant.accountId);
            List<Transaction> transactions = bankService.GetAccountTransactions(accountId);
            if (transactions != null)
            {
                UserOutput.ShowTransactions(transactions);
            }
            else
            {
                UserOutput.ShowMessage(Constant.accountNotFoundError);
            }
        }
        private void RevertTransactionInterface()
        {
            string transactionId = UserInput.GetInputValue(Constant.transactionId);
            Transaction transaction = transactionService.GetTransactionById(transactionId);
            if (transaction != null)
            {
                if (transaction.SenderBankId.Equals(transaction.ReceiverBankId, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(Constant.revertConfirmation);
                    if (Console.ReadLine().Equals("y", StringComparison.OrdinalIgnoreCase))
                    {
                        if (bankService.RevertTransaction(transaction, SessionContext.Bank))
                        {
                            Console.WriteLine(Constant.revertSuccess);
                        }
                        else
                        {
                            Console.WriteLine(Constant.revertFail);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Console.WriteLine(Constant.otherBankInvolvedRevertFail);
                }
            }
            else
            {
                UserOutput.ShowMessage(Constant.noSuchTransaction);
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
        private BankEmployeeMenu GetBankEmployeeMenuByInput(int v)
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
                return BankEmployeeMenu.ModifyServiceCharge;
            else if (v == 8)
                return BankEmployeeMenu.ViewTransactionsForAccount;
            else if (v == 9)
                return BankEmployeeMenu.ViewTransactions;
            else if (v == 10)
                return BankEmployeeMenu.RevertTransaction;
            else
                return BankEmployeeMenu.Logout;
        }
        private Gender GetGenderByInput(int v)
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
        private string GetName()
        {
            Console.WriteLine("Please enter name:");
            string name = Console.ReadLine();
            if (hasSpecialChar(name) || name.Length < 3 || name.Any(Char.IsDigit))
            {
                Console.WriteLine("Name should not contain special characters. please enter again ");
                return GetName();
            }
            else
            {
                return name;
            }
        }
        private string GetPanNumber()
        {
            Console.WriteLine("Please enter your PAN number");
            string pan = Console.ReadLine();
            if (!hasSpecialChar(pan))
                return pan;
            else
            {
                Console.WriteLine("PAN doesnt contain special characters. Enter again");
                return GetPanNumber();
            }
        }
        private bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        private int GetAge()
        {
            int age = UserInput.GetIntegerInput("age");
            if (age <= 0 || age > 100)
            {
                Console.WriteLine(Constant.invalidAgeFormat);
                return GetAge();
            }
            return age;
        }
        private DateTime GetDateTimeInput(string datetime)
        {
            DateTime dob;
            try
            {
                return Convert.ToDateTime(datetime);
            }
            catch (Exception e)
            {
                Console.WriteLine(Constant.invalidDateFormat);
                dob = GetDateTimeInput(Console.ReadLine());

            }
            return dob;

        }


    }
}