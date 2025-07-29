using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class RevenueCreditPacakgemodelcreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevenueCreditPacakge",
                columns: table => new
                {
                    RevenuePackageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RevenueCreditName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NoofCredit = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CreditAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    IsRecommended = table.Column<bool>(type: "bit", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreditImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsExpiry = table.Column<bool>(type: "bit", nullable: false),
                    NoofExpiryDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueCreditPacakge", x => x.RevenuePackageID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevenueCreditPacakge");
        }
    }
}
