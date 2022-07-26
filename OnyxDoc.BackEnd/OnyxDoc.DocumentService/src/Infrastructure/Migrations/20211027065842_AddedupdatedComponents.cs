using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class AddedupdatedComponents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Components",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Label",
                table: "Components");
        }
    }
}
