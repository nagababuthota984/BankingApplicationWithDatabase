using System;

namespace BankingApplication.Models
{
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException(string message) : base(message)
        {

        }
    }
}
