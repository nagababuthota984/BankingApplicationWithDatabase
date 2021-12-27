using System.Collections.Generic;

namespace BankAppDbFirstApproach.Data
{
    public partial class Bank
    {
        public Bank()
        {
            Account = new HashSet<Account>();
            Currency = new HashSet<Currency>();
            Employee = new HashSet<Employee>();
            TransactionBank = new HashSet<Transaction>();
            TransactionOtherPartyBank = new HashSet<Transaction>();
        }

        public string BankId { get; set; }
        public string Bankname { get; set; }
        public string Branch { get; set; }
        public string Ifsc { get; set; }
        public decimal SelfRtgs { get; set; }
        public decimal SelfImps { get; set; }
        public decimal OtherRtgs { get; set; }
        public decimal OtherImps { get; set; }
        public decimal Balance { get; set; }
        public string DefaultCurrencyName { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Currency> Currency { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<Transaction> TransactionBank { get; set; }
        public virtual ICollection<Transaction> TransactionOtherPartyBank { get; set; }
    }
}
