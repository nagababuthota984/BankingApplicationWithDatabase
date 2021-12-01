using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;


namespace BankingApplication.Services
{
    public class BankAppDbContext : DbContext
    {
        public DbSet<Bank> bank { get; set; }
        public DbSet<Account> account { get; set; }
        public DbSet<Employee> employee { get; set; }
        public DbSet<Transaction> transaction { get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<Currency> currency { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString: "Data Source=NAG1211-HP-LAPT\\SQLEXPRESS;Initial Catalog=BankStore;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Bank>().ToTable("bank")
                .HasData(new Bank { BankId = "Axi20211024", BankName = "AxisBank", Branch = "Guntur", Ifsc = "UBIN0000261", SelfRTGS = 0, SelfIMPS = 5, OtherRTGS = 2, OtherIMPS = 6, DefaultCurrencyName = "INR" },
                         new Bank { BankId= "Sta20211026",BankName= "StateBankOfIndia",Branch="Guntur",Ifsc= "SBIN000232",SelfRTGS=0,SelfIMPS=5,OtherRTGS=2,OtherIMPS=6,DefaultCurrencyName="INR" });
            
            
            modelBuilder.Entity<Currency>().ToTable("Currency")
               .HasData(new Currency("INR", 1, "Axi20211024"),
                        new Currency("INR",1, "Sta20211026"));
        }
    }
}
