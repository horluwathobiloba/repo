using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class AdditionToMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipients_Folders_FolderId",
                table: "Recipients");

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "Recipients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipients_Folders_FolderId",
                table: "Recipients",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipients_Folders_FolderId",
                table: "Recipients");

            migrationBuilder.AlterColumn<int>(
                name: "FolderId",
                table: "Recipients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipients_Folders_FolderId",
                table: "Recipients",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
