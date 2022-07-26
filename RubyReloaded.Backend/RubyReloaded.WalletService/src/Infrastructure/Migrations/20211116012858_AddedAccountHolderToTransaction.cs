using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class AddedAccountHolderToTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategoryAndServicesMap_ProductCategoryConfigurations_ProductCategoryConfigurationId",
                table: "ProductCategoryAndServicesMap");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategoryAndServicesMap_ProductCategoryConfigurationId",
                table: "ProductCategoryAndServicesMap");

            migrationBuilder.DropColumn(
                name: "ProductCategoryConfigurationId",
                table: "ProductCategoryAndServicesMap");

            migrationBuilder.RenameColumn(
                name: "ProductCategory",
                table: "ProductCategoryConfigurations",
                newName: "ProductCategoryProperty");

            migrationBuilder.AddColumn<string>(
                name: "AccountHolder",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ServiceConfigurations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<int>(type: "int", nullable: false),
                    IsPayoutBank = table.Column<bool>(type: "bit", nullable: false),
                    IsVirtualBank = table.Column<bool>(type: "bit", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VirtualAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    AccountNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VirtualAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VirtualAccounts_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_PaymentChannelId",
                table: "WalletTransactions",
                column: "PaymentChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceConfigurations_ProductId",
                table: "ServiceConfigurations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryAndServicesMap_ProductCategoryId",
                table: "ProductCategoryAndServicesMap",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualAccounts_BankId",
                table: "VirtualAccounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualAccounts_WalletId",
                table: "VirtualAccounts",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategoryAndServicesMap_ProductCategoryConfigurations_ProductCategoryId",
                table: "ProductCategoryAndServicesMap",
                column: "ProductCategoryId",
                principalTable: "ProductCategoryConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceConfigurations_ProductConfigurations_ProductId",
                table: "ServiceConfigurations",
                column: "ProductId",
                principalTable: "ProductConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletTransactions_PaymentChannels_PaymentChannelId",
                table: "WalletTransactions",
                column: "PaymentChannelId",
                principalTable: "PaymentChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategoryAndServicesMap_ProductCategoryConfigurations_ProductCategoryId",
                table: "ProductCategoryAndServicesMap");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceConfigurations_ProductConfigurations_ProductId",
                table: "ServiceConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletTransactions_PaymentChannels_PaymentChannelId",
                table: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "VirtualAccounts");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_PaymentChannelId",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ServiceConfigurations_ProductId",
                table: "ServiceConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategoryAndServicesMap_ProductCategoryId",
                table: "ProductCategoryAndServicesMap");

            migrationBuilder.DropColumn(
                name: "AccountHolder",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ServiceConfigurations");

            migrationBuilder.RenameColumn(
                name: "ProductCategoryProperty",
                table: "ProductCategoryConfigurations",
                newName: "ProductCategory");

            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryConfigurationId",
                table: "ProductCategoryAndServicesMap",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryAndServicesMap_ProductCategoryConfigurationId",
                table: "ProductCategoryAndServicesMap",
                column: "ProductCategoryConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategoryAndServicesMap_ProductCategoryConfigurations_ProductCategoryConfigurationId",
                table: "ProductCategoryAndServicesMap",
                column: "ProductCategoryConfigurationId",
                principalTable: "ProductCategoryConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
