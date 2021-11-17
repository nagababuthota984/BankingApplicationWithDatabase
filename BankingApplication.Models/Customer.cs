using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Models
{
    public class Customer
    {
        public string AccountId { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public Gender Gender { get; set; }
        public string ContactNumber { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string AadharNumber { get; set; }
        public string PanNumber { get; set; }
        public Customer(string name,string age,Gender gender,DateTime dob,string aadharNumber,string contactNumber,string panNumber, string address)
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
