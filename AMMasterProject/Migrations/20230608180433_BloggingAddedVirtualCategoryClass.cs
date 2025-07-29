using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class BloggingAddedVirtualCategoryClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bloging_categoryid",
                table: "Bloging",
                column: "categoryid");

            migrationBuilder.AddForeignKey(
                name: "FK_Bloging_BlogCategory_categoryid",
                table: "Bloging",
                column: "categoryid",
                principalTable: "BlogCategory",
                principalColumn: "BlogCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bloging_BlogCategory_categoryid",
                table: "Bloging");

            migrationBuilder.DropIndex(
                name: "IX_Bloging_categoryid",
                table: "Bloging");
        }
    }
}
