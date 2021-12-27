namespace BankAppDbFirstApproach.Data
{
    public partial class Currency
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal ExchangeRate { get; set; }
        public string BankId { get; set; }

        public virtual Bank Bank { get; set; }
    }
}
