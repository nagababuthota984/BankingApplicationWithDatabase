//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BankAppDbFirstApproach.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        public Customer()
        {
            this.Employees = new HashSet<Employee>();
            this.Accounts = new HashSet<Account>();
        }
    
        public string customerId { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public int gender { get; set; }
        public string contactNumber { get; set; }
        public System.DateTime dob { get; set; }
        public string address { get; set; }
        public long aadharNumber { get; set; }
        public string panNumber { get; set; }
    
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}