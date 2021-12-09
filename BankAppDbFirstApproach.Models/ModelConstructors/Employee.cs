using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDbFirstApproach.Models
{
    public partial class Employee
    {
        public Employee()
        {

        }

        public Employee(Customer newCustomer, EmployeeDesignation role, Bank bank)
        {
            this.bankId = bank.bankId;
            this.designation = (int)role;
            this.employeeId = $"{bank.bankname.Substring(0, 3)}{DateTime.Now:yyyyMMddhhmm}";
            this.username = $"{newCustomer.name.Substring(0, 3)}{this.employeeId.Substring(5, 3)}";
            this.password = newCustomer.dob.ToString("yyyyMMdd");
            this.customerId = newCustomer.customerId;
        }

        public Employee(string bankId, string username, string password, string customerId)
        {
            this.bankId = bankId;
            this.username = username;
            this.password = password;
            this.customerId = customerId;
        }
    }
}
