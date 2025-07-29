using Microsoft.EntityFrameworkCore.Migrations;
using Stripe;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class removeunwatedtablefromdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "Brands");


            migrationBuilder.DropTable(
                name: "CompanySetup");

            migrationBuilder.DropTable(
                name: "Contact_Master");

            // Repeat for other tables...

            migrationBuilder.DropTable(
                name: "Donation_Master");

            migrationBuilder.DropTable(
              name: "Donation_Received");

            migrationBuilder.DropTable(
             name: "Ecollab_Post_Comment");

            migrationBuilder.DropTable(
            name: "Ecollab_Post_Like");
            migrationBuilder.DropTable(
            name: "Ecollab_Post_Question");
            migrationBuilder.DropTable(
            name: "Email_Setup_Content");




            migrationBuilder.DropTable(
            name: "Email_Setup_Notifications");
            //migrationBuilder.DropTable(
            //name: "General_Setup");
            migrationBuilder.DropTable(
            name: "ItemReview");



            migrationBuilder.DropTable(
            name: "License_Setup");
            migrationBuilder.DropTable(
            name: "NewsLetter");
            migrationBuilder.DropTable(
            name: "NotificationContent");

          
            migrationBuilder.DropTable(
            name: "Product_Amenities_View_V2");
            migrationBuilder.DropTable(
            name: "Product_Basic_V2");
            migrationBuilder.DropTable(
            name: "Product_Brands");


            migrationBuilder.DropTable(
         name: "Product_Comparision_V2");
            migrationBuilder.DropTable(
            name: "Product_Discount");
            migrationBuilder.DropTable(
            name: "Product_Inventory");


            migrationBuilder.DropTable(
       name: "Product_Job_Post");
            migrationBuilder.DropTable(
            name: "Product_Job_Post_Apply");
            migrationBuilder.DropTable(
            name: "Product_Recently_Viewed");



            migrationBuilder.DropTable(
     name: "Product_Review_V2");
            migrationBuilder.DropTable(
            name: "Product_Tags");
            migrationBuilder.DropTable(
            name: "Product_Type_Setup");



            migrationBuilder.DropTable(
     name: "Product_Voucher");
            migrationBuilder.DropTable(
            name: "Setup_AWS");
            migrationBuilder.DropTable(
            name: "Specification_Setup");



            migrationBuilder.DropTable(
     name: "UserActivation");
            migrationBuilder.DropTable(
            name: "User_Notification");
            migrationBuilder.DropTable(
            name: "Users_LoyaltyPoint");




            migrationBuilder.DropTable(
     name: "Vendor_Membership_Package");
            migrationBuilder.DropTable(
            name: "Vendor_WareHouse");
            migrationBuilder.DropTable(
            name: "Website_Setup_CMS");



            migrationBuilder.DropTable(
 name: "Website_Setup_Holiday");
            migrationBuilder.DropTable(
            name: "Website_Setup_LoyaltyPoint");
            migrationBuilder.DropTable(
            name: "Website_Setup_Product_Detail");




            migrationBuilder.DropTable(
 name: "Website_Setup_Product_Setting");
            migrationBuilder.DropTable(
            name: "Website_Setup_Script");
            migrationBuilder.DropTable(
            name: "Website_Setup_Theme_Option");


            migrationBuilder.DropTable(
           name: "Websitesetup_Library");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the unwanted table creation












                
        }
    }
}
