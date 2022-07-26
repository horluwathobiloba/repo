using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class UpdateToComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoordinateType",
                table: "Coordinates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoordinateId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CoordinateId",
                table: "Comments",
                column: "CoordinateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Coordinates_CoordinateId",
                table: "Comments",
                column: "CoordinateId",
                principalTable: "Coordinates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Coordinates_CoordinateId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CoordinateId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CoordinateType",
                table: "Coordinates");

            migrationBuilder.DropColumn(
                name: "CoordinateId",
                table: "Comments");
        }
    }
}
