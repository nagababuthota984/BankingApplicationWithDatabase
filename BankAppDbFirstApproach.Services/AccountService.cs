using BankAppDbFirstApproach.Models;


namespace BankAppDbFirstApproach.Services
{
    public class AccountService : IAccountService
    {
        private ITransactionService transService;
        private BankStorageEntities dbContext;
        public AccountService(ITransactionService transactionService,BankStorageEntities context)
        {
            transService = transactionService;
            dbContext = context;
        }
        public bool IsValidCustomer(string userName, string password)
        {
            Account acc = dbContext.Accounts.ToList().FirstOrDefault(acc => acc.username.EqualInvariant(userName) && acc.password.EqualInvariant(password) && acc.status == (int)AccountStatus.Active);
            if (acc == null)
                return false;
            else
            {
                PrepareCustomerSessionContext(acc);
                return true;
            }
        }
        private void PrepareCustomerSessionContext(Account acc)
        {
            SessionContext.Account = acc;
            SessionContext.Bank = dbContext.Banks.ToList().FirstOrDefault(b=>b.bankId.EqualInvariant(acc.bankId)); 
        }
        public Account GetAccountByAccNumber(string accNumber)
        {
            return dbContext.Accounts.ToList().FirstOrDefault(ac=>ac.accountNumber.EqualInvariant(accNumber));
        }
        public Account GetAccountById(string accountId)
        {
            return dbContext.Accounts.ToList().FirstOrDefault(ac=>ac.accountId.EqualInvariant(accountId));
        }
        public void DepositAmount(Account userAccount, decimal amount, Currency currency)
        {
            amount *= currency.exchangeRate;
            userAccount.balance += amount;
            transService.CreateTransaction(userAccount, TransactionType.Credit, amount, currency.name);
            dbContext.SaveChanges();
        }
        public void WithdrawAmount(Account userAccount, decimal amount)
        {
            userAccount.balance -= amount;
            transService.CreateTransaction(userAccount, TransactionType.Debit, amount, SessionContext.Bank.defaultCurrencyName);
            dbContext.SaveChanges();
        }
        public void TransferAmount(Account senderAccount, Bank senderBank, Account receiverAccount, decimal amount, ModeOfTransfer mode)
        {
            try
            {
                senderAccount.balance -= amount;
                receiverAccount.balance += amount;
                ApplyTransferCharges(senderAccount, senderBank, receiverAccount.bankId, amount, mode, SessionContext.Bank.defaultCurrencyName);
                transService.CreateTransferTransaction(senderAccount, receiverAccount, amount, mode, SessionContext.Bank.defaultCurrencyName);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message+e.InnerException.StackTrace);
            }
        }
        public void ApplyTransferCharges(Account senderAccount, Bank senderBank, string receiverBankId, decimal amount, ModeOfTransfer mode, string currencyName)
        {
            decimal charges = 0;
            if (mode == ModeOfTransfer.RTGS)
            {
                charges = senderAccount.bankId.Equals(receiverBankId)? (senderBank.selfRTGS * amount) / 100 : (senderBank.otherRTGS * amount) / 100;
            }
            else
            {
                charges = senderAccount.bankId.Equals(receiverBankId) ? (senderBank.selfIMPS * amount) / 100 : (senderBank.otherIMPS * amount) / 100;
            }
            senderAccount.balance -= charges;
            senderBank.balance += charges;
            transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
            dbContext.SaveChanges();
        }
    

    }
}

