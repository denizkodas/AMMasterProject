using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class MasterLanguageJsonUpdateV1 : Migration
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

            // Insert the JSON content into the database
            migrationBuilder.Sql($@"
                INSERT INTO Websetting (WebsettingKey, WebsettingValue)
                VALUES ('MasterLanguage', N'{escapedJsonContent}')
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
