using Microsoft.EntityFrameworkCore;

namespace BankAppDbFirstApproach.Data
{
    public partial class BankStorageContext : DbContext
    {
        public BankStorageContext()
        {
        }

        public BankStorageContext(DbContextOptions<BankStorageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=BankStorage;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .HasName("UQ__account__F3DBC5729C769750")
                    .IsUnique();

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasColumnName("accountNumber")
                    .HasMaxLength(450);

                entity.Property(e => e.AccountType).HasColumnName("accountType");

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BankId)
                    .HasColumnName("bankId")
                    .HasMaxLength(450);

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerId")
                    .HasMaxLength(450);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(450);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK__account__bankId__4222D4EF");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_account_customer");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasIndex(e => e.Bankname)
                    .HasName("UQ__bank__206168F8576BD5B7")
                    .IsUnique();

                entity.Property(e => e.BankId).HasColumnName("bankId");

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Bankname)
                    .IsRequired()
                    .HasColumnName("bankname");

                entity.Property(e => e.Branch)
                    .IsRequired()
                    .HasColumnName("branch")
                    .HasMaxLength(450);

                entity.Property(e => e.DefaultCurrencyName)
                    .IsRequired()
                    .HasColumnName("defaultCurrencyName")
                    .HasMaxLength(40);

                entity.Property(e => e.Ifsc)
                    .IsRequired()
                    .HasColumnName("ifsc")
                    .HasMaxLength(450);

                entity.Property(e => e.OtherImps)
                    .HasColumnName("otherIMPS")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.OtherRtgs)
                    .HasColumnName("otherRTGS")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.SelfImps)
                    .HasColumnName("selfIMPS")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.SelfRtgs)
                    .HasColumnName("selfRTGS")
                    .HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.BankId)
                    .HasColumnName("bankId")
                    .HasMaxLength(450);

                entity.Property(e => e.ExchangeRate)
                    .HasColumnName("exchangeRate")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Currency)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK__currency__bankId__3A81B327");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.AadharNumber).HasColumnName("aadharNumber");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(450);

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.ContactNumber)
                    .IsRequired()
                    .HasColumnName("contactNumber")
                    .HasMaxLength(450);

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("datetime");

                entity.Property(e => e.GenderOptions).HasColumnName("gender");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(450);

                entity.Property(e => e.PanNumber)
                    .IsRequired()
                    .HasColumnName("panNumber")
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .HasName("UQ__employee__F3DBC57229C10B7B")
                    .IsUnique();

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.BankId)
                    .HasColumnName("bankId")
                    .HasMaxLength(450);

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasColumnName("customerId")
                    .HasMaxLength(450);

                entity.Property(e => e.Designation).HasColumnName("designation");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(450);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK__employee__bankId__3E52440B");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employee_customer");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransId)
                    .HasName("PK__transact__DB107FA7045820EB");

                entity.Property(e => e.TransId).HasColumnName("transId");

                entity.Property(e => e.AccountId)
                    .HasColumnName("accountId")
                    .HasMaxLength(450);

                entity.Property(e => e.Balance)
                    .HasColumnName("balance")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BankId)
                    .HasColumnName("bankId")
                    .HasMaxLength(450);

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasColumnName("currency")
                    .HasMaxLength(450);

                entity.Property(e => e.IsBankTransaction).HasColumnName("isBankTransaction");

                entity.Property(e => e.ModeOfTransfer).HasColumnName("modeOfTransfer");

                entity.Property(e => e.OtherPartyBankId)
                    .HasColumnName("otherPartyBankId")
                    .HasMaxLength(450);

                entity.Property(e => e.Receivername)
                    .IsRequired()
                    .HasColumnName("receivername")
                    .HasMaxLength(450);

                entity.Property(e => e.Sendername)
                    .IsRequired()
                    .HasColumnName("sendername")
                    .HasMaxLength(450);

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("transactionAmount")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TransactionOn)
                    .HasColumnName("transactionOn")
                    .HasColumnType("datetime");

                entity.Property(e => e.TransactionType).HasColumnName("transactionType");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__transacti__accou__47DBAE45");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.TransactionBank)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("Fk_transaction_bank");

                entity.HasOne(d => d.OtherPartyBank)
                    .WithMany(p => p.TransactionOtherPartyBank)
                    .HasForeignKey(d => d.OtherPartyBankId)
                    .HasConstraintName("FK__Transacti__other__6E01572D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
