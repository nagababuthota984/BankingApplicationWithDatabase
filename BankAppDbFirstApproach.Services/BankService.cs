using AutoMapper;
using BankAppDbFirstApproach.Data;
using BankAppDbFirstApproach.Models;

namespace BankAppDbFirstApproach.Services
{
    public class BankService : IBankService
    {
        private IAccountService accountService;
        private BankStorageContext dbContext;
        private IMapper mapper;
        public BankService(IAccountService accService, BankStorageContext context, IMapper mapperObject)
        {
            accountService = accService;
            dbContext = context;
            mapper = mapperObject;
        }
        public BankViewModel CreateAndGetBank(string name, string branch, string ifsc)
        {
            BankViewModel newBank = new BankViewModel(name, branch, ifsc);
            CurrencyViewModel currency = new CurrencyViewModel("INR", 1, newBank.BankId);
            CustomerViewModel customer = new CustomerViewModel("Admin", newBank.BankId);
            EmployeeViewModel employee = new EmployeeViewModel(newBank.BankId,"Admin", "admin", customer.CustomerId);
            dbContext.Bank.Add(mapper.Map<Bank>(newBank));
            dbContext.Customer.Add(mapper.Map<Customer>(customer));
            dbContext.Currency.Add(mapper.Map<Currency>(currency));
            dbContext.Employee.Add(mapper.Map<Employee>(employee));
            dbContext.SaveChanges();
            return newBank;
        }
        public EmployeeViewModel CreateAndGetEmployee(CustomerViewModel newCustomer, EmployeeDesignation role, BankViewModel bank)
        {
            EmployeeViewModel employee = new EmployeeViewModel(newCustomer, role, bank);
            dbContext.Customer.Add(mapper.Map<Customer>(newCustomer));
            dbContext.Employee.Add(mapper.Map<Employee>(employee));
            dbContext.SaveChanges();
            return employee;
        }
        public bool IsValidEmployee(string userName, string password)
        {
            Employee emp = dbContext.Employee.ToList().FirstOrDefault(e => e.Username.EqualInvariant(userName) && e.Password.EqualInvariant(password)); ;
            if (emp == null)
                return false;
            else
            {
                PrepareEmployeeSessionContext(mapper.Map<EmployeeViewModel>(emp));
                return true;
            }
        }
        private void PrepareEmployeeSessionContext(EmployeeViewModel emp)
        {
            SessionContext.Employee = emp;
            SessionContext.Bank = GetBankById(emp.BankId);
        }
        public void CreateAndAddAccount(AccountViewModel newAccount, CustomerViewModel newCustomer, BankViewModel bank)
        {
            newAccount.BankId = bank.BankId;
            newAccount.AccountNumber = GenerateAccountNumber();
            newAccount.CustomerId = newCustomer.CustomerId;
            dbContext.Account.Add(mapper.Map<Account>(newAccount));
            dbContext.Customer.Add(mapper.Map<Customer>(newCustomer));
            dbContext.SaveChanges();
        }
        public BankViewModel GetBankById(string bankId)
        {
            Bank bank = dbContext.Bank.ToList().FirstOrDefault(b => b.BankId.EqualInvariant(bankId));
            return mapper.Map<BankViewModel>(bank);
        }
        public List<TransactionViewModel> GetTransactionsByDate(DateTime date, string bankId)
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions.AddRange(dbContext.Transaction.Where(tr => tr.TransactionOn.Date == date && tr.BankId.EqualInvariant(bankId)));
            return mapper.Map<List<TransactionViewModel>>(transactions);
        }
        public List<TransactionViewModel> GetAccountTransactions(string accountId)
        {
            return mapper.Map<List<TransactionViewModel>>(dbContext.Transaction.Where(tr => tr.AccountId.Equals(accountId)).ToList());
        }
        public List<TransactionViewModel> GetTransactions(string bankId)
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions.AddRange(dbContext.Transaction.Where(tr=> (tr.IsBankTransaction??false) && tr.BankId.EqualInvariant(bankId)));
            return mapper.Map<List<TransactionViewModel>>(transactions);
        }
        public bool AddNewCurrency(BankViewModel bank, string newCurrencyName, decimal exchangeRate)
        {
            if (dbContext.Currency.Any(c => c.BankId.Equals(bank.BankId) && c.Name.Equals(newCurrencyName)))
            {
                return false;
            }
            dbContext.Currency.Add(mapper.Map<Currency>(new CurrencyViewModel(newCurrencyName, exchangeRate, bank.BankId)));
            dbContext.SaveChanges();
            return true;
        }
        public bool DeleteAccount(Account userAccount)
        {
            userAccount.Status = (int)AccountStatus.Closed;
            dbContext.SaveChanges();
            return true;
        }
        public bool ModifyServiceCharge(ModeOfTransferOptions mode, bool isSelfBankCharge, BankViewModel bank, decimal newValue)
        {
            bool isModified;
            if (isSelfBankCharge)
            {
                if (mode == ModeOfTransferOptions.RTGS)
                {
                    bank.SelfRtgs = newValue;
                }
                else
                {
                    bank.SelfImps = newValue;
                }
                isModified = true;
            }
            else
            {
                if (mode == ModeOfTransferOptions.RTGS)
                {
                    bank.OtherRtgs = newValue;
                }
                else
                {
                    bank.OtherImps = newValue;
                }
                isModified = true;
            }
            dbContext.SaveChanges();
            return isModified;
        }

        public bool RevertTransaction(TransactionViewModel transaction, BankViewModel bank)
        {
            AccountViewModel userAccount = accountService.GetAccountById(transaction.AccountId);
            if (transaction.TransactionType == (int)TransactionType.Credit)
            {
                accountService.WithdrawAmount(userAccount, transaction.TransactionAmount);
            }
            else if (transaction.TransactionType == (int)TransactionType.Debit)
            {
                accountService.DepositAmount(userAccount, transaction.TransactionAmount, mapper.Map<CurrencyViewModel>(dbContext.Currency.FirstOrDefault(c => c.Name.Equals(bank.DefaultCurrencyName))));
            }
            else if (transaction.TransactionType == (int)TransactionType.Transfer)
            {
                AccountViewModel receiverAccount = accountService.GetAccountById(transaction.AccountId);
                accountService.WithdrawAmount(receiverAccount, transaction.TransactionAmount);
                accountService.DepositAmount(userAccount, transaction.TransactionAmount, mapper.Map<CurrencyViewModel>(dbContext.Currency.FirstOrDefault(c => c.Name.Equals(bank.DefaultCurrencyName))));
            }
            dbContext.SaveChanges();
            return true;
        }
        private string GenerateAccountNumber()
        {
            List<AccountViewModel> accounts = accountService.GetAllAccounts();
            string accNumber;
            do
            {
                accNumber = GenerateRandomNumber(12);
            } while (accounts.Any(account => account.AccountNumber.Equals(accNumber)));
            return accNumber;
        }
        private static string GenerateRandomNumber(int length)
        {
            Random r = new Random();
            string accountNumber = "";
            for (int i = 1; i < length; i++)
            {
                accountNumber += r.Next(0, 9).ToString();
            }
            return accountNumber;
        }
    }
}






