using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class WebsiteSetupModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropColumn(
                name: "DeductionType",
                table: "Website_Setup");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                table: "Website_Setup");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Website_Setup");

            migrationBuilder.DropColumn(
                name: "ItemDescription",
                table: "Website_Setup");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "Website_Setup");

            migrationBuilder.DropColumn(
                name: "ItemValue",
                table: "Website_Setup");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Website_Setup");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Website_Setup");

            migrationBuilder.AddColumn<string>(
                name: "ItemMetaData",
                table: "Website_Setup",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemMetaData",
                table: "Website_Setup");

            migrationBuilder.AddColumn<string>(
                name: "DeductionType",
                table: "Website_Setup",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                table: "Website_Setup",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Website_Setup",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemDescription",
                table: "Website_Setup",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "Website_Setup",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemValue",
                table: "Website_Setup",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Website_Setup",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Website_Setup",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Countryid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Countryid);
                });
        }
    }
}
