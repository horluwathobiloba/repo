using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.AuthService.Infrastructure.Migrations
{
    public partial class UpdatesToNotificationSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotificationStatus",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationStatus",
                table: "AspNetUsers");
        }
    }
}
