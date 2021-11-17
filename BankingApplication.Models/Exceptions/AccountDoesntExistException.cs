using System;

namespace BankingApplication.Models
{
    public class AccountDoesntExistException : Exception
    {
        public AccountDoesntExistException(string message) : base(message)
        {

        }
    }
}
