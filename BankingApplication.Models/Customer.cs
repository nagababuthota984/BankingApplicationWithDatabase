using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankingApplication.Models
{
    public class Customer
    {
        #region Properties
        [Key]
        public string CustomerId { get; set; }
        public string AccountId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string ContactNumber { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public long AadharNumber { get; set; }
        public string PanNumber { get; set; }
        #endregion
        public Customer()
        {

        }
        public Customer(string name, int age, Gender gender, DateTime dob, string contactNumber, long aadharNumber, string panNumber, string address)
        {
            this.Name = name;
            this.Age = age;
            this.Gender = gender;
            this.Dob = dob;
            this.Address = address;
            this.AadharNumber = aadharNumber;
            this.ContactNumber = contactNumber;
            this.PanNumber = panNumber;
            
        }

    }
}