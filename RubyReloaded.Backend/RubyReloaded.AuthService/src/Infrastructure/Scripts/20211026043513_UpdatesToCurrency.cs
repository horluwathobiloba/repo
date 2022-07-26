using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.AuthService.Infrastructure.Migrations
{
    public partial class UpdatesToCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "SystemOwnerUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "SystemOwnerUsers");
        }
    }
}
