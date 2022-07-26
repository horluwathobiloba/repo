using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.AuthService.Infrastructure.Migrations
{
    public partial class migrationservices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleAccessLevel",
                table: "DefaultRolesConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RoleAccessLevelDesc",
                table: "DefaultRolesConfigurations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleAccessLevel",
                table: "DefaultRolesConfigurations");

            migrationBuilder.DropColumn(
                name: "RoleAccessLevelDesc",
                table: "DefaultRolesConfigurations");
        }
    }
}
