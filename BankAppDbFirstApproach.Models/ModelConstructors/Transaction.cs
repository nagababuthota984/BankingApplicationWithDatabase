using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppDbFirstApproach.Models
{
    public partial class Transaction
    {
        public Transaction()
        {

        }

        //Normal Debit,Credit transaction
        public Transaction(Account userAccount, TransactionType type, decimal transactionAmount, string currencyName, bool isServiceCharge)
        {
            DateTime timestamp = DateTime.Now;
            this.transId = $"TXN{userAccount.bankId}{userAccount.accountId}{timestamp:yyyyMMddhhmmss}";
            this.accountId = userAccount.accountId;
            this.sendername = userAccount.accountId;
            this.receivername = isServiceCharge ? userAccount.bankId : userAccount.accountId;
            this.transactionAmount = transactionAmount;
            this.transactionType = (int)type;
            this.balance = userAccount.balance;
            this.modeOfTransfer = (int)ModeOfTransfer.None;
            this.transactionOn = timestamp;
            this.currency = currencyName;
            this.bankId = userAccount.bankId;
            this.otherPartyBankId = this.bankId;
            this.isBankTransaction = false;
        }

        //transaction for Bank related charges 
        public Transaction(Account userAccount, Bank bank, TransactionType serviceCharge, decimal charges, string currencyName)
        {
            DateTime timestamp = DateTime.Now;
            this.transId = $"TXN{userAccount.bankId}{userAccount.accountId}{timestamp:yyyyMMddhhmmss}";
            this.accountId = userAccount.accountId;
            this.sendername = userAccount.accountId;
            this.receivername = bank.bankId;
            this.transactionAmount = charges;
            this.transactionType = (int)serviceCharge;
            this.balance = bank.balance;
            this.transactionOn = timestamp;
            this.currency = currencyName;
            this.bankId = bank.bankId;
            this.modeOfTransfer = (int)ModeOfTransfer.None;
            this.otherPartyBankId = bank.bankId;
            this.isBankTransaction = true;

        }
        //transaction generator for one account to other account transfers.
        public Transaction(Account userAccount, Account receiverAccount, TransactionType transfer, decimal transactionAmount, string currencyName, ModeOfTransfer mode)
        {
            DateTime timestamp = DateTime.Now;
            this.transId = $"TXN{userAccount.bankId}{userAccount.accountId}{timestamp:yyyyMMddhhmmss}";
            this.accountId = userAccount.accountId;
            this.sendername = userAccount.accountId;
            this.receivername = receiverAccount.accountId;
            this.transactionAmount = transactionAmount;
            this.transactionType = transactionType;
            this.transactionOn = timestamp;
            this.modeOfTransfer = (int)mode;
            this.balance = userAccount.balance;
            this.currency = currencyName;
            this.bankId = userAccount.bankId;
            this.otherPartyBankId = receiverAccount.bankId;
            this.isBankTransaction = false;
        }

    }
}
