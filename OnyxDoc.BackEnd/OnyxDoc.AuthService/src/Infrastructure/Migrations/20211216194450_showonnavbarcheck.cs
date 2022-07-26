using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.AuthService.Infrastructure.Migrations
{
    public partial class showonnavbarcheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShouldShowOnNavBar",
                table: "Features",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldShowOnNavBar",
                table: "Features");
        }
    }
}
