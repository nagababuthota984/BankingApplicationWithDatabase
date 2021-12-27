using AutoMapper;
using BankAppDbFirstApproach.Models;
using BankAppDbFirstApproach.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankAppDbFirstApproach.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IBankService bankService;
        private readonly IAccountService accountService;
        private readonly IMapper mapper;
        private readonly ITransactionService transactionService;

        public TransactionsController(IBankService bnkService, IAccountService accService, ITransactionService transService,IMapper _mapper)
        {
            bankService = bnkService;
            accountService = accService;
            mapper = _mapper;
            transactionService = transService;
        }
        [HttpGet("getTransByAcc/{accountId}")]
        public List<TransactionViewModel> GetTransByAcc(string accountId)
        {
            return bankService.GetAccountTransactions(accountId);
        }

        [HttpGet("getTransById/{transId}")]
        public TransactionViewModel GetTransById(string transactionId)
        {
            return transactionService.GetTransactionById(transactionId);
        }
        [HttpGet("getTransByDate/{date}")]
        public List<TransactionViewModel> GetTransactionByDate(DateTime date,string bankId)
        {
            return bankService.GetTransactionsByDate(date,bankId);
        }
        [HttpGet("getBankTrans/{bankId}")]
        public List<TransactionViewModel> GetTransactionsOfBank(string bankId)
        {
            return bankService.GetTransactions(bankId);
        }

    }
}
