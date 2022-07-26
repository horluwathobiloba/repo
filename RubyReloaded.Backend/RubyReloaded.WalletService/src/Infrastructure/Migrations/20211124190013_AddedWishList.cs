using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class AddedWishList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionFeeTypeDesc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsCommissionFee",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductType",
                table: "Products",
                newName: "ProductCategory");

            migrationBuilder.RenameColumn(
                name: "ProductCategoryId",
                table: "Products",
                newName: "GLSubClassAccountId");

            migrationBuilder.RenameColumn(
                name: "CommissionFeeType",
                table: "Products",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "CommissionFee",
                table: "Products",
                newName: "MinHoldingPeriod");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxHoldingPeriod",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "WithdrawalSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_WithdrawalSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WithdrawalSettings");

            migrationBuilder.DropColumn(
                name: "MaxHoldingPeriod",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductCategory",
                table: "Products",
                newName: "ProductType");

            migrationBuilder.RenameColumn(
                name: "MinHoldingPeriod",
                table: "Products",
                newName: "CommissionFee");

            migrationBuilder.RenameColumn(
                name: "GLSubClassAccountId",
                table: "Products",
                newName: "ProductCategoryId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Products",
                newName: "CommissionFeeType");

            migrationBuilder.AddColumn<string>(
                name: "CommissionFeeTypeDesc",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCommissionFee",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
