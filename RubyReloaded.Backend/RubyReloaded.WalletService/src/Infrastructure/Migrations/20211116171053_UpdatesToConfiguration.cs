using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class UpdatesToConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentChannels_CurrencyConfigurations_CurrencyConfigurationId",
                table: "PaymentChannels");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceConfigurations_ProductConfigurations_ProductId",
                table: "ServiceConfigurations");

            migrationBuilder.DropTable(
                name: "BankConfigurations");

            migrationBuilder.DropTable(
                name: "CurrencyConfigurations");

            migrationBuilder.DropTable(
                name: "ProductCategoryAndServicesMap");

            migrationBuilder.DropTable(
                name: "VirtualAccountConfigurations");

            migrationBuilder.DropTable(
                name: "ProductCategoryConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceConfigurations",
                table: "ServiceConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductConfigurations",
                table: "ProductConfigurations");

            migrationBuilder.RenameTable(
                name: "ServiceConfigurations",
                newName: "Services");

            migrationBuilder.RenameTable(
                name: "ProductConfigurations",
                newName: "Products");

            migrationBuilder.RenameColumn(
                name: "CurrencyConfigurationId",
                table: "PaymentChannels",
                newName: "CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentChannels_CurrencyConfigurationId",
                table: "PaymentChannels",
                newName: "IX_PaymentChannels_CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceConfigurations_ProductId",
                table: "Services",
                newName: "IX_Services_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyCode = table.Column<int>(type: "int", nullable: false),
                    CurrencyCodeString = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentChannels_Currencies_CurrencyId",
                table: "PaymentChannels",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Products_ProductId",
                table: "Services",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentChannels_Currencies_CurrencyId",
                table: "PaymentChannels");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Products_ProductId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "ServiceConfigurations");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "ProductConfigurations");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "PaymentChannels",
                newName: "CurrencyConfigurationId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentChannels_CurrencyId",
                table: "PaymentChannels",
                newName: "IX_PaymentChannels_CurrencyConfigurationId");

            migrationBuilder.RenameIndex(
                name: "IX_Services_ProductId",
                table: "ServiceConfigurations",
                newName: "IX_ServiceConfigurations_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceConfigurations",
                table: "ServiceConfigurations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductConfigurations",
                table: "ProductConfigurations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BankConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyCode = table.Column<int>(type: "int", nullable: false),
                    CurrencyCodeString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCategoryDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCategoryProperty = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VirtualAccountConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SettlementAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirtualAccountConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryAndServicesMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCategory = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    ServiceConfigurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryAndServicesMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategoryAndServicesMap_ProductCategoryConfigurations_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategoryConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategoryAndServicesMap_ServiceConfigurations_ServiceConfigurationId",
                        column: x => x.ServiceConfigurationId,
                        principalTable: "ServiceConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryAndServicesMap_ProductCategoryId",
                table: "ProductCategoryAndServicesMap",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryAndServicesMap_ServiceConfigurationId",
                table: "ProductCategoryAndServicesMap",
                column: "ServiceConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentChannels_CurrencyConfigurations_CurrencyConfigurationId",
                table: "PaymentChannels",
                column: "CurrencyConfigurationId",
                principalTable: "CurrencyConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceConfigurations_ProductConfigurations_ProductId",
                table: "ServiceConfigurations",
                column: "ProductId",
                principalTable: "ProductConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
