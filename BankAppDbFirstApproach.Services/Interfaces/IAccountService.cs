using BankAppDbFirstApproach.Data;
using BankAppDbFirstApproach.Models;

namespace BankAppDbFirstApproach.Services
{
    public interface IAccountService
    {
        bool IsValidCustomer(string userName, string password);
        AccountViewModel GetAccountByAccNumber(string accNumber);
        AccountViewModel GetAccountById(string accountId);
        void DepositAmount(AccountViewModel userAccount, decimal amount, CurrencyViewModel currency);
        void WithdrawAmount(AccountViewModel userAccount, decimal amount);
        void TransferAmount(AccountViewModel senderAccount, BankViewModel senderBank, AccountViewModel receiverAccount, decimal amount, ModeOfTransferOptions mode);
        void ApplyTransferCharges(AccountViewModel senderAccount, BankViewModel senderBank, string receiverBankId, decimal amount, ModeOfTransferOptions mode, string currencyName);
        List<AccountViewModel> GetAllAccounts();
        void UpdateAccount(CustomerViewModel customer);
    }
}