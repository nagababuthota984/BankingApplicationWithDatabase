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

        public AccountsController(IAccountService accService, IBankService bankService, IMapper mapper)
        {
            _accountService = accService;
            _bankService = bankService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            return Ok(_accountService.GetAllAccounts());
        }
        [HttpGet("getAccountById/{id}")]
        public IActionResult GetAccountById(string id)
        {

            AccountViewModel account = _accountService.GetAccountById(id);
            if (account != null)
                return Ok(account);
            else
                return NotFound("Account with matching id not found");


        }
        [HttpGet("getAccountByAccNum/{accNumber}")]
        public IActionResult GetAccountByAccNum(string accNumber)
        {
            AccountViewModel account = _accountService.GetAccountByAccNumber(accNumber);
            if (account != null)
                return Ok(account);
            else
                return NotFound("Account with matching Account Number not found");
        }
        [HttpPost("createAccount/{bankId}")]
        public IActionResult CreateAccount([FromBody] CustomerViewModel customer, [FromRoute] string bankId, [FromRoute] AccountType accType)
        {
            BankViewModel bank = _bankService.GetBankById(bankId);
            if (bank != null)
            {
                AccountViewModel newAccount = new AccountViewModel(customer, accType, bank);
                return Ok(_bankService.CreateAndAddAccount(newAccount, customer, bank));
            }
            else
                return NotFound("Bank with matching id not found.Please give valid bank id.");
        }
        [HttpPut("updateCustomer")]
        public IActionResult UpdateAccount([FromBody] CustomerViewModel customer)
        {
            _accountService.UpdateAccount(customer);
            return Ok();
        }
        [HttpDelete("deleteAcc/{id}")]
        public IActionResult Delete(string id)
        {
            Account acc = _mapper.Map<Account>(_accountService.GetAccountById(id));
            if (acc != null)
            {
                _bankService.DeleteAccount(acc);
                return Ok();
            }
            else
                return NotFound("Account with matching id not found.Please provide a valid Account ID");

        }
        [HttpPut("deposit/{accountId}/{amount}/{currencyName}")]
        public IActionResult Deposit([FromRoute]string accountId, decimal amount, [FromRoute] string currencyName)
        {
            AccountViewModel account = _accountService.GetAccountById(accountId);
            if (account != null)
            {
                CurrencyViewModel curr = _bankService.GetCurrencyByName(currencyName);
                if (curr != null)
                {
                    if (amount > 0)
                    {
                        _accountService.DepositAmount(account, amount, curr);
                        return Ok();
                    }
                    else
                        return BadRequest("Depositing amount should be greater than 0.");
                }
                else
                    return NotFound("Currency with matching name not found.");
            }
            else
                return NotFound("Account with matching Id not found.Please provide a valid account ID");
        }
        [HttpPut("withdraw")]
        public IActionResult Withdraw(string accountId, decimal amount)
        {
            AccountViewModel account = _accountService.GetAccountById(accountId);
            if (account != null)
            {
                if (amount > 0)
                {
                    if (amount <= account.Balance)
                    {
                        _accountService.WithdrawAmount(account, amount);
                        return Ok();
                    }
                    else
                        return BadRequest("Insufficient funds.");
                }
                else
                    return BadRequest("Withdrawl amount should be greater than 0.");
            }
            else
                return NotFound("Account with matching Id not found");
        }
        [HttpPut("transfer")]
        public IActionResult Transfer(string senderAccId, string receiverAccNumber, decimal amount, ModeOfTransferOptions mode)
        {
            AccountViewModel senderAccount = _accountService.GetAccountById(senderAccId);
            if (senderAccount != null)
            {
                AccountViewModel receiverAccount = _accountService.GetAccountByAccNumber(receiverAccNumber);
                if (receiverAccount != null)
                {
                    if (amount > 0)
                    {
                        if (amount >= senderAccount.Balance)
                        {
                            BankViewModel senderBank = _bankService.GetBankById(senderAccount.BankId);
                            _accountService.TransferAmount(senderAccount, senderBank, receiverAccount, amount, mode);
                            return Ok();
                        }
                        else
                            return BadRequest("Insufficient funds.");
                    }
                    else
                        return BadRequest("Amount should be greater than 0.");
                }
                else
                    return NotFound("Receiver Account with matching Account Number not found");
            }
            else
                return NotFound("Account with matching Account Number not found");
        }
    }
}
