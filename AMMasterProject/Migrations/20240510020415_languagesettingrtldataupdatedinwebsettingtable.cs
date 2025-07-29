using Microsoft.EntityFrameworkCore.Migrations;
using System.Text.Json;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class languagesettingrtldataupdatedinwebsettingtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // JSON serialization of the languages array
            var languages = new[]
            {
            new { Name = "English", Code = "en", IsPublish = true, IsRTL = false },
            new { Name = "French", Code = "fr", IsPublish = true, IsRTL = false },
            new { Name = "Spanish", Code = "es", IsPublish = true, IsRTL = false },
            new { Name = "German", Code = "de", IsPublish = true, IsRTL = false },
            new { Name = "Italian", Code = "it", IsPublish = true, IsRTL = false },
            new { Name = "Japanese", Code = "ja", IsPublish = true, IsRTL = false },
            new { Name = "Turkish", Code = "tr", IsPublish = true, IsRTL = false },
            new { Name = "Arabic", Code = "ar", IsPublish = true, IsRTL = true },
            new { Name = "Urdu", Code = "ur", IsPublish = true, IsRTL = true }
        };

            string jsonLanguages = JsonSerializer.Serialize(languages);

            // Assuming 'WebSettings' is your table name and it has 'Key' and 'Value' columns
            migrationBuilder.Sql($@"
            IF EXISTS (SELECT 1 FROM WebSetting WHERE [WebsettingKey] = 'Language')
                UPDATE WebSetting
                SET [WebsettingValue] = '{jsonLanguages}'
                WHERE [WebsettingKey] = 'Language';
            ELSE
                INSERT INTO WebSetting ([WebsettingKey], [WebsettingValue])
                VALUES ('Language', '{jsonLanguages}');
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // To revert the changes, you might want to delete the entry or reset it to a default value
            migrationBuilder.Sql("DELETE FROM WebSetting WHERE [WebsettingKey] = 'Language';");

            // Alternatively, you could set it back to a default value if deletion is not desired
            // migrationBuilder.Sql("UPDATE WebSettings SET [Value] = 'Default Value' WHERE [Key] = 'Language';");
        }
    }
}
