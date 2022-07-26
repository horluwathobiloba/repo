using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class fixtoinbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Inboxes_DocumentId",
                table: "Inboxes",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inboxes_Documents_DocumentId",
                table: "Inboxes",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inboxes_Documents_DocumentId",
                table: "Inboxes");

            migrationBuilder.DropIndex(
                name: "IX_Inboxes_DocumentId",
                table: "Inboxes");
        }
    }
}
