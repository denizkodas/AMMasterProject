using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class modelcreatedNotificationContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationContent",
                columns: table => new
                {
                    NotificationContentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationChannel = table.Column<int>(type: "int", nullable: false),
                    NotificationSubject = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    NotificationText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationContent", x => x.NotificationContentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationContent");
        }
    }
}
