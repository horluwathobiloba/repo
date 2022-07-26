using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class contractupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxId",
                table: "Vendors",
                newName: "SupplierCode");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "Vendors",
                newName: "SupplierClass");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Vendors",
                newName: "ShortName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupplierCode",
                table: "Vendors",
                newName: "TaxId");

            migrationBuilder.RenameColumn(
                name: "SupplierClass",
                table: "Vendors",
                newName: "RegistrationNumber");

            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "Vendors",
                newName: "Address");
        }
    }
}
