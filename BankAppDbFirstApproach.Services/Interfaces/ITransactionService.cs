using BankAppDbFirstApproach.Models;

namespace BankAppDbFirstApproach.Services
{
    public interface ITransactionService
    {
        void CreateTransaction(AccountViewModel userAccount, TransactionType transtype, decimal transactionamount, string currencyName);
        void CreateTransferTransaction(AccountViewModel userAccount, AccountViewModel receiverAccount, decimal transactionAmount, ModeOfTransferOptions mode, string currencyName);
        void CreateAndAddBankTransaction(BankViewModel bank, AccountViewModel userAccount, decimal charges, string currencyName);
        TransactionViewModel GetTransactionById(string transactionId);


    }
}