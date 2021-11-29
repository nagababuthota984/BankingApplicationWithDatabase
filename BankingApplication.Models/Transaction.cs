using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankingApplication.Models
{
    public class Transaction
    {
        #region Properties
        [Key]
        public string TransId { get; set; }
        public TransactionType Type { get; set; }
        public string SenderAccountId { get; set; }
        public string ReceiverAccountId { get; set; }
        public string SenderBankId { get; set; }
        public string ReceiverBankId { get; set; }
        public DateTime On { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Currency { get; set; }
        public ModeOfTransfer TransferMode { get; set; }
        #endregion

        public Transaction()
        {

        }
        public Transaction(Account senderAccount, Account receiverAccount, TransactionType transtype, decimal transactionamount, string currencyName, ModeOfTransfer mode = ModeOfTransfer.None)
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
            this.Currency = currencyName;
            this.TransferMode = mode;
            this.BalanceAmount = senderAccount.Balance;
        }
        public Transaction(Account userAccount, Bank bank, TransactionType serviceCharge, decimal charges, string currencyName)
            : this(userAccount, userAccount, serviceCharge, charges, currencyName)
        {
            //..creates a bank transaction
            this.ReceiverAccountId = bank.BankId;
            this.BalanceAmount = bank.Balance;
        }
    }

}