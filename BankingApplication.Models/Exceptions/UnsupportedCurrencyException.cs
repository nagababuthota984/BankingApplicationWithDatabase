using System;
using System.Runtime.Serialization;

namespace BankingApplication.Models
{
    [Serializable]
    public class UnsupportedCurrencyException : Exception
    {
        

        public UnsupportedCurrencyException(string message) : base(message)
        {
        }

        
    }
}