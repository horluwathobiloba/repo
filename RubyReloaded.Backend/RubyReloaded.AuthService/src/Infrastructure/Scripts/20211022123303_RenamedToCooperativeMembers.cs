using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.AuthService.Infrastructure.Migrations
{
    public partial class RenamedToCooperativeMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CooperativeUserMappings_Cooperatives_CooperativeId",
                table: "CooperativeUserMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CooperativeUserMappings",
                table: "CooperativeUserMappings");

            migrationBuilder.RenameTable(
                name: "CooperativeUserMappings",
                newName: "CooperativeMembers");

            migrationBuilder.RenameIndex(
                name: "IX_CooperativeUserMappings_CooperativeId",
                table: "CooperativeMembers",
                newName: "IX_CooperativeMembers_CooperativeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CooperativeMembers",
                table: "CooperativeMembers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CurrencyConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyCode = table.Column<int>(type: "int", nullable: false),
                    CurrencyCodeString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemOwnerUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemOwnerId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemOwnerUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionFeeType = table.Column<int>(type: "int", nullable: false),
                    CurrencyConfigurationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentChannels_CurrencyConfigurations_CurrencyConfigurationId",
                        column: x => x.CurrencyConfigurationId,
                        principalTable: "CurrencyConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentChannels_CurrencyConfigurationId",
                table: "PaymentChannels",
                column: "CurrencyConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CooperativeMembers_Cooperatives_CooperativeId",
                table: "CooperativeMembers",
                column: "CooperativeId",
                principalTable: "Cooperatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CooperativeMembers_Cooperatives_CooperativeId",
                table: "CooperativeMembers");

            migrationBuilder.DropTable(
                name: "PaymentChannels");

            migrationBuilder.DropTable(
                name: "SystemOwnerUsers");

            migrationBuilder.DropTable(
                name: "CurrencyConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CooperativeMembers",
                table: "CooperativeMembers");

            migrationBuilder.RenameTable(
                name: "CooperativeMembers",
                newName: "CooperativeUserMappings");

            migrationBuilder.RenameIndex(
                name: "IX_CooperativeMembers_CooperativeId",
                table: "CooperativeUserMappings",
                newName: "IX_CooperativeUserMappings_CooperativeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CooperativeUserMappings",
                table: "CooperativeUserMappings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CooperativeUserMappings_Cooperatives_CooperativeId",
                table: "CooperativeUserMappings",
                column: "CooperativeId",
                principalTable: "Cooperatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
