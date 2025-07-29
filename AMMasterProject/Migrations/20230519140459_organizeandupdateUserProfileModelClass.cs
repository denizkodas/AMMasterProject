using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class organizeandupdateUserProfileModelClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users_Profile",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "Users_Profile",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "loginchannel",
                table: "Users_Profile",
                newName: "Loginchannel");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "Users_Profile",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "Users_Profile",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "isprofilecompleted",
                table: "Users_Profile",
                newName: "Isprofilecompleted");

            migrationBuilder.RenameColumn(
                name: "followers",
                table: "Users_Profile",
                newName: "Followers");

            migrationBuilder.RenameColumn(
                name: "firstname",
                table: "Users_Profile",
                newName: "Firstname");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users_Profile",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "averagerating",
                table: "Users_Profile",
                newName: "Averagerating");

            migrationBuilder.RenameColumn(
                name: "affiliateid",
                table: "Users_Profile",
                newName: "Affiliateid");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Users_Profile",
                newName: "UserType");

            migrationBuilder.RenameColumn(
                name: "InsertDate",
                table: "Users_Profile",
                newName: "Datetime");

            migrationBuilder.RenameColumn(
                name: "ShopURLPath",
                table: "Users_Profile",
                newName: "BusinessURLPath");

            migrationBuilder.RenameColumn(
                name: "ShopName",
                table: "Users_Profile",
                newName: "SellerImage");

            migrationBuilder.RenameColumn(
                name: "ShopDescription",
                table: "Users_Profile",
                newName: "BusinessDescription");

            migrationBuilder.RenameColumn(
                name: "ProfileVideoUrl",
                table: "Users_Profile",
                newName: "SellerVideoURL");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Users_Profile",
                newName: "ProfileImage");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "Users_Profile",
                newName: "SellerDisplayName");

            migrationBuilder.RenameColumn(
                name: "CoverImage",
                table: "Users_Profile",
                newName: "SellerCoverImage");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users_Profile",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Loginchannel",
                table: "Users_Profile",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Lastname",
                table: "Users_Profile",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Firstname",
                table: "Users_Profile",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users_Profile",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Datetime",
                table: "Users_Profile",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Users_Profile",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessType",
                table: "Users_Profile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientDisplayName",
                table: "Users_Profile",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Lastlogin",
                table: "Users_Profile",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "BusinessType",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "ClientDisplayName",
                table: "Users_Profile");

            migrationBuilder.DropColumn(
                name: "Lastlogin",
                table: "Users_Profile");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users_Profile",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Users_Profile",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Loginchannel",
                table: "Users_Profile",
                newName: "loginchannel");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Users_Profile",
                newName: "latitude");

            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "Users_Profile",
                newName: "lastname");

            migrationBuilder.RenameColumn(
                name: "Isprofilecompleted",
                table: "Users_Profile",
                newName: "isprofilecompleted");

            migrationBuilder.RenameColumn(
                name: "Followers",
                table: "Users_Profile",
                newName: "followers");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "Users_Profile",
                newName: "firstname");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users_Profile",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Averagerating",
                table: "Users_Profile",
                newName: "averagerating");

            migrationBuilder.RenameColumn(
                name: "Affiliateid",
                table: "Users_Profile",
                newName: "affiliateid");

            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "Users_Profile",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Datetime",
                table: "Users_Profile",
                newName: "InsertDate");

            migrationBuilder.RenameColumn(
                name: "SellerVideoURL",
                table: "Users_Profile",
                newName: "ProfileVideoUrl");

            migrationBuilder.RenameColumn(
                name: "SellerImage",
                table: "Users_Profile",
                newName: "ShopName");

            migrationBuilder.RenameColumn(
                name: "SellerDisplayName",
                table: "Users_Profile",
                newName: "DisplayName");

            migrationBuilder.RenameColumn(
                name: "SellerCoverImage",
                table: "Users_Profile",
                newName: "CoverImage");

            migrationBuilder.RenameColumn(
                name: "ProfileImage",
                table: "Users_Profile",
                newName: "Logo");

            migrationBuilder.RenameColumn(
                name: "BusinessURLPath",
                table: "Users_Profile",
                newName: "ShopURLPath");

            migrationBuilder.RenameColumn(
                name: "BusinessDescription",
                table: "Users_Profile",
                newName: "ShopDescription");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users_Profile",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "Users_Profile",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "loginchannel",
                table: "Users_Profile",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "lastname",
                table: "Users_Profile",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "firstname",
                table: "Users_Profile",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertDate",
                table: "Users_Profile",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
