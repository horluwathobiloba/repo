using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class TransactionUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bank",
                table: "PaymentChannels");

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "PaymentChannels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentChannels_BankId",
                table: "PaymentChannels",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentChannels_Banks_BankId",
                table: "PaymentChannels",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentChannels_Banks_BankId",
                table: "PaymentChannels");

            migrationBuilder.DropIndex(
                name: "IX_PaymentChannels_BankId",
                table: "PaymentChannels");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "PaymentChannels");

            migrationBuilder.AddColumn<string>(
                name: "Bank",
                table: "PaymentChannels",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
