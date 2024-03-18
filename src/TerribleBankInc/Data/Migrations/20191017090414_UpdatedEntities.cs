using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TerribleBankInc.Data.Migrations
{
    public partial class UpdatedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ForgotPasswordExpiration",
                table: "Users",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "ForgotPasswordToken",
                table: "Users",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "DestinationAccountNumber",
                table: "Transactions",
                nullable: true
            );

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "BankAccounts",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "BankAccounts",
                nullable: false,
                defaultValue: false
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "ForgotPasswordExpiration", table: "Users");

            migrationBuilder.DropColumn(name: "ForgotPasswordToken", table: "Users");

            migrationBuilder.DropColumn(name: "DestinationAccountNumber", table: "Transactions");

            migrationBuilder.DropColumn(name: "Approved", table: "BankAccounts");

            migrationBuilder.DropColumn(name: "Enabled", table: "BankAccounts");
        }
    }
}
