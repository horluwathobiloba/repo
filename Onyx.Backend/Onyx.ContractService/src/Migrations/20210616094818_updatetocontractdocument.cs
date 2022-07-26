using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class updatetocontractdocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ContractDocument_Contracts_ContractId",
            //    table: "ContractDocument");

            migrationBuilder.RenameTable(
                name: "ContractDocument",
                newName: "ContractDocuments");

            migrationBuilder.AddColumn<int>(
              name: "ContractId",
              table: "ContractDocuments",
              type: "int",
              nullable: false,

              defaultValue: 0);
            //migrationBuilder.AlterColumn<int>(
            //    name: "ContractId",
            //    table: "ContractDocuments",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ContractDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ContractDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentSigningUrl",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

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
                name: "Hash",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSigned",
                table: "ContractDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSupportingDocument",
                table: "ContractDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "ContractDocuments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);


            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "ContractDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ContractDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StatusDesc",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractDocuments",
                table: "ContractDocuments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDocuments_ContractId",
                table: "ContractDocuments",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractDocuments_Contracts_ContractId",
                table: "ContractDocuments",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractDocuments_Contracts_ContractId",
                table: "ContractDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractDocuments",
                table: "ContractDocuments");

            migrationBuilder.DropIndex(
                name: "IX_ContractDocuments_ContractId",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentSigningUrl",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "ContractDocuments");

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
                name: "Hash",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "IsSigned",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "IsSupportingDocument",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "ContractDocuments");

            migrationBuilder.RenameTable(
                name: "ContractDocuments",
                newName: "ContractDocument");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "ContractDocument",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractDocument_Contracts_ContractId",
                table: "ContractDocument",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
