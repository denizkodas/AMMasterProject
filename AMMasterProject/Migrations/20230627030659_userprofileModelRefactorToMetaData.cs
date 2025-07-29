using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class userprofileModelRefactorToMetaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminStatus",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Affiliateid",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "IsUserNameVerified",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "IsVerificationCode",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Isprofilecompleted",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "VerificationCodeDate",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "Users_Profile");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminStatus",
                table: "Users_Profile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Affiliateid",
                table: "Users_Profile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Users_Profile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Users_Profile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUserNameVerified",
                table: "Users_Profile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerificationCode",
                table: "Users_Profile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Isprofilecompleted",
                table: "Users_Profile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Users_Profile",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "Users_Profile",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Users_Profile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Users_Profile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "Users_Profile",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationCodeDate",
                table: "Users_Profile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "Users_Profile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
