using System;
using System.Collections.Generic;

namespace BankingApplication.Models
{
    public class Bank : BaseBank
    {
        public Bank()
        {

        }
        public Bank(string name, string branch, string ifsc)
        {
            BankName = name;
            BankId = $"{name.Substring(0, 3)}{DateTime.Now:yyyyMMddhhmmss}";
            Branch = branch;
            Ifsc = ifsc;
            SelfRTGS = 0;
            SelfIMPS = 5;
            OtherRTGS = 2;
            OtherIMPS = 6;
            Balance = 0;
            SupportedCurrency = new List<Currency> { new Currency("INR", 1) };
            DefaultCurrency = new Currency("INR", 1);
            Accounts = new List<Account>();
            Transactions = new List<Transaction>();
            Employees = new List<Employee>();
        }

        public List<Account> Accounts { get; set; }
        
        public decimal SelfRTGS { get; set; }
        public decimal SelfIMPS { get; set; }
        public decimal OtherRTGS { get; set; }
        public decimal OtherIMPS { get; set; }
        public decimal Balance { get; set; }
        public Currency DefaultCurrency { get; set; }
        public List<Transaction> Transactions  { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Currency> SupportedCurrency { get; set; }
    }
}
