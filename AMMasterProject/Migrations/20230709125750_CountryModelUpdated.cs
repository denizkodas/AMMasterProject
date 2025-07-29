using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class CountryModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_COUNTRY_code",
                table: "COUNTRY_code");

            migrationBuilder.DropColumn(
                name: "CCode",
                table: "COUNTRY_code");

            migrationBuilder.DropColumn(
                name: "Flag",
                table: "COUNTRY_code");

            migrationBuilder.DropColumn(
                name: "countryName",
                table: "COUNTRY_code");

            migrationBuilder.DropColumn(
                name: "isbuyeractive",
                table: "COUNTRY_code");

            migrationBuilder.DropColumn(
                name: "isselleractive",
                table: "COUNTRY_code");

            migrationBuilder.RenameTable(
                name: "COUNTRY_code",
                newName: "CountryCode");

            migrationBuilder.RenameColumn(
                name: "flagpath",
                table: "CountryCode",
                newName: "FlagPath");

            migrationBuilder.RenameColumn(
                name: "currencycode",
                table: "CountryCode",
                newName: "CurrencyCode");

            migrationBuilder.RenameColumn(
                name: "countrycode",
                table: "CountryCode",
                newName: "CountryCode3Digit");

            migrationBuilder.RenameColumn(
                name: "cActive",
                table: "CountryCode",
                newName: "IsPublish");

            migrationBuilder.RenameColumn(
                name: "CID",
                table: "CountryCode",
                newName: "CountryID");

            migrationBuilder.AlterColumn<string>(
                name: "FlagPath",
                table: "CountryCode",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "CountryCode",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode2Digit",
                table: "CountryCode",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                table: "CountryCode",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileCode",
                table: "CountryCode",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CountryCode",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CountryCode",
                table: "CountryCode",
                column: "CountryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CountryCode",
                table: "CountryCode");

            migrationBuilder.DropColumn(
                name: "CountryCode2Digit",
                table: "CountryCode");

            migrationBuilder.DropColumn(
                name: "CurrencyName",
                table: "CountryCode");

            migrationBuilder.DropColumn(
                name: "MobileCode",
                table: "CountryCode");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CountryCode");

            migrationBuilder.RenameTable(
                name: "CountryCode",
                newName: "COUNTRY_code");

            migrationBuilder.RenameColumn(
                name: "FlagPath",
                table: "COUNTRY_code",
                newName: "flagpath");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "COUNTRY_code",
                newName: "currencycode");

            migrationBuilder.RenameColumn(
                name: "IsPublish",
                table: "COUNTRY_code",
                newName: "cActive");

            migrationBuilder.RenameColumn(
                name: "CountryCode3Digit",
                table: "COUNTRY_code",
                newName: "countrycode");

            migrationBuilder.RenameColumn(
                name: "CountryID",
                table: "COUNTRY_code",
                newName: "CID");

            migrationBuilder.AlterColumn<string>(
                name: "flagpath",
                table: "COUNTRY_code",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "currencycode",
                table: "COUNTRY_code",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CCode",
                table: "COUNTRY_code",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Flag",
                table: "COUNTRY_code",
                type: "image",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "countryName",
                table: "COUNTRY_code",
                type: "varchar(250)",
                unicode: false,
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "isbuyeractive",
                table: "COUNTRY_code",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isselleractive",
                table: "COUNTRY_code",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_COUNTRY_code",
                table: "COUNTRY_code",
                column: "CID");
        }
    }
}
