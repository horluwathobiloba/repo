using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class MIGRATIONFORDOCUMENTS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inboxes_Recipients_RecipientId",
                table: "Inboxes");

            migrationBuilder.DropIndex(
                name: "IX_Inboxes_RecipientId",
                table: "Inboxes");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Inboxes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                table: "Inboxes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inboxes_RecipientId",
                table: "Inboxes",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inboxes_Recipients_RecipientId",
                table: "Inboxes",
                column: "RecipientId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
