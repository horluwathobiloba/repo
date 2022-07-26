using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class FixToWallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClosingingBalance",
                table: "Wallets",
                newName: "ClosingBalance");

            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WalletAccountNumber",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "WalletAccountNumber",
                table: "Wallets");

            migrationBuilder.RenameColumn(
                name: "ClosingBalance",
                table: "Wallets",
                newName: "ClosingingBalance");
        }
    }
}
