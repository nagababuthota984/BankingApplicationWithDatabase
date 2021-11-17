using System;
using System.Runtime.Serialization;

namespace BankingApplication.Models
{
    public class TransactionDoesntExist : Exception
    {
        
        public TransactionDoesntExist(string message) : base(message)
        {
        }
    }
}