using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class AvailablitySetupMetaDataAddedandRemovedWebsiteSetupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Website_Setup");

            migrationBuilder.AddColumn<string>(
                name: "AvailabilitySetupMetaData",
                table: "Users_Profile",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailabilitySetupMetaData",
                table: "Users_Profile");

            migrationBuilder.CreateTable(
                name: "Website_Setup",
                columns: table => new
                {
                    WebsiteSetupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemMetaData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Setup", x => x.WebsiteSetupId);
                });
        }
    }
}
