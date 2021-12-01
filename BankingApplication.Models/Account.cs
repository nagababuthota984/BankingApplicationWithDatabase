using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BankingApplication.Models
{
    public class Account
    {
        #region Properties
        public string BankId { get; set; }
        public Customer Customer { get; set; }
        public string AccountNumber { get; set; }
        [Key]
        public string AccountId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public AccountStatus Status { get; set; }
        public List<Transaction> Transactions { get; set; }

        #endregion Properties
        public Account()
        {

        }
        public Account(Customer customer, AccountType type,Bank bank, List<Account> accounts)
        {
            this.Customer = customer;
            string nameTrimmed = String.Concat(customer.Name.Where(c => !Char.IsWhiteSpace(c)));
            this.UserName = $"{nameTrimmed.Substring(0,4)}{customer.Dob:yyyy}{DateTime.Now:ffff}";
            this.Password = $"{customer.Dob:yyyyMMdd}";
            this.AccountId = $"{customer.Name[..3]}{customer.Dob:yyyyMMdd}";
            this.Customer.AccountId = this.AccountId;
            this.AccountType = type;
            this.Transactions = new List<Transaction>();
            this.AccountNumber = GenerateAccountNumber(accounts);
            this.BankId = bank.BankId;
        }

        private string GenerateAccountNumber(List<Account> accounts)
        {
            string accNumber;
            do
            {
                accNumber = GenerateRandomNumber(12);
            } while (accounts.Any(account => account.AccountNumber.Equals(accNumber)));
            return accNumber;
        }

        public void SetPassword(string v)
        {
            this.Password = v;
        }

        private static string GenerateRandomNumber(int length)
        {
            Random r = new Random();
            string accountNumber = "";
            for (int i = 1; i < length; i++)
            {
                accountNumber += r.Next(0, 9).ToString();
            }
            return accountNumber;
        }

    }
}