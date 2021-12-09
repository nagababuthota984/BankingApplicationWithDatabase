using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDbFirstApproach.Models
{
    public partial class Customer
    {
        public Customer(string name, string bankId)
        {
            this.customerId = name + bankId;
            this.name = name;
            this.age = 0;
            this.gender = (int)Gender.PreferNotToSay;
            this.dob = DateTime.Now;
            this.address = "Same as bank";
            this.aadharNumber = 0;
            this.contactNumber = "0";
            this.panNumber = "DoesNotExist";

        }

        public Customer(string name, int age, Gender gender, DateTime dob, string contactNumber, long aadharNumber, string panNumber, string address)
        {
            this.customerId = name.Substring(0, 3) + age.ToString() + panNumber.Substring(0, 3);
            this.name = name;
            this.age = age;
            this.gender = (int)gender;
            this.dob = dob;
            this.address = address;
            this.aadharNumber = aadharNumber;
            this.contactNumber = contactNumber;
            this.panNumber = panNumber;

        }
    }
}
