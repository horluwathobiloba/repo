using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.AuthService.Infrastructure.Migrations
{
    public partial class updatedblobservice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkFlowUserCategory",
                table: "Roles",
                newName: "WorkflowUserCategory");

            //migrationBuilder.AddColumn<string>(
            //    name: "CreatedById",
            //    table: "AspNetUsers",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "CreatedById",
            //    table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "WorkflowUserCategory",
                table: "Roles",
                newName: "WorkFlowUserCategory");
        }
    }
}
