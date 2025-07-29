using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class GenderVideoURLDObUserprofilemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<DateTime>(
                name: "Dateofbirth",
                table: "Users_Profile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileVideoUrl",
                table: "Users_Profile",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users_Profile",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropColumn(
                name: "Dateofbirth",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "ProfileVideoUrl",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users_Profile");
        }
    }
}
