using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Models
{
    public class Employee
    {
        #region Properties
        public Customer customer { get; set; }
        public string customerId { get; set; }
        [Key]
        public string EmployeeId { get; set; }
        public string BankId { get; set; } 
        public string UserName { get; set; }
        public string Password { get; set; }
        public EmployeeDesignation Designation { get; set; }
        #endregion

        public Employee()
        {

        }
        public Employee(string bankId,string bankName)
        {
            this.EmployeeId = $"{bankName.Substring(0, 3)}{DateTime.Now:yyyyMMddhhmm}";
            this.UserName = bankId;
            this.Password = bankId;
            this.BankId = bankId;
            this.customer.Name = "Admin";
            this.customer.Age = 0;
            this.customer.Dob = DateTime.Now;
            this.customer.Gender = Gender.PreferNotToSay;

        }
        public Employee(Customer newCustomer, EmployeeDesignation role, Bank bank)
        {
            this.customer = newCustomer;
            this.BankId = bank.BankId;
            this.Designation = role;
            this.EmployeeId = $"{bank.BankName.Substring(0, 3)}{DateTime.Now:yyyyMMddhhmm}";
            this.UserName = $"{customer.Name.Substring(0, 3)}{EmployeeId.Substring(5, 3)}";
            this.Password = customer.Dob.ToString("yyyyMMdd");
            this.customerId = customer.CustomerId;
        }

    }
}