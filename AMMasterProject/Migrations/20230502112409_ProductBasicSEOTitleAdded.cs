using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class ProductBasicSEOTitleAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SEO_Keywords",
                table: "Product_Basic_V2",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SEO_MetaTItle",
                table: "Product_Basic_V2",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SEO_Metadescription",
                table: "Product_Basic_V2",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SEO_PageName",
                table: "Product_Basic_V2",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SEO_Keywords",
                table: "Product_Basic_V2");

            migrationBuilder.DropColumn(
                name: "SEO_MetaTItle",
                table: "Product_Basic_V2");

            migrationBuilder.DropColumn(
                name: "SEO_Metadescription",
                table: "Product_Basic_V2");

            migrationBuilder.DropColumn(
                name: "SEO_PageName",
                table: "Product_Basic_V2");
        }
    }
}
