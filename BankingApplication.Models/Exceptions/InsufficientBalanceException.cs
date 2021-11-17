using System;

namespace BankingApplication.Models
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException(string message) : base(message)
        {

        }
    }
}
