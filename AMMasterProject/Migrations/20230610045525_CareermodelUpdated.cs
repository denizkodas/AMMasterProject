using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class CareermodelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CareerGuid",
                table: "Careers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Careers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                table: "Careers",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublish",
                table: "Careers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Careers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SEODescription",
                table: "Careers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SEOKeyword",
                table: "Careers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SEOPageName",
                table: "Careers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SEOTitle",
                table: "Careers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Careers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Careers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "categoryid",
                table: "Careers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "externalurl",
                table: "Careers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isaddonhomepage",
                table: "Careers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CareerGuid",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "IsPublish",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "SEODescription",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "SEOKeyword",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "SEOPageName",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "SEOTitle",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "categoryid",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "externalurl",
                table: "Careers");

            migrationBuilder.DropColumn(
                name: "isaddonhomepage",
                table: "Careers");
        }
    }
}
