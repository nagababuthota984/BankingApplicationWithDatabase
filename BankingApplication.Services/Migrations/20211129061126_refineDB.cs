using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApplication.Services.Migrations
{
    public partial class refineDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bank_Currency_DefaultCurrencyName",
                table: "bank");

            migrationBuilder.DropForeignKey(
                name: "FK_transaction_Currency_CurrencyName",
                table: "transaction");

            migrationBuilder.DropIndex(
                name: "IX_transaction_CurrencyName",
                table: "transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_bank_DefaultCurrencyName",
                table: "bank");

            migrationBuilder.DropColumn(
                name: "CurrencyName",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "DefaultCurrencyName",
                table: "bank");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "transaction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Currency",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Currency",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DefaultCurrencyName",
                table: "bank",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "Id");

            migrationBuilder.InsertData(
                table: "bank",
                columns: new[] { "BankId", "Balance", "BankName", "Branch", "DefaultCurrencyName", "Ifsc", "OtherIMPS", "OtherRTGS", "SelfIMPS", "SelfRTGS" },
                values: new object[] { "Axi20211024", 0m, "AxisBank", "Guntur", "INR", "UBIN0000261", 6m, 2m, 5m, 0m });

            migrationBuilder.InsertData(
                table: "bank",
                columns: new[] { "BankId", "Balance", "BankName", "Branch", "DefaultCurrencyName", "Ifsc", "OtherIMPS", "OtherRTGS", "SelfIMPS", "SelfRTGS" },
                values: new object[] { "Sta20211026", 0m, "StateBankOfIndia", "Guntur", "INR", "SBIN000232", 6m, 2m, 5m, 0m });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "BankId", "ExchangeRate", "Name" },
                values: new object[] { "Axi20211024INR", "Axi20211024", 1m, "INR" });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "BankId", "ExchangeRate", "Name" },
                values: new object[] { "Sta20211026INR", "Sta20211026", 1m, "INR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyColumnType: "nvarchar(450)",
                keyValue: "Axi20211024INR");

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyColumnType: "nvarchar(450)",
                keyValue: "Sta20211026INR");

            migrationBuilder.DeleteData(
                table: "bank",
                keyColumn: "BankId",
                keyValue: "Axi20211024");

            migrationBuilder.DeleteData(
                table: "bank",
                keyColumn: "BankId",
                keyValue: "Sta20211026");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "DefaultCurrencyName",
                table: "bank");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                table: "transaction",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Currency",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefaultCurrencyName",
                table: "bank",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_CurrencyName",
                table: "transaction",
                column: "CurrencyName");

            migrationBuilder.CreateIndex(
                name: "IX_bank_DefaultCurrencyName",
                table: "bank",
                column: "DefaultCurrencyName");

            migrationBuilder.AddForeignKey(
                name: "FK_bank_Currency_DefaultCurrencyName",
                table: "bank",
                column: "DefaultCurrencyName",
                principalTable: "Currency",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_Currency_CurrencyName",
                table: "transaction",
                column: "CurrencyName",
                principalTable: "Currency",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
