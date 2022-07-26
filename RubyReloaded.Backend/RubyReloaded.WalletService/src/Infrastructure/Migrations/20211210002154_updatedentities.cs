using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RubyReloaded.WalletService.Infrastructure.Migrations
{
    public partial class updatedentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionFeeType",
                table: "PaymentChannels",
                newName: "PaymentGatewayFeeType");

            migrationBuilder.RenameColumn(
                name: "TransactionFee",
                table: "PaymentChannels",
                newName: "PaymentGatewayFee");

            migrationBuilder.RenameColumn(
                name: "PaymentChannelCategoryDesc",
                table: "PaymentChannels",
                newName: "PaymentGatewayFeeTypeDesc");

            migrationBuilder.RenameColumn(
                name: "PaymentChannelCategory",
                table: "PaymentChannels",
                newName: "PaymentGatewayCategory");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "PaymentChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentGatewayCategoryDesc",
                table: "PaymentChannels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountStatus = table.Column<int>(type: "int", nullable: false),
                    AccountStatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParentAccountId = table.Column<int>(type: "int", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitBalanceLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditBalanceLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountFreezeType = table.Column<int>(type: "int", nullable: false),
                    AccountClass = table.Column<int>(type: "int", nullable: false),
                    AccountPrefix = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankServiceCategory = table.Column<int>(type: "int", nullable: false),
                    MinimumTransactionLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumTransactionLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionFeeType = table.Column<int>(type: "int", nullable: false),
                    CommissionFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CommissionFeeType = table.Column<int>(type: "int", nullable: false),
                    PaymentChannelId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankServices_PaymentChannels_PaymentChannelId",
                        column: x => x.PaymentChannelId,
                        principalTable: "PaymentChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentGatewayServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentGatewayServiceCategory = table.Column<int>(type: "int", nullable: false),
                    PaymentChannelId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentGatewayServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentGatewayServices_PaymentChannels_PaymentChannelId",
                        column: x => x.PaymentChannelId,
                        principalTable: "PaymentChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WishCategory = table.Column<int>(type: "int", nullable: false),
                    ContributionFrequency = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    WishListStatus = table.Column<int>(type: "int", nullable: false),
                    WishListStatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WishlistBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WishlistFundingSourceId = table.Column<int>(type: "int", nullable: false),
                    WishlistSavingsPeriod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WishlistExtensionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WishlistExtensionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WishlistExtensionFrequency = table.Column<int>(type: "int", nullable: false),
                    DaysLeft = table.Column<int>(type: "int", nullable: false),
                    RecurringContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinimumFundingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumFundingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumBalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumBalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumWithdrawalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumWithdrawalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AllowCustomerOverrideAmount = table.Column<bool>(type: "bit", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductCategory = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumHoldingPeriod = table.Column<int>(type: "int", nullable: false),
                    MinimumHoldingDuration = table.Column<int>(type: "int", nullable: false),
                    MaximumHoldingPeriod = table.Column<int>(type: "int", nullable: false),
                    MaximumHoldingDuration = table.Column<int>(type: "int", nullable: false),
                    GLSubClassAccountId1 = table.Column<int>(type: "int", nullable: true),
                    GLSubClassAccountId = table.Column<int>(type: "int", nullable: false),
                    ComputeCommissionFee = table.Column<bool>(type: "bit", nullable: false),
                    ComputeTransactionFee = table.Column<bool>(type: "bit", nullable: false),
                    ComputeInterest = table.Column<bool>(type: "bit", nullable: false),
                    AllowPenaltyCharges = table.Column<bool>(type: "bit", nullable: false),
                    PenaltyFeeType = table.Column<int>(type: "int", nullable: false),
                    PenaltyFeeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Accounts_GLSubClassAccountId1",
                        column: x => x.GLSubClassAccountId1,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductFundingSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductFundingCategory = table.Column<int>(type: "int", nullable: false),
                    PaymentChannelId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFundingSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFundingSources_Payments_PaymentChannelId",
                        column: x => x.PaymentChannelId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductFundingSources_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInterest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinimumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InterestType = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    VariableType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInterest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInterest_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductItemCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefaultImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItemCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductItemCategory_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionServiceCategory = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionService_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSettlementAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSettlementAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSettlementAccounts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInterestPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductInterestId = table.Column<int>(type: "int", nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInterestPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInterestPeriod_ProductInterest_ProductInterestId",
                        column: x => x.ProductInterestId,
                        principalTable: "ProductInterest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankServices_PaymentChannelId",
                table: "BankServices",
                column: "PaymentChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentGatewayServices_PaymentChannelId",
                table: "PaymentGatewayServices",
                column: "PaymentChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFundingSources_PaymentChannelId",
                table: "ProductFundingSources",
                column: "PaymentChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFundingSources_ProductId",
                table: "ProductFundingSources",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInterest_ProductId",
                table: "ProductInterest",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInterestPeriod_ProductInterestId",
                table: "ProductInterestPeriod",
                column: "ProductInterestId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItemCategory_ProductId",
                table: "ProductItemCategory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_GLSubClassAccountId1",
                table: "Products",
                column: "GLSubClassAccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionService_ProductId",
                table: "TransactionService",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSettlementAccounts_ProductId",
                table: "ProductSettlementAccounts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankServices");

            migrationBuilder.DropTable(
                name: "PaymentGatewayServices");

            migrationBuilder.DropTable(
                name: "ProductFundingSources");

            migrationBuilder.DropTable(
                name: "ProductInterestPeriod");

            migrationBuilder.DropTable(
                name: "ProductItemCategory");

            migrationBuilder.DropTable(
                name: "TransactionService");

            migrationBuilder.DropTable(
                name: "ProductSettlementAccounts");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropTable(
                name: "ProductInterest");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "PaymentChannels");

            migrationBuilder.DropColumn(
                name: "PaymentGatewayCategoryDesc",
                table: "PaymentChannels");

            migrationBuilder.RenameColumn(
                name: "PaymentGatewayFeeTypeDesc",
                table: "PaymentChannels",
                newName: "PaymentChannelCategoryDesc");

            migrationBuilder.RenameColumn(
                name: "PaymentGatewayFeeType",
                table: "PaymentChannels",
                newName: "TransactionFeeType");

            migrationBuilder.RenameColumn(
                name: "PaymentGatewayFee",
                table: "PaymentChannels",
                newName: "TransactionFee");

            migrationBuilder.RenameColumn(
                name: "PaymentGatewayCategory",
                table: "PaymentChannels",
                newName: "PaymentChannelCategory");
        }
    }
}
