using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.FormBuilderService.Infrastructure.Migrations
{
    public partial class UpdatesForDocumentVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InitialDocumentVersionId",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialDocumentVersionId",
                table: "Documents");
        }
    }
}
