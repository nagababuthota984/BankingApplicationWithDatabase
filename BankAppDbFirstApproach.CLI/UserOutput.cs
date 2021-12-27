using BankAppDbFirstApproach.Models;

namespace BankAppDbFirstApproach.CLI
{
    public class UserOutput
    {

        public static void ShowTransactions(List<Transaction> Transactions)
        {
            int count = 1;
            if (Transactions.Count >= 1)
            {
                Console.WriteLine(Constant.displayTransactionHeader);
                Console.WriteLine(Constant.lineBreak);
                foreach (Transaction trans in Transactions.OrderBy(tr => tr.transactionOn))
                {
                    string output = $"{count,5}|{trans.transId,50}   |{(TransactionType)trans.transactionType,14}|{trans.transactionAmount,7}|{trans.balance,10}|{trans.transactionOn}";
                    Console.WriteLine(output);
                    count++;
                    Console.WriteLine();
                }
            }
            else
                Console.WriteLine(Constant.noTransactions);

        }

        internal static void ShowMessage(string output)
        {
            Console.WriteLine(output);
        }
    }
}
