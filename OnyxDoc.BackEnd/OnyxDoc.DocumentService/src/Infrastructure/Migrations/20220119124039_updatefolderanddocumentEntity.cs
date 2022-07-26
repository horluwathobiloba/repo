using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class updatefolderanddocumentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FolderRecipients_Folders_FolderId",
                table: "FolderRecipients");

            migrationBuilder.DropColumn(
                name: "SubcriberId",
                table: "Folders");

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "FolderRecipients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FolderRecipients_Folders_FolderId",
                table: "FolderRecipients",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FolderRecipients_Folders_FolderId",
                table: "FolderRecipients");

            migrationBuilder.AddColumn<int>(
                name: "SubcriberId",
                table: "Folders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "FolderRecipients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_FolderRecipients_Folders_FolderId",
                table: "FolderRecipients",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
