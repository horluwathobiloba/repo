using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class addedjobfunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobFunctionId",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "JobFunctionName",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Contracts_ContractTypeId",
            //    table: "Contracts",
            //    column: "ContractTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Contracts_PermitTypeId",
            //    table: "Contracts",
            //    column: "PermitTypeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Contracts_ContractTypes_ContractTypeId",
            //    table: "Contracts",
            //    column: "ContractTypeId",
            //    principalTable: "ContractTypes",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Contracts_PermitTypes_PermitTypeId",
            //    table: "Contracts",
            //    column: "PermitTypeId",
            //    principalTable: "PermitTypes",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Contracts_ContractTypes_ContractTypeId",
            //    table: "Contracts");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Contracts_PermitTypes_PermitTypeId",
            //    table: "Contracts");

            //migrationBuilder.DropIndex(
            //    name: "IX_Contracts_ContractTypeId",
            //    table: "Contracts");

            //migrationBuilder.DropIndex(
            //    name: "IX_Contracts_PermitTypeId",
            //    table: "Contracts");

            migrationBuilder.DropColumn(
                name: "JobFunctionId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "JobFunctionName",
                table: "Contracts");
        }
    }
}
