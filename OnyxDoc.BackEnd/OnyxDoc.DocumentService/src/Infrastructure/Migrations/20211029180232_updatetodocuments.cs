using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class updatetodocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentStatus",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentStatusDesc",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextActorAction",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextActorEmail",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextActorRank",
                table: "Documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SignedDocument",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_DocumentId",
                table: "Recipients",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipients_Documents_DocumentId",
                table: "Recipients",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipients_Documents_DocumentId",
                table: "Recipients");

            migrationBuilder.DropIndex(
                name: "IX_Recipients_DocumentId",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "DocumentStatus",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DocumentStatusDesc",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "NextActorAction",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "NextActorEmail",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "NextActorRank",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "SignedDocument",
                table: "Documents");
        }
    }
}
