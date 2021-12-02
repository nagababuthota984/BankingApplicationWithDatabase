﻿// <auto-generated />
using System;
using BankingApplication.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankingApplication.Services.Migrations
{
    [DbContext(typeof(BankAppDbContext))]
    [Migration("20211201115326_employee-customer")]
    partial class employeecustomer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BankingApplication.Models.Account", b =>
                {
                    b.Property<string>("AccountId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AccountType")
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("BankId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId");

                    b.HasIndex("BankId");

                    b.ToTable("account");
                });

            modelBuilder.Entity("BankingApplication.Models.Bank", b =>
                {
                    b.Property<string>("BankId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("BankName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Branch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DefaultCurrencyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ifsc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("OtherIMPS")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OtherRTGS")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SelfIMPS")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SelfRTGS")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BankId");

                    b.ToTable("bank");

                    b.HasData(
                        new
                        {
                            BankId = "Axi20211024",
                            Balance = 0m,
                            BankName = "AxisBank",
                            Branch = "Guntur",
                            DefaultCurrencyName = "INR",
                            Ifsc = "UBIN0000261",
                            OtherIMPS = 6m,
                            OtherRTGS = 2m,
                            SelfIMPS = 5m,
                            SelfRTGS = 0m
                        },
                        new
                        {
                            BankId = "Sta20211026",
                            Balance = 0m,
                            BankName = "StateBankOfIndia",
                            Branch = "Guntur",
                            DefaultCurrencyName = "INR",
                            Ifsc = "SBIN000232",
                            OtherIMPS = 6m,
                            OtherRTGS = 2m,
                            SelfIMPS = 5m,
                            SelfRTGS = 0m
                        });
                });

            modelBuilder.Entity("BankingApplication.Models.Currency", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BankId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.ToTable("Currency");

                    b.HasData(
                        new
                        {
                            Id = "Axi20211024INR",
                            BankId = "Axi20211024",
                            ExchangeRate = 1m,
                            Name = "INR"
                        },
                        new
                        {
                            Id = "Sta20211026INR",
                            BankId = "Sta20211026",
                            ExchangeRate = 1m,
                            Name = "INR"
                        });
                });

            modelBuilder.Entity("BankingApplication.Models.Customer", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("AadharNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("AccountId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("ContactNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PanNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.HasIndex("AccountId")
                        .IsUnique()
                        .HasFilter("[AccountId] IS NOT NULL");

                    b.ToTable("customer");
                });

            modelBuilder.Entity("BankingApplication.Models.Employee", b =>
                {
                    b.Property<string>("EmployeeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BankId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Designation")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeId");

                    b.HasIndex("BankId");

                    b.ToTable("employee");
                });

            modelBuilder.Entity("BankingApplication.Models.Transaction", b =>
                {
                    b.Property<string>("TransId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("BalanceAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("BankId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("On")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReceiverAccountId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverBankId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderAccountId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderBankId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TransactionAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TransferMode")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("TransId");

                    b.HasIndex("AccountId");

                    b.HasIndex("BankId");

                    b.ToTable("transaction");
                });

            modelBuilder.Entity("BankingApplication.Models.Account", b =>
                {
                    b.HasOne("BankingApplication.Models.Bank", null)
                        .WithMany("Accounts")
                        .HasForeignKey("BankId");
                });

            modelBuilder.Entity("BankingApplication.Models.Currency", b =>
                {
                    b.HasOne("BankingApplication.Models.Bank", null)
                        .WithMany("SupportedCurrency")
                        .HasForeignKey("BankId");
                });

            modelBuilder.Entity("BankingApplication.Models.Customer", b =>
                {
                    b.HasOne("BankingApplication.Models.Account", null)
                        .WithOne("Customer")
                        .HasForeignKey("BankingApplication.Models.Customer", "AccountId");
                });

            modelBuilder.Entity("BankingApplication.Models.Employee", b =>
                {
                    b.HasOne("BankingApplication.Models.Bank", null)
                        .WithMany("Employees")
                        .HasForeignKey("BankId");
                });

            modelBuilder.Entity("BankingApplication.Models.Transaction", b =>
                {
                    b.HasOne("BankingApplication.Models.Account", null)
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId");

                    b.HasOne("BankingApplication.Models.Bank", null)
                        .WithMany("Transactions")
                        .HasForeignKey("BankId");
                });

            modelBuilder.Entity("BankingApplication.Models.Account", b =>
                {
                    b.Navigation("Customer");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("BankingApplication.Models.Bank", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Employees");

                    b.Navigation("SupportedCurrency");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}