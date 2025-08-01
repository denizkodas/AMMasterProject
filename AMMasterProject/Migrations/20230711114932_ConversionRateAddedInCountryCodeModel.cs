﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class ConversionRateAddedInCountryCodeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ConversionRate",
                table: "CountryCode",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversionRate",
                table: "CountryCode");
        }
    }
}
