using Microsoft.EntityFrameworkCore.Migrations;
using System;
using ThirdParty.Json.LitJson;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class itemdetailOverCMS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert into PageNames
            migrationBuilder.InsertData(
                table: "PageName",
                columns: new[] { "PageName", "PageCategoryId", "Type", "IsPublish", "IsURL", "URL", "InsertDate", "Sortnumber", "SEOPageName", "SEOTitle", "SEOKeyword", "SEODescription", "IsAdminDefine", "PageType", "ProfileId", "CMSKey" },
                values: new object[] { "Item Detail Overview", 1, "cms", true, false, "", DateTime.Now, 1, "", "", "", "", true, "cms", 1, "itemdetailoverview" });

            // Retrieve the PageNameId where CMSKey is "itemdetailover"
            // Retrieve the PageNameId where CMSKey is "itemdetailover"
          

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
