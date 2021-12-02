using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApplication.Services.Migrations
{
    public partial class employeecustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "Dob",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "employee");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "employee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Dob",
                table: "employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "employee",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
