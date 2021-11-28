using System;
namespace BankingApplication.Models
{
    public class Employee
    {
        public Employee()
        {

        }
        public Employee(string name, int age, DateTime dob, Gender gender, EmployeeDesignation role, Bank bank)
        {
            this.Name = name;
            this.Age = age;
            this.Dob = dob;
            this.Gender = gender;
            this.BankId = bank.BankId;
            this.Designation = role;
            this.EmployeeId = $"{bank.BankName,3}{Name,3}{dob:MMdd}";
            this.UserName = $"{Name.Substring(0, 3)}{EmployeeId.Substring(5, 3)}";
            this.Password = dob.ToString("yyyyMMdd");

        }

        public string Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime Dob { get; set; }
        public string BankId { get; set; }
        public int Age { get; set; }
        public string EmployeeId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public EmployeeDesignation Designation { get; set; }
    }
}
