using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class Contractcheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractDuration",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Attachment",
                table: "ContractComments");

            migrationBuilder.AddColumn<int>(
                name: "ContractDurationId",
                table: "Contracts",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommentById",
                table: "ContractComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ContractDurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    DurationFrequency = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    OrganisationName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractDurations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractDurationId",
                table: "Contracts",
                column: "ContractDurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ContractDurations_ContractDurationId",
                table: "Contracts",
                column: "ContractDurationId",
                principalTable: "ContractDurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ContractDurations_ContractDurationId",
                table: "Contracts");

            migrationBuilder.DropTable(
                name: "ContractDurations");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContractDurationId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractDurationId",
                table: "Contracts");

            migrationBuilder.AddColumn<int>(
                name: "ContractDuration",
                table: "Contracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CommentById",
                table: "ContractComments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Attachment",
                table: "ContractComments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
