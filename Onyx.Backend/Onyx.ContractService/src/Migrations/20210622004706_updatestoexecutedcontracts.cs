using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class updatestoexecutedcontracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Signature",
                table: "Contracts",
                newName: "NextActorRank");

            migrationBuilder.AddColumn<string>(
                name: "ExecutedContract",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExecutedContractFileExtension",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExecutedContractMimeType",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InitiatorSignature",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InitiatorSignatureFileExtension",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InitiatorSignatureMimeType",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextActorAction",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextActorEmail",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproverSignatureBlobFileUrl",
                table: "ContractRecipientActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproverSignatureFileExtension",
                table: "ContractRecipientActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproverSignatureMimeType",
                table: "ContractRecipientActions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutedContract",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ExecutedContractFileExtension",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ExecutedContractMimeType",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "InitiatorSignature",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "InitiatorSignatureFileExtension",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "InitiatorSignatureMimeType",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "NextActorAction",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "NextActorEmail",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ApproverSignatureBlobFileUrl",
                table: "ContractRecipientActions");

            migrationBuilder.DropColumn(
                name: "ApproverSignatureFileExtension",
                table: "ContractRecipientActions");

            migrationBuilder.DropColumn(
                name: "ApproverSignatureMimeType",
                table: "ContractRecipientActions");

            migrationBuilder.RenameColumn(
                name: "NextActorRank",
                table: "Contracts",
                newName: "Signature");
        }
    }
}
