using Microsoft.EntityFrameworkCore.Migrations;

namespace OnyxDoc.DocumentService.Infrastructure.Migrations
{
    public partial class updatetodocumentrecipients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Action",
                table: "RecipientActions",
                newName: "RecipientActionDesc");

            migrationBuilder.AddColumn<int>(
                name: "DocumentRecipientAction",
                table: "RecipientActions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentRecipientAction",
                table: "RecipientActions");

            migrationBuilder.RenameColumn(
                name: "RecipientActionDesc",
                table: "RecipientActions",
                newName: "Action");
        }
    }
}
