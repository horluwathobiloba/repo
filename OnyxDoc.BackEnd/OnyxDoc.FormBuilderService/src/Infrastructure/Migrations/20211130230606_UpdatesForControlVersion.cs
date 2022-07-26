using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.FormBuilderService.Infrastructure.Migrations
{
    public partial class UpdatesForControlVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InitialControlVersionId",
                table: "Controls",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialControlVersionId",
                table: "Controls");
        }
    }
}
