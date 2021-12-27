using BankAppDbFirstApproach.Models;
using BankAppDbFirstApproach.Data;
using AutoMapper;

namespace BankAppDbFirstApproach.Services
{
    public class AccountService : IAccountService
    {
        private ITransactionService transService;
        private BankStorageContext dbContext;
        private IMapper mapper;
        public AccountService(ITransactionService transactionService, BankStorageContext context,IMapper mapperObject)
        {
            transService = transactionService;
            dbContext = context;
            mapper = mapperObject;
        }
        public bool IsValidCustomer(string userName, string password)
        {
            var acc = dbContext.Account.ToList().FirstOrDefault(acc => acc.Username.EqualInvariant(userName) && acc.Password.EqualInvariant(password) && acc.Status == (int)AccountStatus.Active);
            if (acc == null)
                return false;
            else
            {
                PrepareCustomerSessionContext(mapper.Map<AccountViewModel>(acc));
                return true;
            }
        }
        private void PrepareCustomerSessionContext(AccountViewModel acc)
        {
            SessionContext.Account = acc;
            SessionContext.Bank = mapper.Map<BankViewModel>(dbContext.Bank.ToList().FirstOrDefault(b => b.BankId.EqualInvariant(acc.BankId)));
        }
        public AccountViewModel GetAccountByAccNumber(string accNumber)
        {
            return mapper.Map<AccountViewModel>(dbContext.Account.ToList().FirstOrDefault(ac => ac.AccountNumber.EqualInvariant(accNumber)));
        }
        public AccountViewModel GetAccountById(string accountId)
        {
            return mapper.Map<AccountViewModel>(dbContext.Account.ToList().FirstOrDefault(ac => ac.AccountId.EqualInvariant(accountId)));
        }
        public void DepositAmount(AccountViewModel userAccount, decimal amount, CurrencyViewModel currency)
        {
            amount *= currency.ExchangeRate;
            userAccount.Balance += amount;
            transService.CreateTransaction(userAccount, TransactionType.Credit, amount, currency.Name);
            dbContext.SaveChanges();
        }
        public void WithdrawAmount(AccountViewModel userAccount, decimal amount)
        {
            userAccount.Balance -= amount;
            transService.CreateTransaction(userAccount, TransactionType.Debit, amount, SessionContext.Bank.DefaultCurrencyName);
            dbContext.SaveChanges();
        }
        public void TransferAmount(AccountViewModel senderAccount, BankViewModel senderBank, AccountViewModel receiverAccount, decimal amount, ModeOfTransferOptions mode)
        {
            try
            {
                senderAccount.Balance -= amount;
                receiverAccount.Balance += amount;
                ApplyTransferCharges(senderAccount, senderBank, receiverAccount.BankId, amount, mode, SessionContext.Bank.DefaultCurrencyName);
                transService.CreateTransferTransaction(senderAccount, receiverAccount, amount, mode, SessionContext.Bank.DefaultCurrencyName);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message + e.InnerException.StackTrace);
            }
        }
        public void ApplyTransferCharges(Models.AccountViewModel senderAccount, Models.BankViewModel senderBank, string receiverBankId, decimal amount, ModeOfTransferOptions mode, string currencyName)
        {
            decimal charges = 0;
            if (mode == ModeOfTransferOptions.RTGS)
            {
                charges = senderAccount.BankId.Equals(receiverBankId) ? (senderBank.SelfRtgs * amount) / 100 : (senderBank.OtherRtgs * amount) / 100;
            }
            else
            {
                charges = senderAccount.BankId.Equals(receiverBankId) ? (senderBank.SelfImps * amount) / 100 : (senderBank.OtherImps * amount) / 100;
            }
            senderAccount.Balance -= charges;
            senderBank.Balance += charges;
            transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
            dbContext.SaveChanges();
        }

        public List<AccountViewModel> GetAllAccounts()
        {
            return mapper.Map<List<AccountViewModel>>(dbContext.Account);
        }
       
    }
}

