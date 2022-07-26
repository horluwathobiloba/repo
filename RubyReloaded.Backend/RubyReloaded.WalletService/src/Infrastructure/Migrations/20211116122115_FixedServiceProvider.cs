using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class FixedServiceProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceProvider",
                table: "ServiceConfigurations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceProvider",
                table: "ServiceConfigurations");
        }
    }
}
