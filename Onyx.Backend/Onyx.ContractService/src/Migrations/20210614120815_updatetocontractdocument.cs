using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class updatetocontractdocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "UploadedById",
            //    table: "ContractDocuments");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ContractDocuments",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "ContractDocuments",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsSupportingDocument",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignedDocumentUrl",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "File",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "IsSupportingDocument",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "SignedDocumentUrl",
                table: "ContractDocuments");

            migrationBuilder.RenameColumn(
                name: "Version",
                table: "ContractDocuments",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ContractDocuments",
                newName: "ImagePath");

            //migrationBuilder.AddColumn<int>(
            //    name: "UploadedById",
            //    table: "ContractDocuments",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
        }
    }
}
