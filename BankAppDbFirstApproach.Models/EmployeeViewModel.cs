namespace BankAppDbFirstApproach.Models
{
    public partial class EmployeeViewModel
    {
        public string EmployeeId { get; set; }
        public string BankId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Designation { get; set; }
        public string CustomerId { get; set; }

        public EmployeeViewModel()
        {

        }

        public EmployeeViewModel(CustomerViewModel newCustomer, EmployeeDesignation role, BankViewModel bank)
        {
            this.BankId = bank.BankId;
            this.Designation = (int)role;
            this.EmployeeId = $"{bank.BankId.Substring(0, 3)}{DateTime.Now:yyyyMMddhhmm}";
            this.Username = $"{newCustomer.Name.Substring(0, 3)}{this.EmployeeId.Substring(5, 3)}";
            this.Password = newCustomer.Dob.ToString("yyyyMMdd");
            this.CustomerId = newCustomer.CustomerId;
        }

        public EmployeeViewModel(string bankId, string username, string password, string customerId)
        {
            this.BankId = bankId;
            this.Username = $"{username}{DateTime.Now:ssfff}";
            this.Password = password;
            this.CustomerId = customerId;
            this.Designation = (int)EmployeeDesignation.AccountsManager;
            this.EmployeeId = $"{bankId.Substring(0, 3)}{DateTime.Now:yyyyMMddhhmm}";
        }
    }
}
