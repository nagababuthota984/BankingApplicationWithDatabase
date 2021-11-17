using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApplication.Models
{
    public class InvalidBankException:Exception
    {
        public InvalidBankException(string message):base(message)
        {

        }
    }
}
