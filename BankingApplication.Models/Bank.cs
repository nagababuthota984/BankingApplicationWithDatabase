

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Models
{
    public class Bank
    {
        #region Properties
        [Key]
        public string BankId { get; set; } 
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string Ifsc { get; set; }
        public List<Account> Accounts { get; set; }

        public decimal SelfRTGS { get; set; }
        public decimal SelfIMPS { get; set; }
        public decimal OtherRTGS { get; set; }
        public decimal OtherIMPS { get; set; }
        public decimal Balance { get; set; }
        public string DefaultCurrencyName { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Currency> SupportedCurrency { get; set; }
        #endregion Properties
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
            SupportedCurrency = new List<Currency> { new Currency("INR", 1,BankId) };
            DefaultCurrencyName = "INR";
            Accounts = new List<Account>();
            Transactions = new List<Transaction>();
            Employees = new List<Employee> { new Employee(BankId,BankName) };          //default employee
        }
    }
}