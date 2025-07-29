using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class MasterLanguageJsonUpdateV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Define the path to the JSON file
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "masterlanguageV1.json");

            // Read the JSON file content
            var jsonContent = File.ReadAllText(path, Encoding.UTF8);

            // Ensure JSON content is properly escaped for SQL insertion
            var escapedJsonContent = jsonContent.Replace("'", "''");

            // Upsert the JSON content into the database
            migrationBuilder.Sql($@"
        MERGE INTO Websetting AS target
        USING (SELECT 'MasterLanguage' AS WebsettingKey, N'{escapedJsonContent}' AS WebsettingValue) AS source
        ON (target.WebsettingKey = source.WebsettingKey)
        WHEN MATCHED THEN 
            UPDATE SET WebsettingValue = source.WebsettingValue
        WHEN NOT MATCHED THEN
            INSERT (WebsettingKey, WebsettingValue) 
            VALUES (source.WebsettingKey, source.WebsettingValue);
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete the inserted row in the Down method
            migrationBuilder.Sql($@"
        DELETE FROM Websetting
        WHERE WebsettingKey = 'MasterLanguage'
            ");
        }
    }
}
