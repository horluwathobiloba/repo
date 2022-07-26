﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class addisexecuteddocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnExecutedDocument",
                table: "Contracts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnExecutedDocument",
                table: "Contracts");
        }
    }
}
