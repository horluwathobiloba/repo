using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class InitialMigrationForComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Dimensions");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Dimensions");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Dimensions",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Top",
                table: "Dimensions",
                newName: "Validators");

            migrationBuilder.RenameColumn(
                name: "Left",
                table: "Dimensions",
                newName: "FilePath");

            migrationBuilder.AddColumn<int>(
                name: "CoordinateId",
                table: "Dimensions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SelectOptions",
                table: "Dimensions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ContractComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    CommentById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractCommentType = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    SubscriberName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractComments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coordinate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Transform = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    SubscriberName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinate", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dimensions_CoordinateId",
                table: "Dimensions",
                column: "CoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractComments_DocumentId",
                table: "ContractComments",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dimensions_Coordinate_CoordinateId",
                table: "Dimensions",
                column: "CoordinateId",
                principalTable: "Coordinate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dimensions_Coordinate_CoordinateId",
                table: "Dimensions");

            migrationBuilder.DropTable(
                name: "ContractComments");

            migrationBuilder.DropTable(
                name: "Coordinate");

            migrationBuilder.DropIndex(
                name: "IX_Dimensions_CoordinateId",
                table: "Dimensions");

            migrationBuilder.DropColumn(
                name: "CoordinateId",
                table: "Dimensions");

            migrationBuilder.DropColumn(
                name: "SelectOptions",
                table: "Dimensions");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Dimensions",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "Validators",
                table: "Dimensions",
                newName: "Top");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Dimensions",
                newName: "Left");

            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "Dimensions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Dimensions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
