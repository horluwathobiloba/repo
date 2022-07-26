using Microsoft.EntityFrameworkCore.Migrations;

namespace Onyx.ContractService.Infrastructure.Migrations
{
    public partial class UpdateToCraetedByEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "WorkflowPhases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "WorkflowLevels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "VendorTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "SupportingDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ReportValues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ReminderRecipients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ProductServiceTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "PermitTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "PaymentPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "LicenseTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "Inboxes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "Dimensions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "CurrencyConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ContractTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ContractTypeInitiators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ContractTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ContractTaskAssignees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ContractTags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ContractRecipients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ContractRecipientActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ContractDurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ContractDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "ContractComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByEmail",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "WorkflowPhases");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "WorkflowLevels");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "VendorTypes");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "SupportingDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ReportValues");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ReminderRecipients");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ProductServiceTypes");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "PermitTypes");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "PaymentPlans");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "LicenseTypes");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "Inboxes");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "Dimensions");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "CurrencyConfigurations");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ContractTypes");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ContractTypeInitiators");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ContractTasks");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ContractTaskAssignees");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ContractTags");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ContractRecipients");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ContractRecipientActions");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ContractDurations");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ContractDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "ContractComments");

            migrationBuilder.DropColumn(
                name: "CreatedByEmail",
                table: "AuditLogs");
        }
    }
}
