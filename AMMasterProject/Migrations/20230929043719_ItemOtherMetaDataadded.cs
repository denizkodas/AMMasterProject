using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class ItemOtherMetaDataadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Averagerating",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Followers",
                table: "Users_Profile");

            migrationBuilder.AddColumn<string>(
                name: "ItemOtherMetaData",
                table: "ItemListings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemOtherMetaData",
                table: "ItemListings");

            migrationBuilder.AddColumn<int>(
                name: "Averagerating",
                table: "Users_Profile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Followers",
                table: "Users_Profile",
                type: "int",
                nullable: true);
        }
    }
}
