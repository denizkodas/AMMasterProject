using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class FollowerFields_movetouserprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "followers",
                table: "Product_Basic_V2");

            migrationBuilder.AddColumn<int>(
                name: "followers",
                table: "Users_Profile",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "followers",
                table: "Users_Profile");

            migrationBuilder.AddColumn<int>(
                name: "followers",
                table: "Product_Basic_V2",
                type: "int",
                nullable: true);
        }
    }
}
