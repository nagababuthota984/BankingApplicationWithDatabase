
using Newtonsoft.Json;

namespace BankAppDbFirstApproach.Models
{
    public partial class CustomerViewModel
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public string ContactNumber { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public long AadharNumber { get; set; }
        public string PanNumber { get; set; }
        [JsonConstructor]
        public CustomerViewModel(string name, string bankId)
        {
            this.CustomerId = name + bankId;
            this.Name = name;
            this.Age = 0;
            this.Gender = (int)GenderOptions.PreferNotToSay;
            this.Dob = DateTime.Now;
            this.Address = "Same as bank";
            this.AadharNumber = 0;
            this.ContactNumber = "0";
            this.PanNumber = "DoesNotExist";

        }

        public CustomerViewModel(string name, int age, GenderOptions gender, DateTime dob, string contactNumber, long aadharNumber, string panNumber, string address)
        {
            this.CustomerId = name.Substring(0, 3) + age.ToString() + panNumber.Substring(0, 3);
            this.Name = name;
            this.Age = age;
            this.Gender = (int)gender;
            this.Dob = dob;
            this.Address = address;
            this.AadharNumber = aadharNumber;
            this.ContactNumber = contactNumber;
            this.PanNumber = panNumber;

        }
    }
}
