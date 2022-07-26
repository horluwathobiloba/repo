using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class updatesToContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxId",
                table: "Contracts",
                newName: "SupplierCode");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "Contracts",
                newName: "SupplierClass");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Contracts",
                newName: "ShortName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupplierCode",
                table: "Contracts",
                newName: "TaxId");

            migrationBuilder.RenameColumn(
                name: "SupplierClass",
                table: "Contracts",
                newName: "RegistrationNumber");

            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "Contracts",
                newName: "Address");
        }
    }
}
