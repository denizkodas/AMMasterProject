using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class UserscreditModelchangetoUserMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersCredits");

            migrationBuilder.CreateTable(
                name: "UserMemberships",
                columns: table => new
                {
                    UserCreditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    RevenueCreditID = table.Column<int>(type: "int", nullable: false),
                    RevenueCreditName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NoofCredit = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CreditAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PaymentReferencekey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentReferenceFile = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsExpiry = table.Column<bool>(type: "bit", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMemberships", x => x.UserCreditId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMemberships");

            migrationBuilder.CreateTable(
                name: "UsersCredits",
                columns: table => new
                {
                    UserCreditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comments = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreditAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsExpiry = table.Column<bool>(type: "bit", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    NoofCredit = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PaymentReferenceFile = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PaymentReferencekey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    RevenueCreditID = table.Column<int>(type: "int", nullable: false),
                    RevenueCreditName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCredits", x => x.UserCreditId);
                });
        }
    }
}
