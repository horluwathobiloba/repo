using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class LatestUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignerAction",
                table: "ContractRecipients");

            migrationBuilder.RenameColumn(
                name: "SigningUrl",
                table: "ContractRecipients",
                newName: "RecipientCategory");

            migrationBuilder.CreateTable(
                name: "ContractRecipientAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    ContractRecipientId = table.Column<int>(type: "int", nullable: false),
                    SignerAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppSigningUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignedDocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_ContractRecipientAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractRecipientAction_ContractRecipients_ContractRecipientId",
                        column: x => x.ContractRecipientId,
                        principalTable: "ContractRecipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContractRecipientAction_ContractRecipientId",
                table: "ContractRecipientAction",
                column: "ContractRecipientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractRecipientAction");

            migrationBuilder.RenameColumn(
                name: "RecipientCategory",
                table: "ContractRecipients",
                newName: "SigningUrl");

            migrationBuilder.AddColumn<int>(
                name: "SignerAction",
                table: "ContractRecipients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
