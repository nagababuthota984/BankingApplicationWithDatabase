using BankAppDbFirstApproach.Services;
using System;
using BankAppDbFirstApproach.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BankAppDbFirstApproach.CLI
{
    public class AccountHolderPage
    {
        private readonly IAccountService accountService;
        private readonly IBankService bankService;
        private readonly Program program;
        private readonly BankStorageEntities dbContext;

        public AccountHolderPage()
        {
            accountService = Factory.GetService<IAccountService>();
            bankService = Factory.GetService<IBankService>();
            program = new Program();
            dbContext = Factory.GetService<BankStorageEntities>();
        }
        public void CustomerInterface()
        {
            Console.WriteLine(Constant.customerInterfaceHeader);
            string userName = UserInput.GetUserName();
            string password = UserInput.GetPassword();
            Console.WriteLine();
            if (!accountService.IsValidCustomer(userName, password))
            {
                Console.WriteLine(Constant.invalidCredentialsError);
                if (Console.ReadLine() == "0")
                {
                    program.WelcomeMenu();
                }
                else
                {
                    CustomerInterface();
                }
            }
            else
            {
                try
                {
                    AccountHolderActions();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void AccountHolderActions()
        {
            switch (UserInput.ShowAccountHolderMenu())
            {
                case AccountHolderMenu.Deposit:
                    DepositInterface();
                    break;

                case AccountHolderMenu.Withdraw:
                    WithdrawInterface();
                    break;
                case AccountHolderMenu.Transfer:
                    TransferInterface();
                    break;
                case AccountHolderMenu.PrintStatement:
                    Console.WriteLine(Constant.transactionHistoryHeader);
                    List<Transaction> transactions = dbContext.Transactions.ToList().FindAll(t=>t.accountId.Equals(SessionContext.Account.accountId));
                    UserOutput.ShowTransactions(transactions);
                    break;
                case AccountHolderMenu.CheckBalance:
                    Console.WriteLine($"\nCurrent Balance - {SessionContext.Account.balance} {SessionContext.Bank.defaultCurrencyName}\n");
                    break;
                case AccountHolderMenu.LogOut:
                    SessionContext.Employee = null;
                    SessionContext.Bank = null;
                    program.WelcomeMenu();
                    return;
            }
            AccountHolderActions();
        }

        private void DepositInterface()
        {
            Console.WriteLine(Constant.moneyDepositHeader);
            decimal amount = UserInput.GetDecimalInput(Constant.amountToDeposit);
            if (amount > 0)
            {
                string name = UserInput.GetInputValue(Constant.currencyName);
                Currency currency = dbContext.Currencies.ToList().FirstOrDefault(c => c.name.EqualInvariant(name) && c.bankId.EqualInvariant(SessionContext.Bank.bankId));
                if (currency != null)
                {
                    accountService.DepositAmount(SessionContext.Account, amount, currency);
                    UserOutput.ShowMessage(Constant.creditSuccess);
                }
                else
                {
                    UserOutput.ShowMessage(Constant.unsupportedCurrency);
                }
            }
            else
            {
                UserOutput.ShowMessage(Constant.invalidAmount);
            }
        }
        private void WithdrawInterface()
        {
            Console.WriteLine(Constant.withdrawlHeader);
            decimal amount = UserInput.GetDecimalInput(Constant.amountToWithdraw);
            if (amount > 0)
            {
                if (amount <= SessionContext.Account.balance)
                {
                    accountService.WithdrawAmount(SessionContext.Account, amount);
                    UserOutput.ShowMessage(Constant.debitSuccess);
                }
                else
                {
                    UserOutput.ShowMessage(Constant.insufficientFunds);
                }
            }
            else
            {
                UserOutput.ShowMessage(Constant.invalidAmount);
            }
        }
        private void TransferInterface()
        {
            Console.WriteLine(Constant.transferHeader);
            string receiverAccNumber = UserInput.GetInputValue(Constant.receiverAccountNumber);
            Account recipientAccount = accountService.GetAccountByAccNumber(receiverAccNumber);

            if (recipientAccount != null)
            {
                decimal amount = UserInput.GetDecimalInput(Constant.amountToTransfer);
                if (amount > 0)
                {
                    if (amount <= SessionContext.Account.balance)
                    {
                        ModeOfTransfer mode = (ModeOfTransfer)UserInput.GetIntegerInput(Constant.transferModeOptions);
                        accountService.TransferAmount(SessionContext.Account, SessionContext.Bank, recipientAccount, amount, mode);
                        UserOutput.ShowMessage(Constant.transferSuccess);
                    }
                    else
                    {
                        UserOutput.ShowMessage(Constant.insufficientFunds);
                    }
                }
                else
                {
                    UserOutput.ShowMessage(Constant.invalidAmount);
                }
            }
            else
            {
                UserOutput.ShowMessage(Constant.recipientAccountNotFound);
            }
        }
    }
}