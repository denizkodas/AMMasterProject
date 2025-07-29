using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class OrderMasterOrderPaymentModelRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualCurrency",
                table: "OrderPayments");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "UserMemberships",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemMetaData",
                table: "UserMemberships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "OrderPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentReferenceFile",
                table: "OrderPayments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "UserMemberships");

            migrationBuilder.DropColumn(
                name: "ItemMetaData",
                table: "UserMemberships");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "OrderPayments");

            migrationBuilder.DropColumn(
                name: "PaymentReferenceFile",
                table: "OrderPayments");

            migrationBuilder.AddColumn<string>(
                name: "ActualCurrency",
                table: "OrderPayments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
