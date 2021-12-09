using BankAppDbFirstApproach.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppDbFirstApproach.CLI
{
    public class UserOutput
    {
        
        public static void ShowTransactions(List<Transaction> Transactions)
        {
            int count = 1;

            if (Transactions.Count>=1)
            {
                string heading = "Sno  | Transaction Id\t\t\t\t|  Type  | Amount | Balance | Transaction On";
                Console.WriteLine(heading);
                Console.WriteLine("-----------------------------------------------------------------------------------------------");
                foreach (Transaction trans in Transactions)
                {
                    string output = $"{count,5}|{trans.transId,19}   |{(TransactionType)trans.transactionType,7}|{trans.transactionAmount,7}|{trans.balance,10}|{trans.transactionOn}";
                    Console.WriteLine(output);
                    count++;
                    Console.WriteLine();
                } 
            }
            else
            {
                Console.WriteLine("\nNo transactions to show!\n");
            }
        }

        internal static void ShowMessage(string output)
        {
            Console.WriteLine(output);
        }
    }
}
