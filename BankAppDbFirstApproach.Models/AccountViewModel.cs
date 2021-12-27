namespace BankAppDbFirstApproach.Models
{
    public partial class AccountViewModel
    {
        #region Properties
        public string AccountId { get; set; }
        public string BankId { get; set; }
        public string AccountNumber { get; set; }
        public int AccountType { get; set; }
        public decimal Balance { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public string CustomerId { get; set; }
       

        #endregion
        public AccountViewModel(CustomerViewModel customer, AccountType type, BankViewModel bank)
        {
            string nameTrimmed = String.Concat(customer.Name.Where(c => !Char.IsWhiteSpace(c)));
            this.Username = $"{nameTrimmed.Substring(0, 4)}{customer.Dob:yyyy}{DateTime.Now:ffff}";
            this.Password = $"{customer.Dob:yyyyMMdd}";
            this.AccountId = $"{customer.Name.Substring(0, 3)}{customer.Dob:yyyyMMdd}";
            this.AccountType = (int)type;
            this.BankId = bank.BankId;
            this.CustomerId = customer.CustomerId;  
            this.AccountNumber = "";
        }

        public AccountViewModel()
        {
        }

        
    }
}
