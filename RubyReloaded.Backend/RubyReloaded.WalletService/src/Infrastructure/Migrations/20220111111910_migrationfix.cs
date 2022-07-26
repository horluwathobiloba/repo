using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class migrationfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Accounts_GLSubClassAccountId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_GLSubClassAccountId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GLSubClassAccountId1",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Accounts_ProductId",
            //    table: "Accounts",
            //    column: "ProductId",
            //    unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Products_ProductId",
                table: "Accounts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Products_ProductId",
                table: "Accounts");

            //migrationBuilder.DropIndex(
            //    name: "IX_Accounts_ProductId",
            //    table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "GLSubClassAccountId1",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_GLSubClassAccountId1",
                table: "Products",
                column: "GLSubClassAccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Accounts_GLSubClassAccountId1",
                table: "Products",
                column: "GLSubClassAccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
