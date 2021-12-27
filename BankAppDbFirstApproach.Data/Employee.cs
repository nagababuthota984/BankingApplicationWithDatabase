namespace BankAppDbFirstApproach.Data
{
    public partial class Employee
    {
        public string EmployeeId { get; set; }
        public string BankId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Designation { get; set; }
        public string CustomerId { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
