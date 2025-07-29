using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class UserMembershipModelRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "UserMemberships");

            migrationBuilder.DropColumn(
                name: "CreditAmount",
                table: "UserMemberships");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "UserMemberships");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "UserMemberships");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "UserMemberships");

            migrationBuilder.DropColumn(
                name: "PaymentReferenceFile",
                table: "UserMemberships");

            migrationBuilder.DropColumn(
                name: "PaymentReferencekey",
                table: "UserMemberships");

            migrationBuilder.DropColumn(
                name: "RevenueCreditID",
                table: "UserMemberships");

            migrationBuilder.DropColumn(
                name: "RevenueCreditName",
                table: "UserMemberships");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "UserMemberships",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CreditAmount",
                table: "UserMemberships",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "UserMemberships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "UserMemberships",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "UserMemberships",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentReferenceFile",
                table: "UserMemberships",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentReferencekey",
                table: "UserMemberships",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RevenueCreditID",
                table: "UserMemberships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RevenueCreditName",
                table: "UserMemberships",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
