using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class PageJsonPropertyAddedInPageDescriptionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PageDescriptoin",
                table: "PageDetails",
                newName: "PageDescription");

            migrationBuilder.AddColumn<string>(
                name: "PageJson",
                table: "PageDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageJson",
                table: "PageDetails");

            migrationBuilder.RenameColumn(
                name: "PageDescription",
                table: "PageDetails",
                newName: "PageDescriptoin");
        }
    }
}
