using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.AuthService.Infrastructure.Migrations
{
    public partial class adddescforroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessLevelDesc",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkflowUserCategoryDesc",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessLevelDesc",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "WorkflowUserCategoryDesc",
                table: "Roles");
        }
    }
}
