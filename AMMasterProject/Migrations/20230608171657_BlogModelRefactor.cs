using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class BlogModelRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog_Setup");

            migrationBuilder.DropTable(
                name: "Media_notes");

            migrationBuilder.CreateTable(
                name: "Blog_Setup_Category",
                columns: table => new
                {
                    BlogCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogCategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog_Setup_Category", x => x.BlogCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Bloging",
                columns: table => new
                {
                    BlogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    categoryid = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fileattached = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    isaddonhomepage = table.Column<bool>(type: "bit", nullable: false),
                    externalurl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SEOPageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SEOTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SEOKeyword = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SEODescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloging", x => x.BlogId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog_Setup_Category");

            migrationBuilder.DropTable(
                name: "Bloging");

            migrationBuilder.CreateTable(
                name: "Blog_Setup",
                columns: table => new
                {
                    BlogSetup_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Banner = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    category_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    categoryvalue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsIncludeMenu = table.Column<bool>(type: "bit", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    IsShowHomePage = table.Column<bool>(type: "bit", nullable: true),
                    keyid = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    parent_category_id = table.Column<int>(type: "int", nullable: true),
                    SEO_Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SEO_Keyword = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SEO_PageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SEO_Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    URLPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog_Setup", x => x.BlogSetup_id);
                });

            migrationBuilder.CreateTable(
                name: "Media_notes",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryid = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    externalurl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Fileattached = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    isaddonhomepage = table.Column<bool>(type: "bit", nullable: true),
                    isaddonslider = table.Column<bool>(type: "bit", nullable: true),
                    section = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TitleSEO = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media_notes", x => x.MediaId);
                });
        }
    }
}
