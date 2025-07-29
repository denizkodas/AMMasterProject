using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileIdentityProofMetaDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificateProofMetaData",
                table: "Users_Profile",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityProofMetaData",
                table: "Users_Profile",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryContactMetaData",
                table: "Users_Profile",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateProofMetaData",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "IdentityProofMetaData",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "SecondaryContactMetaData",
                table: "Users_Profile");
        }
    }
}
