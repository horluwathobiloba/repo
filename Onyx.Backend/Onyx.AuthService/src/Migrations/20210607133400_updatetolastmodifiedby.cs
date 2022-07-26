using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.AuthService.Infrastructure.Migrations
{
    public partial class updatetolastmodifiedby : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "UserCount",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "TodoList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "TodoItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "RolePermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "PasswordResetAttempts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_OrganizationId",
                table: "Roles",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Organizations_OrganizationId",
                table: "Roles",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Organizations_OrganizationId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_OrganizationId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "UserCount");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "TodoList");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "TodoItem");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "PasswordResetAttempts");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Clients");
        }
    }
}
