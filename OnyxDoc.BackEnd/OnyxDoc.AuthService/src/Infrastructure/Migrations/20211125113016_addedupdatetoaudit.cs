using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.AuthService.Infrastructure.Migrations
{
    public partial class addedupdatetoaudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiUrl",
                table: "AuditTrails");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AuditTrails");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AuditTrails",
                newName: "MicroserviceName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MicroserviceName",
                table: "AuditTrails",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "ApiUrl",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AuditTrails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
