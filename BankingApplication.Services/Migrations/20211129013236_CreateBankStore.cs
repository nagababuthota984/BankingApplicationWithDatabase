using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApplication.Services.Migrations
{
    public partial class CreateBankStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharNumber = table.Column<long>(type: "bigint", nullable: false),
                    PanNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    TransId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    SenderAccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverAccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderBankId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverBankId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    On = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TransferMode = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BankId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.TransId);
                });

            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BankId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BankId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "bank",
                columns: table => new
                {
                    BankId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ifsc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelfRTGS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SelfIMPS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherRTGS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherIMPS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultCurrencyName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank", x => x.BankId);
                    table.ForeignKey(
                        name: "FK_bank_Currency_DefaultCurrencyName",
                        column: x => x.DefaultCurrencyName,
                        principalTable: "Currency",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_employee_bank_BankId",
                        column: x => x.BankId,
                        principalTable: "bank",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_BankId",
                table: "account",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_bank_DefaultCurrencyName",
                table: "bank",
                column: "DefaultCurrencyName");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_BankId",
                table: "Currency",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AccountId",
                table: "Customer",
                column: "AccountId",
                unique: true,
                filter: "[AccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_employee_BankId",
                table: "employee",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_AccountId",
                table: "transaction",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_BankId",
                table: "transaction",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_CurrencyName",
                table: "transaction",
                column: "CurrencyName");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_account_AccountId",
                table: "Customer",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_account_AccountId",
                table: "transaction",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_bank_BankId",
                table: "transaction",
                column: "BankId",
                principalTable: "bank",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_Currency_CurrencyName",
                table: "transaction",
                column: "CurrencyName",
                principalTable: "Currency",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_account_bank_BankId",
                table: "account",
                column: "BankId",
                principalTable: "bank",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Currency_bank_BankId",
                table: "Currency",
                column: "BankId",
                principalTable: "bank",
                principalColumn: "BankId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currency_bank_BankId",
                table: "Currency");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "bank");

            migrationBuilder.DropTable(
                name: "Currency");
        }
    }
}
