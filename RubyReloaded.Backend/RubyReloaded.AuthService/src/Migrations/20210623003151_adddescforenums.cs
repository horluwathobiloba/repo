using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.AuthService.Infrastructure.Migrations
{
    public partial class adddescforenums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusDesc",
                table: "UserCount",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusDesc",
                table: "TodoList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusDesc",
                table: "TodoItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusDesc",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusDesc",
                table: "RolePermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusDesc",
                table: "PasswordResetAttempts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusDesc",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusDesc",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "UserCount");

            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "TodoList");

            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "TodoItem");

            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "PasswordResetAttempts");

            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "Clients");
        }
    }
}
