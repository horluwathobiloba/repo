using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.FormBuilderService.Infrastructure.Migrations
{
    public partial class controlpropertyfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlProperties_ControlProperties_ParentPropertyId",
                table: "ControlProperties");

            migrationBuilder.DropIndex(
                name: "IX_ControlProperties_ParentPropertyId",
                table: "ControlProperties");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ControlProperties_ParentPropertyId",
                table: "ControlProperties",
                column: "ParentPropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ControlProperties_ControlProperties_ParentPropertyId",
                table: "ControlProperties",
                column: "ParentPropertyId",
                principalTable: "ControlProperties",
                principalColumn: "Id");
        }
    }
}
