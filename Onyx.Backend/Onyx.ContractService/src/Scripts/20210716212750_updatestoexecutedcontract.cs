using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class updatestoexecutedcontract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Attachment",
            //    table: "ContractComments");

            migrationBuilder.AlterColumn<string>(
                name: "CommentById",
                table: "ContractComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CommentById",
                table: "ContractComments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Attachment",
            //    table: "ContractComments",
            //    type: "nvarchar(max)",
            //    nullable: true);
        }
    }
}
