using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class changefieldnametoNotificationBodyinNotificationContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificationText",
                table: "NotificationContent",
                newName: "NotificationBody");

            migrationBuilder.AlterColumn<string>(
                name: "NotificationSubject",
                table: "NotificationContent",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotificationBody",
                table: "NotificationContent",
                newName: "NotificationText");

            migrationBuilder.AlterColumn<string>(
                name: "NotificationSubject",
                table: "NotificationContent",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
