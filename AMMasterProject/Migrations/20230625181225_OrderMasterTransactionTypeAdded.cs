﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class OrderMasterTransactionTypeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "OrderMasters",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "OrderMasters");
        }
    }
}
