using System;
using System.Collections.Generic;

namespace BankAppDbFirstApproach.Data
{
    public partial class Customer
    {
        public Customer()
        {
            Account = new HashSet<Account>();
            Employee = new HashSet<Employee>();
        }

        public string CustomerId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int GenderOptions { get; set; }
        public string ContactNumber { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public long AadharNumber { get; set; }
        public string PanNumber { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
    }
}
