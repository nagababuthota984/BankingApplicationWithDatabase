using System;
using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Models
{
    public class Employee
    {
        #region Properties
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime Dob { get; set; }
        public string BankId { get; set; }
        public int Age { get; set; }
        [Key]
        public string EmployeeId { get; set; }
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
            this.Name = "Admin";
            this.Age = 0;
            this.Dob = DateTime.Now;
            this.Gender = Gender.PreferNotToSay;

        }
        public Employee(string name, int age, DateTime dob, Gender gender, EmployeeDesignation role, Bank bank)
        {
            this.Name = name;
            this.Age = age;
            this.Dob = dob;
            this.Gender = gender;
            this.BankId = bank.BankId;
            this.Designation = role;
            this.EmployeeId = $"{bank.BankName.Substring(0, 3)}{DateTime.Now:yyyyMMddhhmm}";
            this.UserName = $"{Name.Substring(0, 3)}{EmployeeId.Substring(5, 3)}";
            this.Password = dob.ToString("yyyyMMdd");

        }

    }
}