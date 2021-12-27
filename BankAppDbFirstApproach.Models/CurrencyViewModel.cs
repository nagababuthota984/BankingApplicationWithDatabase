namespace BankAppDbFirstApproach.Models
{
    public partial class CurrencyViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal ExchangeRate { get; set; }
        public string BankId { get; set; }

        public CurrencyViewModel()
        {

        }
        public CurrencyViewModel(string name, decimal exchangeRate, string bankId)
        {
            this.Id = bankId + name;
            this.Name = name;
            this.ExchangeRate = exchangeRate;
            this.BankId = bankId;
        }
    }
}
