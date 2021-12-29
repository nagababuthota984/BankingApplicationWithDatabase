using BankAppDbFirstApproach.Models;
using BankAppDbFirstApproach.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace BankAppDbFirstApproach.Services
{
    public class AccountService : IAccountService
    {
        private ITransactionService transService;
        private BankStorageContext dbContext;
        private IMapper mapper;
        public AccountService(ITransactionService transactionService, BankStorageContext context, IMapper mapperObject)
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
                InitializeSessionContext(mapper.Map<AccountViewModel>(acc));
                return true;
            }
        }
        private void InitializeSessionContext(AccountViewModel acc)
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
            var acc = mapper.Map<Account>(userAccount);
            acc.Balance += amount;
            dbContext.Account.Update(acc);
            //...For now creating the tranction here in the future will mov...
            transService.CreateTransaction(userAccount, TransactionType.Credit, amount, currency.Name);
            dbContext.SaveChanges();
        }
        public void WithdrawAmount(AccountViewModel userAccount, decimal amount)
        {
            userAccount.Balance -= amount;
            dbContext.Account.Update(mapper.Map<Account>(userAccount));
            transService.CreateTransaction(userAccount, TransactionType.Debit, amount, SessionContext.Bank.DefaultCurrencyName);
            dbContext.SaveChanges();
        }
        public void TransferAmount(AccountViewModel senderAccount, BankViewModel senderBank, AccountViewModel receiverAccount, decimal amount, ModeOfTransferOptions mode)
        {

            senderAccount.Balance -= amount;
            receiverAccount.Balance += amount;
            dbContext.Account.Update(mapper.Map<Account>(senderAccount));                        
            dbContext.Account.Update(mapper.Map<Account>(receiverAccount));
            ApplyTransferCharges(senderAccount, senderBank, receiverAccount.BankId, amount, mode, SessionContext.Bank.DefaultCurrencyName);
            transService.CreateTransferTransaction(senderAccount, receiverAccount, amount, mode, SessionContext.Bank.DefaultCurrencyName);
            dbContext.SaveChanges();
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
            dbContext.Account.Update(mapper.Map<Account>(senderAccount));
            dbContext.Bank.Update(mapper.Map<Bank>(senderBank));
            transService.CreateAndAddBankTransaction(senderBank, senderAccount, charges, currencyName);
            dbContext.SaveChanges();
        }

        public List<AccountViewModel> GetAllAccounts()
        {
            return mapper.Map<List<AccountViewModel>>(dbContext.Account);
        }
        public void UpdateAccount(CustomerViewModel customer)
        {
            Customer updatedCustomer = mapper.Map<Customer>(customer);
            dbContext.Customer.Update(updatedCustomer);

        }
    }
}

