using BankingApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingApplication.Services
{
    public class Utilities
    {
        //Contains all the helper methods needed for AccountService and BankServices.

        //remove this
        internal static bool IsDuplicateAccountNumber(string accountNumber, string bankid)
        {

            var RequiredBank = RBIStorage.banks.SingleOrDefault(bank => bank.BankId == bankid);
            if (RequiredBank != null)
            {
                foreach (var Acc in RequiredBank.Accounts)
                {
                    if (Acc.AccountNumber == accountNumber)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        internal static string GenerateRandomNumber(int length)
        {
            Random r = new Random();          //account number generator.
            string NumberString = "";
            int i;
            for (i = 1; i < length; i++)
            {
                NumberString += r.Next(0, 9).ToString();
            }
            return NumberString;
        }
    }
}
