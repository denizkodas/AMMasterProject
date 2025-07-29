using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class remove_Username_Added_userid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Customer_Wishlist");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Customer_Wishlist",
                type: "int",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Customer_Wishlist");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Customer_Wishlist",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }
    }
}
