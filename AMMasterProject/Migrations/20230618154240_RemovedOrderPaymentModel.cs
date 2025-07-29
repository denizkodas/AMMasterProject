using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class RemovedOrderPaymentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderPayments");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMetaData",
                table: "OrderMasters",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMetaData",
                table: "OrderMasters");

            migrationBuilder.CreateTable(
                name: "OrderPayments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActualAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdminNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BuyerNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ConversionAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConversionCurrency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CustomerAddressGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentReference = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PaymentReferenceFile = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    VendorNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPayments", x => x.PaymentID);
                });
        }
    }
}
