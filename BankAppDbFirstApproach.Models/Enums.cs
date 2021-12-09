namespace BankAppDbFirstApproach.Models
{

    public enum MainMenu
    {
        AccountHolder = 1,
        BankEmployee,
        None
    }
    public enum AccountHolderMenu
    {
        Deposit = 1,
        Withdraw,
        Transfer,
        PrintStatement,
        CheckBalance,
        LogOut
    }
    public enum BankEmployeeMenu
    {
        CreateAccount = 1,
        AddBank,
        UpdateAccount,
        DeleteAccount,
        AddNewEmployee,
        AddNewCurrency,
        ModifyServiceCharge,
        ViewTransactionsForAccount,
        ViewTransactions,
        RevertTransaction,
        Logout
    }
    public enum AccountType
    {
        Savings = 1,
        Current
    }
    public enum TransactionType
    {
        Credit,
        Debit,
        Transfer,
        ServiceCharge
    }
    public enum ModeOfTransfer
    {
        RTGS = 1,
        IMPS,
        None
    }
    public enum EmployeeDesignation
    {
        Manager = 1,
        AccountsManager,
        FinancialAnalyst,
        LoanOfficer
    }
    public enum AccountStatus
    {
        Active,
        Inactive,
        Closed
    }
    public enum Gender
    {
        Male = 1,
        Female,
        PreferNotToSay
    }
    public enum CustomerProperties
    {
        Name = 1,
        Age,
        Gender,
        Dob,
        AadharNumber,
        PanNumber,
        ContactNumber,
        Address,
        None
    }
}