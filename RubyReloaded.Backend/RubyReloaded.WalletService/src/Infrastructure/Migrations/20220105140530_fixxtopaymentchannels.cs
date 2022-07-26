using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class fixxtopaymentchannels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ProductInterestPeriod");

            migrationBuilder.RenameColumn(
                name: "Interval",
                table: "ProductInterestPeriod",
                newName: "HoldingPeriod");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "ProductFundingSources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bank",
                table: "ProductFundingSources",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "ProductFundingSources");

            migrationBuilder.DropColumn(
                name: "Bank",
                table: "ProductFundingSources");

            migrationBuilder.RenameColumn(
                name: "HoldingPeriod",
                table: "ProductInterestPeriod",
                newName: "Interval");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "ProductInterestPeriod",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
