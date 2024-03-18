using Microsoft.EntityFrameworkCore.Migrations;

namespace TerribleBankInc.Data.Migrations
{
    public partial class UpdatedEntities2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Transactions",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "SourceAccountNumber",
                table: "Transactions",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "SourceClientEmail",
                table: "Transactions",
                nullable: true
            );

            migrationBuilder.AlterColumn<bool>(
                name: "Approved",
                table: "BankAccounts",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit"
            );

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "BankAccounts",
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Details", table: "Transactions");

            migrationBuilder.DropColumn(name: "SourceAccountNumber", table: "Transactions");

            migrationBuilder.DropColumn(name: "SourceClientEmail", table: "Transactions");

            migrationBuilder.DropColumn(name: "Reason", table: "BankAccounts");

            migrationBuilder.AlterColumn<bool>(
                name: "Approved",
                table: "BankAccounts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true
            );
        }
    }
}
