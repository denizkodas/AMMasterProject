using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class PageDetailtableremovedandpropertymergeinpagename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PageCSS",
                table: "PageName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PageDescription",
                table: "PageName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PageHTML",
                table: "PageName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PageJson",
                table: "PageName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageCSS",
                table: "PageName");

            migrationBuilder.DropColumn(
                name: "PageDescription",
                table: "PageName");

            migrationBuilder.DropColumn(
                name: "PageHTML",
                table: "PageName");

            migrationBuilder.DropColumn(
                name: "PageJson",
                table: "PageName");
        }
    }
}
