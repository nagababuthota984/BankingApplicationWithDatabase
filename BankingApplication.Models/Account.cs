using System.Collections.Generic;

namespace BankingApplication.Models
{
    public class Account : BaseBank
    {
        public Account()
        {

        }
        public Account(Customer customer, AccountType type)
        {
            this.Customer = customer;
            this.UserName = $"{customer.Name}{customer.Dob:yyyy}";
            this.Password = $"{customer.Dob:yyyyMMdd}";
            this.AccountId = $"{customer.Name}{customer.Dob:yyyyMMdd}";
            this.Customer.AccountId = this.AccountId;
            this.AccountType = type;
            this.Transactions = new List<Transaction>();
        }

        public Customer Customer { get; set; }
        public string AccountNumber { get; set; }
        public string AccountId { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Balance { get; set; }
        public string UserName { get; set; }
        public string Password { get; private set; }
        public AccountStatus Status { get; set; }
        public List<Transaction> Transactions { get; set; }
        public void SetPassword(string password)
        {
            if (password != null)
            {
                Password = password;
            }
        }
    }
}
