using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class emailtemplatecmsforsellercontactform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert into PageNames
            migrationBuilder.InsertData(
                table: "PageName",
                columns: new[] { "PageName", "PageCategoryId", "Type", "IsPublish", "IsURL", "URL", "InsertDate", "Sortnumber", "SEOPageName", "SEOTitle", "SEOKeyword", "SEODescription", "IsAdminDefine", "PageType", "ProfileId", "CMSKey" },
                values: new object[] { "Seller Contact Form", 1, "email", true, false, "", DateTime.Now, 1, "", "", "", "", true, "cms", 1, "sellercontactform" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
