using AutoMapper;
using BankAppDbFirstApproach.Data;
using BankAppDbFirstApproach.Models;
using BankAppDbFirstApproach.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankAppDbFirstApproach.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IBankService _bankService;
        private readonly IMapper _mapper;

        public AccountsController(IAccountService accService, IBankService bankService,IMapper mapper)
        {
            _accountService = accService;
            _bankService = bankService;
            _mapper = mapper;
        }

        [HttpGet]
        public List<AccountViewModel> GetAllAccounts()
        {
            return _accountService.GetAllAccounts();
        }
        [HttpGet("getAccountById/{id}")]
        public AccountViewModel GetAccountById(string id)
        {
            return _accountService.GetAccountById(id);
        }
        [HttpGet("getAccountByAccNum/{accNumber}")]
        public AccountViewModel GetAccountByAccNum(string accNumber)
        {
            return _accountService.GetAccountByAccNumber(accNumber);
        }
        [HttpPost("createAccount/{bankId}")]
        public void CreateAccount([FromBody] CustomerViewModel customer, [FromRoute]string bankId,[FromRoute]AccountType accType)
        {
            BankViewModel bank = _bankService.GetBankById(bankId);
            AccountViewModel newAccount = new AccountViewModel(customer,accType,bank);
            _bankService.CreateAndAddAccount(newAccount, customer,bank);
        }
        [HttpDelete("deleteAcc/{id}")]
        public void Delete(string id)
        {
            Account acc = _mapper.Map<Account>(_accountService.GetAccountById(id));
            _bankService.DeleteAccount(acc);
        }
    }
}
