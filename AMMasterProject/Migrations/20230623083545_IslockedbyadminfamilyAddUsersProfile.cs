using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class IslockedbyadminfamilyAddUsersProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminRemarksOnLocked",
                table: "Users_Profile",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLockedByAdmin",
                table: "Users_Profile",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnLockedDate",
                table: "Users_Profile",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminRemarksOnLocked",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "IsLockedByAdmin",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "UnLockedDate",
                table: "Users_Profile");
        }
    }
}
