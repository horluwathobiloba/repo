using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class AddedToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Products_ProductId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ProductId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "MaxAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TransactionFeeTypeDesc",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "TransactionFeeType",
                table: "Products",
                newName: "PenaltyFeeType");

            migrationBuilder.RenameColumn(
                name: "MinHoldingPeriod",
                table: "Products",
                newName: "PenaltyFeeRate");

            migrationBuilder.RenameColumn(
                name: "MinAmount",
                table: "Products",
                newName: "MinimumAmount");

            migrationBuilder.RenameColumn(
                name: "MaxHoldingPeriod",
                table: "Products",
                newName: "MaximumAmount");

            migrationBuilder.RenameColumn(
                name: "IsTransactionFee",
                table: "Products",
                newName: "ComputeTransactionFee");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Products",
                newName: "MinimumHoldingPeriod");

            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "Accounts",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "ProductCategory",
                table: "Accounts",
                newName: "AccountStatusDesc");

            migrationBuilder.RenameColumn(
                name: "AccountCode",
                table: "Accounts",
                newName: "AccountNumber");

            migrationBuilder.AddColumn<bool>(
                name: "AllowPenaltyCharges",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ComputeCommissionFee",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ComputeInterest",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GLSubClassAccountId1",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumHoldingDuration",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaximumHoldingPeriod",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumHoldingDuration",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AccountType",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountClass",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountFreezeType",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountPrefix",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountStatus",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ClosingBalance",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CreditBalanceLimit",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "DebitBalanceLimit",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OpeningBalance",
                table: "Accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ParentAccountId",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductInterestPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ProductInterestPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_TransactionService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionService_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductInterest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinimumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InterestType = table.Column<int>(type: "int", nullable: false),
                    ProductInterestPeriodId = table.Column<int>(type: "int", nullable: false),
                    VariableType = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_ProductInterest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInterest_ProductInterestPeriod_ProductInterestPeriodId",
                        column: x => x.ProductInterestPeriodId,
                        principalTable: "ProductInterestPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductInterest_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_GLSubClassAccountId1",
                table: "Products",
                column: "GLSubClassAccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInterest_ProductId",
                table: "ProductInterest",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInterest_ProductInterestPeriodId",
                table: "ProductInterest",
                column: "ProductInterestPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionService_ProductId",
                table: "TransactionService",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Accounts_GLSubClassAccountId1",
                table: "Products",
                column: "GLSubClassAccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Accounts_GLSubClassAccountId1",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductInterest");

            migrationBuilder.DropTable(
                name: "TransactionService");

            migrationBuilder.DropTable(
                name: "ProductInterestPeriod");

            migrationBuilder.DropIndex(
                name: "IX_Products_GLSubClassAccountId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AllowPenaltyCharges",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ComputeCommissionFee",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ComputeInterest",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GLSubClassAccountId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaximumHoldingDuration",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaximumHoldingPeriod",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinimumHoldingDuration",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AccountClass",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountFreezeType",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountPrefix",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ClosingBalance",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CreditBalanceLimit",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "DebitBalanceLimit",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "OpeningBalance",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ParentAccountId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "PenaltyFeeType",
                table: "Products",
                newName: "TransactionFeeType");

            migrationBuilder.RenameColumn(
                name: "PenaltyFeeRate",
                table: "Products",
                newName: "MinHoldingPeriod");

            migrationBuilder.RenameColumn(
                name: "MinimumHoldingPeriod",
                table: "Products",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "MinimumAmount",
                table: "Products",
                newName: "MinAmount");

            migrationBuilder.RenameColumn(
                name: "MaximumAmount",
                table: "Products",
                newName: "MaxHoldingPeriod");

            migrationBuilder.RenameColumn(
                name: "ComputeTransactionFee",
                table: "Products",
                newName: "IsTransactionFee");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Accounts",
                newName: "ShortName");

            migrationBuilder.RenameColumn(
                name: "AccountStatusDesc",
                table: "Accounts",
                newName: "ProductCategory");

            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "Accounts",
                newName: "AccountCode");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxAmount",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TransactionFeeTypeDesc",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ProductId",
                table: "Services",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Products_ProductId",
                table: "Services",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
