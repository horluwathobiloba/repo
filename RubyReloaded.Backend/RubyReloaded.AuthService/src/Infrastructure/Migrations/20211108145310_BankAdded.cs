using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.AuthService.Infrastructure.Migrations
{
    public partial class BankAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bank",
                table: "PaymentChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SettlementAccountNumber",
                table: "PaymentChannels",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bank",
                table: "PaymentChannels");

            migrationBuilder.DropColumn(
                name: "SettlementAccountNumber",
                table: "PaymentChannels");
        }
    }
}
