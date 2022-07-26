using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class RemovalOfOrganisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "VirtualAccountConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "VirtualAccountConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "ServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "ServiceConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "ProductConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "ProductConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "ProductCategoryConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "ProductCategoryConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "PaymentChannels");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "PaymentChannels");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "CurrencyConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "CurrencyConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "CardAuthorizations");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "CardAuthorizations");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "BankConfigurations");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "BankConfigurations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "WalletTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "Wallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "Wallets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "VirtualAccountConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "VirtualAccountConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "ServiceConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "ServiceConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "ProductConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "ProductConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "ProductCategoryConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "ProductCategoryConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "PaymentChannels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "PaymentChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "CurrencyConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "CurrencyConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "CardAuthorizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "CardAuthorizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganisationId",
                table: "BankConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "BankConfigurations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
