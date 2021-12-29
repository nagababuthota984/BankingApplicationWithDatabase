using BankAppDbFirstApproach.Data;
using BankAppDbFirstApproach.Models;

namespace BankAppDbFirstApproach.Services
{
    public interface IBankService
    {
        BankViewModel CreateAndGetBank(string name, string branch, string ifsc);
        bool IsValidEmployee(string userName, string password);
        AccountViewModel CreateAndAddAccount(AccountViewModel newAccount, CustomerViewModel customer, BankViewModel bank);
        bool DeleteAccount(Account userAccount);
        bool AddNewCurrency(BankViewModel bank, string newName, decimal exchangeRate);
        bool ModifyServiceCharge(ModeOfTransferOptions mode, bool isSelfBankCharge, BankViewModel bank, decimal newValue);
        List<TransactionViewModel> GetAccountTransactions(string accountId);
        bool RevertTransaction(TransactionViewModel transaction, BankViewModel bank);
        EmployeeViewModel CreateAndGetEmployee(CustomerViewModel customer, EmployeeDesignation role, BankViewModel bank);
        List<TransactionViewModel> GetTransactionsByDate(DateTime date, string bankId);
        List<TransactionViewModel> GetTransactions(string bankId);
        BankViewModel GetBankById(string bankid);
        CurrencyViewModel GetCurrencyByName(string currencyName);
    }
}