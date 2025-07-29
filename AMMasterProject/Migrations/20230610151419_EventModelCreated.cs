using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class EventModelCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventCategory",
                columns: table => new
                {
                    EventCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventCategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategory", x => x.EventCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    categoryid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventStartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EventStartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EventEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    EventEndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    LastDateOfRegistration = table.Column<DateTime>(type: "date", nullable: true),
                    LastTimeOfRegistration = table.Column<TimeSpan>(type: "time", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    isaddonhomepage = table.Column<bool>(type: "bit", nullable: false),
                    externalurl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SEOPageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SEOTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SEOKeyword = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SEODescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventCategory");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
