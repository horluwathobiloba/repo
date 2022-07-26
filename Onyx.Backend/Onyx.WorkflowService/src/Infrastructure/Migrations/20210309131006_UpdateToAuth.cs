using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.WorkFlowService.Infrastructure.Migrations
{
    public partial class UpdateToAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GenerateStaffCode",
                table: "Organizations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenerateStaffCode",
                table: "Organizations");
        }
    }
}
