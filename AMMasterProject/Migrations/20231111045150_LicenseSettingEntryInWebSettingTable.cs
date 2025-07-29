using AMMasterProject.ViewModel;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class LicenseSettingEntryInWebSettingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create an instance of your LicenseAppSettingsModel
            var licensesettings = new LicenseAppSettingsModel
            {
                LicenseKey = "",
                ActivationDate = null,
                ExpiryDate = null,
                LicenseKeyForBrandRemoval = "",
                BrandRemovalActivationDate = null,
                BrandRemovalExpiryDate = null
            };

            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(licensesettings);

            migrationBuilder.InsertData(
                table: "Websetting",
                columns: new[] { "WebsettingKey", "WebsettingValue" },
                values: new object[] { "LicenseAppSettings", jsonData });
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
