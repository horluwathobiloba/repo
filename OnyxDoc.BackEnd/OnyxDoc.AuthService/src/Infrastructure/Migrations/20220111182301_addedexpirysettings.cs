using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.AuthService.Infrastructure.Migrations
{
    public partial class addedexpirysettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpiryPeriod_SystemSettings_SystemSettingId",
                table: "ExpiryPeriod");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpiryPeriod",
                table: "ExpiryPeriod");

            migrationBuilder.RenameTable(
                name: "ExpiryPeriod",
                newName: "ExpiryPeriods");

            migrationBuilder.RenameIndex(
                name: "IX_ExpiryPeriod_SystemSettingId",
                table: "ExpiryPeriods",
                newName: "IX_ExpiryPeriods_SystemSettingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpiryPeriods",
                table: "ExpiryPeriods",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpiryPeriods_SystemSettings_SystemSettingId",
                table: "ExpiryPeriods",
                column: "SystemSettingId",
                principalTable: "SystemSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpiryPeriods_SystemSettings_SystemSettingId",
                table: "ExpiryPeriods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpiryPeriods",
                table: "ExpiryPeriods");

            migrationBuilder.RenameTable(
                name: "ExpiryPeriods",
                newName: "ExpiryPeriod");

            migrationBuilder.RenameIndex(
                name: "IX_ExpiryPeriods_SystemSettingId",
                table: "ExpiryPeriod",
                newName: "IX_ExpiryPeriod_SystemSettingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpiryPeriod",
                table: "ExpiryPeriod",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpiryPeriod_SystemSettings_SystemSettingId",
                table: "ExpiryPeriod",
                column: "SystemSettingId",
                principalTable: "SystemSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
