using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class createcmshomefooter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert into PageNames
            migrationBuilder.InsertData(
                table: "PageName",
                columns: new[] { "PageName", "PageCategoryId", "Type", "IsPublish", "IsURL", "URL", "InsertDate", "Sortnumber", "SEOPageName", "SEOTitle", "SEOKeyword", "SEODescription", "IsAdminDefine", "PageType", "ProfileId", "CMSKey" },
                values: new object[] { "Home Page Footer", 1, "cms", true, false, "", DateTime.Now, 1, "", "", "", "", true, "cms", 1, "homefooter" });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
        }
    }
}
