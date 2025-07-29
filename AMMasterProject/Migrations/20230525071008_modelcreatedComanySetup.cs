using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class modelcreatedComanySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logo");

            migrationBuilder.CreateTable(
                name: "CompanySetup",
                columns: table => new
                {
                    CompanySetupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Favicon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CompanyDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CompanyAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SupportContact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SupportEmail = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySetup", x => x.CompanySetupId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanySetup");

            migrationBuilder.CreateTable(
                name: "Logo",
                columns: table => new
                {
                    LogoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Favicon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FooterLogo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Keyword = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    logoheight = table.Column<int>(type: "int", nullable: true),
                    logowidth = table.Column<int>(type: "int", nullable: true),
                    SupportContact = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WebsiteName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logo", x => x.LogoId);
                });
        }
    }
}
