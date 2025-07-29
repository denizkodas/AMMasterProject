using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class RecreateUsersProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Users_Profile",
               columns: table => new
               {
                   ProfileId = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   ProfileGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                   UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                   Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                   InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                   AdminStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                   Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                   ShopName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                   ShopURLPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                   Logo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                   CoverImage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                   ContactNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                   Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                   City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                   Zip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                   State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                   Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                   ShopDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                   PaypalId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                   IsMembershipFree = table.Column<bool>(type: "bit", nullable: true),
                   stripeid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                   latitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                   longitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                   firstname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                   lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                   aboutshop = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                   email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                   isprofilecompleted = table.Column<bool>(type: "bit", nullable: true),
                   affiliateid = table.Column<int>(type: "int", nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Users_Profile", x => x.ProfileId);
               });



            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");
        }
    }
}
