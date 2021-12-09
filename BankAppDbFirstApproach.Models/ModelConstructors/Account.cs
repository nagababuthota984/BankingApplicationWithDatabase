using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDbFirstApproach.Models
{
    public partial class Account
    {
        public Account(Customer customer, AccountType type, Bank bank, List<Account> accounts)
        {
            this.Customer = customer;
            string nameTrimmed = String.Concat(customer.name.Where(c => !Char.IsWhiteSpace(c)));
            this.username = $"{nameTrimmed.Substring(0, 4)}{customer.dob:yyyy}{DateTime.Now:ffff}";
            this.password = $"{customer.dob:yyyyMMdd}";
            this.accountId = $"{customer.name.Substring(0, 3)}{customer.dob:yyyyMMdd}";
            this.accountType = (int)type;
            this.accountNumber = GenerateAccountNumber(accounts);
            this.bankId = bank.bankId;
        }
        private string GenerateAccountNumber(List<Account> accounts)
        {
            string accNumber;
            do
            {
                accNumber = GenerateRandomNumber(12);
            } while (accounts.Any(account => account.accountNumber.Equals(accNumber)));
            return accNumber;
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
