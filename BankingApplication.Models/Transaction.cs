using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Models
{
    public class Transaction
    {
        public string TransId { get; set; }
        public TransactionType Type { get; set; }
        public string SenderAccountId { get; set; }
        public string ReceiverAccountId { get; set; }
        public string SenderBankId { get; set; }
        public string ReceiverBankId { get; set; }
        public DateTime On { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public Currency Currency { get; set; }
        public ModeOfTransfer TransferMode { get; set; }
        public Transaction()
        {

        }
        public Transaction(Account senderAccount,Account receiverAccount,TransactionType transtype, decimal transactionamount, Currency currency,ModeOfTransfer mode=ModeOfTransfer.None)
        {
            DateTime timestamp = DateTime.Now; 
            this.TransId = $"TXN{senderAccount.BankId}{senderAccount.AccountId}{timestamp:yyyyMMddhhmmss}";
            this.SenderAccountId = senderAccount.AccountId;
            this.ReceiverAccountId = receiverAccount.AccountId;
            this.Type = transtype;
            this.On = timestamp;
            this.SenderBankId = senderAccount.BankId;
            this.ReceiverBankId = receiverAccount.BankId;
            this.TransactionAmount = transactionamount;
            this.Currency = currency;
            this.TransferMode = mode;
            this.BalanceAmount = senderAccount.Balance;
        }
        public Transaction(Account userAccount, Bank bank, TransactionType serviceCharge, decimal charges, Currency currency)
            : this(userAccount,userAccount,serviceCharge,charges,currency)
        {
            //..creates a bank transaction
            this.ReceiverAccountId = bank.BankId;
            this.BalanceAmount = bank.Balance;
        }

       


        
    }
    
}
