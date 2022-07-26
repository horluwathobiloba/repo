using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class ProductFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Product_Accounts_GLSubClassAccountId1",
            //    table: "Products");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductFundingSources_Product_ProductId",
            //    table: "ProductFundingSources");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductInterest_Product_ProductId",
            //    table: "ProductInterest");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductItemCategory_Product_ProductId",
            //    table: "ProductItemCategory");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductSettlementAccounts_Product_ProductId",
            //    table: "ProductSettlementAccounts");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_TransactionService_Product_ProductId",
            //    table: "TransactionService");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Product_TempId",
            //    table: "Products");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Product_TempId1",
            //    table: "Products");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Product_TempId2",
            //    table: "Products");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Product_TempId3",
            //    table: "Products");

            //migrationBuilder.DropUniqueConstraint(
            //    name: "AK_Product_TempId4",
            //    table: "Products");

            //migrationBuilder.RenameTable(
            //    name: "Products",
            //    newName: "Products");

            //migrationBuilder.RenameColumn(
            //    name: "TempId4",
            //    table: "Products",
            //    newName: "TransactionFeeType");

            //migrationBuilder.RenameColumn(
            //    name: "TempId3",
            //    table: "Products",
            //    newName: "Status");

            //migrationBuilder.RenameColumn(
            //    name: "TempId2",
            //    table: "Products",
            //    newName: "ProductCategory");

            //migrationBuilder.RenameColumn(
            //    name: "TempId1",
            //    table: "Products",
            //    newName: "PenaltyFeeType");

            //migrationBuilder.RenameColumn(
            //    name: "TempId",
            //    table: "Products",
            //    newName: "MinimumHoldingPeriod");

            //migrationBuilder.AddColumn<int>(
            //    name: "Id",
            //    table: "Products",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0)
            //    .Annotation("SqlServer:Identity", "1, 1");

            

        migrationBuilder.AddColumn<decimal>(
                name: "CommissionAmountOrRate",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CommissionFeeType",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CommissionFeeTypeDesc",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CreatedBy",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CreatedByEmail",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "CreatedDate",
            //    table: "Products",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddColumn<string>(
            //    name: "Currency",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Description",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "DeviceId",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableCommission",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableCustomerOverrideAmount",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableInterest",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnablePenaltyCharges",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableTransactionFee",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AddColumn<int>(
            //    name: "GLSubClassAccountId",
            //    table: "Products",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "LastModifiedBy",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "LastModifiedDate",
            //    table: "Products",
            //    type: "datetime2",
            //    nullable: true);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "MaximumBalanceAmount",
            //    table: "Products",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "MaximumFundingAmount",
            //    table: "Products",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<int>(
            //    name: "MaximumHoldingDuration",
            //    table: "Products",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "MaximumHoldingPeriod",
            //    table: "Products",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "MaximumWithdrawalAmount",
            //    table: "Products",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "MinimumBalanceAmount",
            //    table: "Products",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "MinimumFundingAmount",
            //    table: "Products",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<int>(
            //    name: "MinimumHoldingDuration",
            //    table: "Products",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "MinimumWithdrawalAmount",
            //    table: "Products",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<string>(
            //    name: "Name",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "PenaltyFeeRate",
            //    table: "Products",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m);

            //migrationBuilder.AddColumn<string>(
            //    name: "ProductCategoryDesc",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "StatusDesc",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "TermsAndConditions",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<decimal>(
            //    name: "TransactionFee",
            //    table: "Products",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransactionFeeAmountOrRate",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TransactionFeeTypeDesc",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "UserId",
            //    table: "Products",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Products",
            //    table: "Products",
            //    column: "Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Products_GLSubClassAccountId1",
            //    table: "Products",
            //    column: "GLSubClassAccountId1");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductFundingSources_Products_ProductId",
            //    table: "ProductFundingSources",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductInterest_Products_ProductId",
            //    table: "ProductInterest",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductItemCategory_Products_ProductId",
            //    table: "ProductItemCategory",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Products_Accounts_GLSubClassAccountId1",
            //    table: "Products",
            //    column: "GLSubClassAccountId1",
            //    principalTable: "Accounts",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProductSettlementAccounts_Products_ProductId",
            //    table: "ProductSettlementAccounts",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_TransactionService_Products_ProductId",
            //    table: "TransactionService",
            //    column: "ProductId",
            //    principalTable: "Products",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFundingSources_Products_ProductId",
                table: "ProductFundingSources");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInterest_Products_ProductId",
                table: "ProductInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductItemCategory_Products_ProductId",
                table: "ProductItemCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Accounts_GLSubClassAccountId1",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSettlementAccounts_Products_ProductId",
                table: "ProductSettlementAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionService_Products_ProductId",
                table: "TransactionService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_GLSubClassAccountId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CommissionAmountOrRate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CommissionFeeType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CommissionFeeTypeDesc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EnableCommission",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EnableCustomerOverrideAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EnableInterest",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EnablePenaltyCharges",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EnableTransactionFee",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GLSubClassAccountId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaximumBalanceAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaximumFundingAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaximumHoldingDuration",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaximumHoldingPeriod",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaximumWithdrawalAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinimumBalanceAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinimumFundingAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinimumHoldingDuration",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinimumWithdrawalAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PenaltyFeeRate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductCategoryDesc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StatusDesc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TermsAndConditions",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TransactionFee",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TransactionFeeAmountOrRate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TransactionFeeTypeDesc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products");

            migrationBuilder.RenameColumn(
                name: "TransactionFeeType",
                table: "Products",
                newName: "TempId4");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Products",
                newName: "TempId3");

            migrationBuilder.RenameColumn(
                name: "ProductCategory",
                table: "Products",
                newName: "TempId2");

            migrationBuilder.RenameColumn(
                name: "PenaltyFeeType",
                table: "Products",
                newName: "TempId1");

            migrationBuilder.RenameColumn(
                name: "MinimumHoldingPeriod",
                table: "Products",
                newName: "TempId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Product_TempId",
                table: "Products",
                column: "TempId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Product_TempId1",
                table: "Products",
                column: "TempId1");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Product_TempId2",
                table: "Products",
                column: "TempId2");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Product_TempId3",
                table: "Products",
                column: "TempId3");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Product_TempId4",
                table: "Products",
                column: "TempId4");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Accounts_GLSubClassAccountId1",
                table: "Products",
                column: "GLSubClassAccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFundingSources_Product_ProductId",
                table: "ProductFundingSources",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInterest_Product_ProductId",
                table: "ProductInterest",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "TempId1",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItemCategory_Product_ProductId",
                table: "ProductItemCategory",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "TempId2",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSettlementAccounts_Product_ProductId",
                table: "ProductSettlementAccounts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "TempId3",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionService_Product_ProductId",
                table: "TransactionService",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "TempId4",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
