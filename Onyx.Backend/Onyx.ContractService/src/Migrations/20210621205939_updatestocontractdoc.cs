using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class updatestocontractdoc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractRecipientAction_ContractRecipients_ContractRecipientId",
                table: "ContractRecipientAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractRecipientAction",
                table: "ContractRecipientAction");

            migrationBuilder.DropColumn(
                name: "IsSupportingDocument",
                table: "ContractDocuments");

            migrationBuilder.RenameTable(
                name: "ContractRecipientAction",
                newName: "ContractRecipientActions");

            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "ContractDocuments",
                newName: "MimeType");

            migrationBuilder.RenameColumn(
                name: "SignerAction",
                table: "ContractRecipientActions",
                newName: "RecipientAction");

            migrationBuilder.RenameIndex(
                name: "IX_ContractRecipientAction_ContractRecipientId",
                table: "ContractRecipientActions",
                newName: "IX_ContractRecipientActions_ContractRecipientId");

            migrationBuilder.AddColumn<string>(
                name: "DocumentSigningUrl",
                table: "ContractRecipients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractRecipientActions",
                table: "ContractRecipientActions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractRecipientActions_ContractRecipients_ContractRecipientId",
                table: "ContractRecipientActions",
                column: "ContractRecipientId",
                principalTable: "ContractRecipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractRecipientActions_ContractRecipients_ContractRecipientId",
                table: "ContractRecipientActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractRecipientActions",
                table: "ContractRecipientActions");

            migrationBuilder.DropColumn(
                name: "DocumentSigningUrl",
                table: "ContractRecipients");

            migrationBuilder.RenameTable(
                name: "ContractRecipientActions",
                newName: "ContractRecipientAction");

            migrationBuilder.RenameColumn(
                name: "MimeType",
                table: "ContractDocuments",
                newName: "DocumentType");

            migrationBuilder.RenameColumn(
                name: "RecipientAction",
                table: "ContractRecipientAction",
                newName: "SignerAction");

            migrationBuilder.RenameIndex(
                name: "IX_ContractRecipientActions_ContractRecipientId",
                table: "ContractRecipientAction",
                newName: "IX_ContractRecipientAction_ContractRecipientId");

            migrationBuilder.AddColumn<bool>(
                name: "IsSupportingDocument",
                table: "ContractDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractRecipientAction",
                table: "ContractRecipientAction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractRecipientAction_ContractRecipients_ContractRecipientId",
                table: "ContractRecipientAction",
                column: "ContractRecipientId",
                principalTable: "ContractRecipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
