using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class BlogCategoryModelRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Blog_Setup_Category",
                table: "Blog_Setup_Category");

            migrationBuilder.RenameTable(
                name: "Blog_Setup_Category",
                newName: "BlogCategory");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                table: "BlogCategory",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "BlogCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogCategory",
                table: "BlogCategory",
                column: "BlogCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogCategory",
                table: "BlogCategory");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                table: "BlogCategory");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "BlogCategory");

            migrationBuilder.RenameTable(
                name: "BlogCategory",
                newName: "Blog_Setup_Category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blog_Setup_Category",
                table: "Blog_Setup_Category",
                column: "BlogCategoryId");
        }
    }
}
