using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class ReportValueAddedModuleNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AuditLogs");
        }
    }
}
