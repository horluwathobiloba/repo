using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.AuthService.Infrastructure.Migrations
{
    public partial class addedexplorepostfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostImages",
                table: "ExplorePosts");

            migrationBuilder.RenameColumn(
                name: "ExploreImageType",
                table: "ExplorePosts",
                newName: "ExplorePostFileId");

            migrationBuilder.CreateTable(
                name: "ExplorePostFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExplorePostFileURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExploreImageType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorePostFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExplorePosts_ExplorePostFileId",
                table: "ExplorePosts",
                column: "ExplorePostFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExplorePosts_ExplorePostFiles_ExplorePostFileId",
                table: "ExplorePosts",
                column: "ExplorePostFileId",
                principalTable: "ExplorePostFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExplorePosts_ExplorePostFiles_ExplorePostFileId",
                table: "ExplorePosts");

            migrationBuilder.DropTable(
                name: "ExplorePostFiles");

            migrationBuilder.DropIndex(
                name: "IX_ExplorePosts_ExplorePostFileId",
                table: "ExplorePosts");

            migrationBuilder.RenameColumn(
                name: "ExplorePostFileId",
                table: "ExplorePosts",
                newName: "ExploreImageType");

            migrationBuilder.AddColumn<string>(
                name: "PostImages",
                table: "ExplorePosts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
