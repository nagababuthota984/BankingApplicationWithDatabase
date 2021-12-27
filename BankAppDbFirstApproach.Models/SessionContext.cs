namespace BankAppDbFirstApproach.Models
{
    public static class SessionContext
    {
        public static BankViewModel Bank { get; set; }
        public static AccountViewModel Account { get; set; }
        public static EmployeeViewModel Employee { get; set; }
    }
}
