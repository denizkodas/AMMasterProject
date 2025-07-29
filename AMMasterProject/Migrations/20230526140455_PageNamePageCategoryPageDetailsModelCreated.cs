using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class PageNamePageCategoryPageDetailsModelCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainPageCategory");

            migrationBuilder.DropTable(
                name: "PageCategoryDetails");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                table: "PageCategory");

            migrationBuilder.DropColumn(
                name: "IsURL",
                table: "PageCategory");

            migrationBuilder.DropColumn(
                name: "MainPageCategoryId",
                table: "PageCategory");

            migrationBuilder.DropColumn(
                name: "PageCategory",
                table: "PageCategory");

            migrationBuilder.DropColumn(
                name: "SEO_Description",
                table: "PageCategory");

            migrationBuilder.DropColumn(
                name: "SEO_Keyword",
                table: "PageCategory");

            migrationBuilder.DropColumn(
                name: "SEO_PageName",
                table: "PageCategory");

            migrationBuilder.DropColumn(
                name: "SEO_Title",
                table: "PageCategory");

            migrationBuilder.DropColumn(
                name: "Sortnumber",
                table: "PageCategory");

            migrationBuilder.DropColumn(
                name: "URL",
                table: "PageCategory");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "PageCategory",
                newName: "Category");

            migrationBuilder.AlterColumn<int>(
                name: "WebsiteItemSetupId",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Website_Setup_Product_Setting",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);


            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowTitle",
                table: "Website_Setup_Product_Setting",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowItemAsSlider",
                table: "Website_Setup_Product_Setting",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ShowBanner",
                table: "Website_Setup_Product_Setting",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SellingTypeId",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductViewQty",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductBoxCount",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublish",
                table: "Website_Setup_Product_Setting",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEditable",
                table: "Website_Setup_Product_Setting",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                table: "Website_Setup_Product_Setting",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PageDetails",
                columns: table => new
                {
                    PageDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageNameId = table.Column<int>(type: "int", nullable: true),
                    PageDescriptoin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageDetail", x => x.PageDetailId);
                });

            migrationBuilder.CreateTable(
                name: "PageName",
                columns: table => new
                {
                    PageNameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageCategoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsURL = table.Column<bool>(type: "bit", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sortnumber = table.Column<int>(type: "int", nullable: true),
                    SEOPageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SEOTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SEOKeyword = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SEODescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageName", x => x.PageNameId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageDetails");

            migrationBuilder.DropTable(
                name: "PageName");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "PageCategory",
                newName: "Type");

            migrationBuilder.AlterColumn<int>(
                name: "WebsiteItemSetupId",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Website_Setup_Product_Setting",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowTitle",
                table: "Website_Setup_Product_Setting",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowItemAsSlider",
                table: "Website_Setup_Product_Setting",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowBanner",
                table: "Website_Setup_Product_Setting",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "SellingTypeId",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProductViewQty",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProductBoxCount",
                table: "Website_Setup_Product_Setting",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPublish",
                table: "Website_Setup_Product_Setting",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEditable",
                table: "Website_Setup_Product_Setting",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                table: "Website_Setup_Product_Setting",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                table: "PageCategory",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsURL",
                table: "PageCategory",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MainPageCategoryId",
                table: "PageCategory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageCategory",
                table: "PageCategory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SEO_Description",
                table: "PageCategory",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SEO_Keyword",
                table: "PageCategory",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SEO_PageName",
                table: "PageCategory",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SEO_Title",
                table: "PageCategory",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sortnumber",
                table: "PageCategory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "PageCategory",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MainPageCategory",
                columns: table => new
                {
                    MainPageCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainPageCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainPageCategory", x => x.MainPageCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "PageCategoryDetails",
                columns: table => new
                {
                    PageCategoryDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PageCategoryDescriptoin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageCategoryId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageCategoryDetail", x => x.PageCategoryDetailId);
                });
        }
    }
}
