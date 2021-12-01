using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApplication.Services.Migrations
{
    public partial class updatetransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_account_AccountId",
                table: "Customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "customer");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_AccountId",
                table: "customer",
                newName: "IX_customer_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customer",
                table: "customer",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_customer_account_AccountId",
                table: "customer",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_account_AccountId",
                table: "customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customer",
                table: "customer");

            migrationBuilder.RenameTable(
                name: "customer",
                newName: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_customer_AccountId",
                table: "Customer",
                newName: "IX_Customer_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_account_AccountId",
                table: "Customer",
                column: "AccountId",
                principalTable: "account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
