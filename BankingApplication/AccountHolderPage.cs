﻿using BankingApplication.Services;
using System;
using BankingApplication.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankingApplication.CLI
{
    public class AccountHolderPage
    {
        static IAccountService accountService;
        static IBankService bankService;
        static Program program;
        public AccountHolderPage()
        {
            accountService = Factory.CreateAccountService();
            bankService = Factory.CreateBankService();
            program = new Program();
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
                    SessionContext.Bank = bankService.GetBankById(SessionContext.Account.BankId);
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
                    UserOutput.ShowTransactions(SessionContext.Account.Transactions);
                    break;
                case AccountHolderMenu.CheckBalance:
                    Console.WriteLine($"\nCurrent Balance - {SessionContext.Account.Balance} {SessionContext.Bank.DefaultCurrency.Name}\n");
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
                string Name = UserInput.GetInputValue(Constant.currencyName);
                Currency currency = SessionContext.Bank.SupportedCurrency.FirstOrDefault(c => c.Name.EqualInvariant(Name));
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
                if (amount <= SessionContext.Account.Balance)
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
                    if (amount <= SessionContext.Account.Balance)
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