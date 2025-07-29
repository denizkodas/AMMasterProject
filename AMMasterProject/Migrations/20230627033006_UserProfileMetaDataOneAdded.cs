using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileMetaDataOneAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminStatusMetaData",
                table: "Users_Profile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryAddressMetaData",
                table: "Users_Profile",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryAddressMetaData",
                table: "Users_Profile",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellerPayAccountMetaData",
                table: "Users_Profile",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaMetaData",
                table: "Users_Profile",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamMembersMetaData",
                table: "Users_Profile",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminStatusMetaData",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "PrimaryAddressMetaData",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "SecondaryAddressMetaData",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "SellerPayAccountMetaData",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "SocialMediaMetaData",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "TeamMembersMetaData",
                table: "Users_Profile");
        }
    }
}
