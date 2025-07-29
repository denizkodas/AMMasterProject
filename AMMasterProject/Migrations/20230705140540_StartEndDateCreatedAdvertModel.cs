using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class StartEndDateCreatedAdvertModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdvertisementId",
                table: "Advert",
                newName: "AdvertId");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Advert",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                table: "Advert",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Advert",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Advert",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Advert");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                table: "Advert");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Advert");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Advert");

            migrationBuilder.RenameColumn(
                name: "AdvertId",
                table: "Advert",
                newName: "AdvertisementId");
        }
    }
}
