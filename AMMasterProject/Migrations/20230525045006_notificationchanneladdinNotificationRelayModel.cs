using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class notificationchanneladdinNotificationRelayModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationChannel",
                table: "NotificationRelays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "NotificationRelays",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationChannel",
                table: "NotificationRelays");

            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "NotificationRelays");
        }
    }
}
