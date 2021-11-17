using System.IO;

namespace BankingApplication.Models
{
    public class Constant
    {
        public static string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
        public static string filePath = projectDirectory + "\\BankingApplication.Database\\Data.json";
        public static string employeeMenu = "Choice\n1.Create Account\n2.AddBank\n3.UpdateAccount\n4.Delete an Account\n5.AddNewEmployee\n6.AddNewCurrency\n7.SetServiceCharge\n8.ViewTransactions\n9.RevertTransaction\nAny other key to logout.";
        public static string welcomeMessage = "\n\n==============================Welcome to Technovert Banking Solutions=============================.\n********************\n1.Account Holder Login\n2.Bank Employee Login.\n\nPlease enter one option\n********************";
        public static string accountNotFoundError = "Account Does not exists.\n";
        public static string customerPropertiesMenu = "Please enter an option:\n1.Name\n2.Age\n3.Gender\n4.Dob\n5.Aadhar\n6.PAN\n7.Contact Number\n8.Address\nAny other key to exit.";
        public static string accountHolderOptions= "\nChoose any one option:\n1.Deposit\n2.Withdraw\n3.Transfer Amount\n4.Print Transaction history\n5.Check Balance\n6.Logout";
    }
}
