using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class userProfileBusienessMetaDataAddedGenderDOBRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dateofbirth",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users_Profile");

            migrationBuilder.AddColumn<string>(
                name: "BusinessMetaData",
                table: "Users_Profile",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessMetaData",
                table: "Users_Profile");

            migrationBuilder.AddColumn<DateTime>(
                name: "Dateofbirth",
                table: "Users_Profile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users_Profile",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
