using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class AddedNewColumnsToServiceConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FeeTypeDesc",
                table: "ServiceConfigurations",
                newName: "TransactionFeeTypeDesc");

            migrationBuilder.RenameColumn(
                name: "FeeType",
                table: "ServiceConfigurations",
                newName: "TransactionFeeType");

            migrationBuilder.RenameColumn(
                name: "Fee",
                table: "ServiceConfigurations",
                newName: "TransactionFee");

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionFee",
                table: "ServiceConfigurations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CommissionFeeType",
                table: "ServiceConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CommissionFeeTypeDesc",
                table: "ServiceConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCommissionFee",
                table: "ServiceConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTransactionFee",
                table: "ServiceConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionFee",
                table: "ServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "CommissionFeeType",
                table: "ServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "CommissionFeeTypeDesc",
                table: "ServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "IsCommissionFee",
                table: "ServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "IsTransactionFee",
                table: "ServiceConfigurations");

            migrationBuilder.RenameColumn(
                name: "TransactionFeeTypeDesc",
                table: "ServiceConfigurations",
                newName: "FeeTypeDesc");

            migrationBuilder.RenameColumn(
                name: "TransactionFeeType",
                table: "ServiceConfigurations",
                newName: "FeeType");

            migrationBuilder.RenameColumn(
                name: "TransactionFee",
                table: "ServiceConfigurations",
                newName: "Fee");
        }
    }
}
