﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class urloptioninadvert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "Advert",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
              name: "Url",
              table: "Advert",
              type: "nvarchar(500)",
              maxLength: 500,
              nullable: true,
              oldClrType: typeof(string),
              oldType: "nvarchar(500)",
              oldMaxLength: 500,
              oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "Advert",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
               name: "Url",
               table: "Advert",
               type: "nvarchar(500)",
               maxLength: 500,
               nullable: true,
               oldClrType: typeof(string),
               oldType: "nvarchar(500)",
               oldMaxLength: 500,
               oldNullable: true);
        }
    }
}
