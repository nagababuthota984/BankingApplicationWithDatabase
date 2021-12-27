namespace BankAppDbFirstApproach.Models
{
    public partial class BankViewModel
    {
        #region Properties
        public string BankId { get; set; }
        public string Bankname { get; set; }
        public string Branch { get; set; }
        public string Ifsc { get; set; }
        public decimal SelfRtgs { get; set; }
        public decimal SelfImps { get; set; }
        public decimal OtherRtgs { get; set; }
        public decimal OtherImps { get; set; }
        public decimal Balance { get; set; }
        public string DefaultCurrencyName { get; set; }


        #endregion
        public BankViewModel()
        {

        }
        public BankViewModel(string name, string branch, string ifsc)
        {
            this.Bankname = name;
            this.BankId = $"{name.Substring(0, 3)}{DateTime.Now:yyyyMMddhhmmss}";
            this.Branch = branch;
            this.Ifsc = ifsc;
            this.SelfRtgs = 0;
            this.SelfImps = 5;
            this.OtherRtgs = 2;
            this.OtherImps = 6;
            this.Balance = 0;
            this.DefaultCurrencyName = "INR";
        }
    }
}
