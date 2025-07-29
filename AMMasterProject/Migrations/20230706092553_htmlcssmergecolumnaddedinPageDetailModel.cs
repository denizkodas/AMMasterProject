using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class htmlcssmergecolumnaddedinPageDetailModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PageJson",
                table: "PageDetails",
                newName: "PageHTML");

            migrationBuilder.AddColumn<string>(
                name: "PageCSS",
                table: "PageDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdminLocked",
                table: "ItemListings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageCSS",
                table: "PageDetails");

            migrationBuilder.DropColumn(
                name: "IsAdminLocked",
                table: "ItemListings");

            migrationBuilder.RenameColumn(
                name: "PageHTML",
                table: "PageDetails",
                newName: "PageJson");
        }
    }
}
