using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class categorymodellistingtypeandsellingtypeadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryTypeId",
                table: "category_master",
                newName: "SellingTypeID");

            migrationBuilder.AddColumn<int>(
                name: "ListingTypeID",
                table: "category_master",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListingTypeID",
                table: "category_master");

            migrationBuilder.RenameColumn(
                name: "SellingTypeID",
                table: "category_master",
                newName: "CategoryTypeId");
        }
    }
}
