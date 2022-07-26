using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class additionofuserid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WishlistFundingSourceId",
                table: "Wishlists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "VirtualAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductSettlementAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TransactionService",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductItemCategory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductInterestPeriod",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductInterest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductFundingSources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PaymentGatewayServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PaymentChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Currencies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BankServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "WishlistFundingSource",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FundingSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FundingSourceCategory = table.Column<int>(type: "int", nullable: false),
            //        PaymentChannelId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_WishlistFundingSource", x => x.Id);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Wishlists_WishlistFundingSourceId",
            //    table: "Wishlists",
            //    column: "WishlistFundingSourceId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Wishlists_WishlistFundingSource_WishlistFundingSourceId",
            //    table: "Wishlists",
            //    column: "WishlistFundingSourceId",
            //    principalTable: "WishlistFundingSource",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        //    migrationBuilder.DropForeignKey(
        //        name: "FK_Wishlists_WishlistFundingSource_WishlistFundingSourceId",
        //        table: "Wishlists");

            //migrationBuilder.DropTable(
            //    name: "WishlistFundingSource");

            //migrationBuilder.DropIndex(
            //    name: "IX_Wishlists_WishlistFundingSourceId",
            //    table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "VirtualAccounts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductSettlementAccounts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TransactionService");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductItemCategory");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductInterestPeriod");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductInterest");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductFundingSources");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PaymentGatewayServices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PaymentChannels");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BankServices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Banks");

            migrationBuilder.AlterColumn<int>(
                name: "WishlistFundingSourceId",
                table: "Wishlists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
