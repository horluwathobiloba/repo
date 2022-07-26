using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class UpdateToFolders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Folders");

            migrationBuilder.RenameColumn(
                name: "PermissionStatus",
                table: "Recipients",
                newName: "FilePermission");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "FolderShareDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FolderStatus",
                table: "Folders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FolderStatus",
                table: "Folders");

            migrationBuilder.RenameColumn(
                name: "FilePermission",
                table: "Recipients",
                newName: "PermissionStatus");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "FolderShareDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Folders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Folders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
