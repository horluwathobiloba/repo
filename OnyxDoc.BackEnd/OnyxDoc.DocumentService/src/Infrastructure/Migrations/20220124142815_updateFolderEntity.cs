using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class updateFolderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolderRecipients");

            migrationBuilder.AddColumn<int>(
                name: "FileType",
                table: "Recipients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "Recipients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PermissionStatus",
                table: "Recipients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Recipients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "Recipients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_FolderId",
                table: "Recipients",
                column: "FolderId");

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

            migrationBuilder.DropIndex(
                name: "IX_Recipients_FolderId",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "PermissionStatus",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "Recipients");

            migrationBuilder.CreateTable(
                name: "FolderRecipients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FolderId = table.Column<int>(type: "int", nullable: false),
                    FolderPermissionStatus = table.Column<int>(type: "int", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoldId = table.Column<int>(type: "int", nullable: false),
                    RoldName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    SubscriberName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FolderRecipients_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FolderRecipients_FolderId",
                table: "FolderRecipients",
                column: "FolderId");
        }
    }
}
