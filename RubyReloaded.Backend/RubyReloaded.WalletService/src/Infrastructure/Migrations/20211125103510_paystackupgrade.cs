using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class paystackupgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentChannelCategory",
                table: "PaymentChannels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentChannelCategoryDesc",
                table: "PaymentChannels",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentChannelCategory",
                table: "PaymentChannels");

            migrationBuilder.DropColumn(
                name: "PaymentChannelCategoryDesc",
                table: "PaymentChannels");
        }
    }
}
