using AutoMapper;
using BankAppDbFirstApproach.Data;
using BankAppDbFirstApproach.Models;

namespace BankAppDbFirstApproach.Services
{
    public class TransactionService : ITransactionService
    {
        private BankStorageContext dbContext;
        private IMapper _mapper;
        public TransactionService(BankStorageContext context,IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }
        public void CreateTransaction(AccountViewModel userAccount, TransactionType transtype, decimal transactionamount, string currencyName)
        {
            TransactionViewModel newTransaction = new TransactionViewModel(userAccount, transtype, transactionamount, currencyName, false);
            dbContext.Transaction.Add(_mapper.Map<Transaction>(newTransaction));
            dbContext.SaveChanges();
        }
        public void CreateTransferTransaction(AccountViewModel userAccount, AccountViewModel receiverAccount, decimal transactionAmount, ModeOfTransferOptions mode, string currencyName)
        {
            TransactionViewModel senderTransaction = new TransactionViewModel(userAccount, receiverAccount, TransactionType.Transfer, transactionAmount, currencyName, mode);
            dbContext.Transaction.Add(_mapper.Map<Transaction>(senderTransaction));
            TransactionViewModel receiverTransaction = new TransactionViewModel(userAccount, receiverAccount, TransactionType.Transfer, transactionAmount, currencyName, mode);
            receiverTransaction.AccountId = receiverAccount.AccountId;
            receiverTransaction.Balance = receiverAccount.Balance;
            dbContext.Transaction.Add(_mapper.Map<Transaction>(receiverTransaction));
            dbContext.SaveChanges();
        }
        public void CreateAndAddBankTransaction(BankViewModel bank, AccountViewModel userAccount, decimal charges, string currencyName)
        {
            TransactionViewModel userTransaction = new TransactionViewModel(userAccount, TransactionType.Debit, charges, currencyName, true);
            TransactionViewModel newBankTransaction = new TransactionViewModel(userAccount, bank, TransactionType.ServiceCharge, charges, currencyName);
            _mapper.Map<Transaction>(userTransaction);
            dbContext.Transaction.Add(_mapper.Map<Transaction>(newBankTransaction));
            dbContext.Transaction.Add(_mapper.Map<Transaction>(userTransaction));
            dbContext.SaveChanges();
        }
        public TransactionViewModel GetTransactionById(string transactionId)
        {
            return _mapper.Map<TransactionViewModel>(dbContext.Transaction.FirstOrDefault(tr => tr.TransId.Equals(transactionId)));
        }
    }
}