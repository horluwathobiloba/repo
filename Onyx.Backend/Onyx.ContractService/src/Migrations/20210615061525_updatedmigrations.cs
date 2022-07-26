using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class updatedmigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Contracts_ContractTypes_ContractTypeId",
            //    table: "Contracts");

            //migrationBuilder.DropIndex(
            //    name: "IX_Contracts_ContractTypeId",
            //    table: "Contracts");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "RoleId",
            //    table: "Contracts",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "RoleName",
            //    table: "Contracts",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxId",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorTypeId",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "Contracts");

            //migrationBuilder.DropColumn(
            //    name: "RoleId",
            //    table: "Contracts");

            //migrationBuilder.DropColumn(
            //    name: "RoleName",
            //    table: "Contracts");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "VendorTypeId",
                table: "Contracts");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Contracts_ContractTypeId",
            //    table: "Contracts",
            //    column: "ContractTypeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Contracts_ContractTypes_ContractTypeId",
            //    table: "Contracts",
            //    column: "ContractTypeId",
            //    principalTable: "ContractTypes",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
