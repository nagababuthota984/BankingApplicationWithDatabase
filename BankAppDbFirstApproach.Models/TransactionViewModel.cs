namespace BankAppDbFirstApproach.Models
{
    public partial class TransactionViewModel
    {
        public string TransId { get; set; }
        public string AccountId { get; set; }
        public string Sendername { get; set; }
        public string Receivername { get; set; }
        public DateTime TransactionOn { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal Balance { get; set; }
        public int ModeOfTransfer { get; set; }
        public string Currency { get; set; }
        public int TransactionType { get; set; }
        public string BankId { get; set; }
        public string OtherPartyBankId { get; set; }
        public bool? IsBankTransaction { get; set; }

        
        public TransactionViewModel()
        {

        }

        //Normal Debit,Credit transaction
        public TransactionViewModel(AccountViewModel userAccount, TransactionType type, decimal transactionAmount, string currencyName, bool isServiceCharge)
        {
            DateTime timestamp = DateTime.Now;
            this.TransId = $"TXN{userAccount.BankId}{userAccount.AccountId}{timestamp:yyMMddhhmmssfff}";
            this.AccountId = userAccount.AccountId;
            this.Sendername = userAccount.AccountId;
            this.Receivername = isServiceCharge ? userAccount.BankId : userAccount.AccountId;
            this.TransactionAmount = transactionAmount;
            this.TransactionType = (int)type;
            this.Balance = userAccount.Balance;
            this.ModeOfTransfer = (int)ModeOfTransferOptions.None;
            this.TransactionOn = timestamp;
            this.Currency = currencyName;
            this.BankId = userAccount.BankId;
            this.OtherPartyBankId = this.BankId;
            this.IsBankTransaction = false;
        }

        //transaction for Bank related charges 
        public TransactionViewModel(AccountViewModel userAccount, BankViewModel bank, TransactionType serviceCharge, decimal charges, string currencyName)
        {
            DateTime timestamp = DateTime.Now;
            this.TransId = $"TXN{userAccount.BankId}{userAccount.AccountId}{timestamp:yyMMddhhmmssfff}";
            this.AccountId = userAccount.AccountId;
            this.Sendername = userAccount.AccountId;
            this.Receivername = bank.BankId;
            this.TransactionAmount = charges;
            this.TransactionType = (int)serviceCharge;
            this.Balance = bank.Balance;
            this.TransactionOn = timestamp;
            this.Currency = currencyName;
            this.BankId = bank.BankId;
            this.ModeOfTransfer = (int)ModeOfTransferOptions.None;
            this.OtherPartyBankId = bank.BankId;
            this.IsBankTransaction = true;

        }
        //transaction generator for one account to other account transfers.
        public TransactionViewModel(AccountViewModel userAccount, AccountViewModel receiverAccount, TransactionType transfer, decimal transactionAmount, string currencyName, ModeOfTransferOptions mode)
        {
            DateTime timestamp = DateTime.Now;
            this.TransId = $"TXN{userAccount.BankId}{userAccount.AccountId}{timestamp:yyMMddhhmmssfffff}";
            this.AccountId = userAccount.AccountId;
            this.Sendername = userAccount.AccountId;
            this.Receivername = receiverAccount.AccountId;
            this.TransactionAmount = transactionAmount;
            this.TransactionType = TransactionType;
            this.TransactionOn = timestamp;
            this.ModeOfTransfer = (int)mode;
            this.Balance = userAccount.Balance;
            this.Currency = currencyName;
            this.BankId = userAccount.BankId;
            this.OtherPartyBankId = receiverAccount.BankId;
            this.IsBankTransaction = false;
        }

    }
}
