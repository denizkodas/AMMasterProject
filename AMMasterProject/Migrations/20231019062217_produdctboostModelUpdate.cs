using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class produdctboostModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fees",
                table: "Product_Boost");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Product_Boost");

            migrationBuilder.DropColumn(
                name: "ProductGUID",
                table: "Product_Boost");

            migrationBuilder.AddColumn<string>(
                name: "BoostType",
                table: "Product_Boost",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "Product_Boost",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ItemID",
                table: "Product_Boost",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoostType",
                table: "Product_Boost");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Product_Boost");

            migrationBuilder.DropColumn(
                name: "ItemID",
                table: "Product_Boost");

            migrationBuilder.AddColumn<decimal>(
                name: "Fees",
                table: "Product_Boost",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Product_Boost",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductGUID",
                table: "Product_Boost",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
