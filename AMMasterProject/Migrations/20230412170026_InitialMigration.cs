using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ads_Detail",
                columns: table => new
                {
                    AdId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdsPlacementId = table.Column<int>(type: "int", nullable: true),
                    AdsPageId = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsUrl = table.Column<bool>(type: "bit", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ads_Detail", x => x.AdId);
                });

            migrationBuilder.CreateTable(
                name: "Ads_PageName",
                columns: table => new
                {
                    AdsPageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdsPlacementId = table.Column<int>(type: "int", nullable: true),
                    PageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ads_PageName", x => x.AdsPageId);
                });

            migrationBuilder.CreateTable(
                name: "Ads_Placement",
                columns: table => new
                {
                    AdsPlacementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlacementName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ads_Placement", x => x.AdsPlacementId);
                });

            migrationBuilder.CreateTable(
                name: "Alphabet",
                columns: table => new
                {
                    Alphabetid = table.Column<int>(type: "int", nullable: false),
                    Alphabet = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Blog_Setup",
                columns: table => new
                {
                    BlogSetup_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    URLPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    parent_category_id = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Banner = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    IsShowHomePage = table.Column<bool>(type: "bit", nullable: true),
                    IsIncludeMenu = table.Column<bool>(type: "bit", nullable: true),
                    SEO_PageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SEO_Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SEO_Keyword = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SEO_Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    categoryvalue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    keyid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog_Setup", x => x.BlogSetup_id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_Id = table.Column<int>(type: "int", nullable: true),
                    BrandName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BrandDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BrandURLPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "category_master",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    URLPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    parent_category_id = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Banner = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    IsShowHomePage = table.Column<bool>(type: "bit", nullable: true),
                    IsIncludeMenu = table.Column<bool>(type: "bit", nullable: true),
                    SEO_PageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SEO_Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SEO_Keyword = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SEO_Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    CategoryTypeId = table.Column<int>(type: "int", nullable: true),
                    Sellingtypeid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_master", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "Contact_Master",
                columns: table => new
                {
                    ContactMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact_Master", x => x.ContactMasterId);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Countryid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Countryid);
                });

            migrationBuilder.CreateTable(
                name: "COUNTRY_code",
                columns: table => new
                {
                    CID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    countryName = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    cActive = table.Column<bool>(type: "bit", nullable: false),
                    Flag = table.Column<byte[]>(type: "image", nullable: true),
                    CCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    currencycode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    countrycode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    flagpath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    isbuyeractive = table.Column<bool>(type: "bit", nullable: true),
                    isselleractive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COUNTRY_code", x => x.CID);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "date", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "Customer_Address",
                columns: table => new
                {
                    CustomerAddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerAddressGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Zipcode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BuyerId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer_Address", x => x.CustomerAddressId);
                });

            migrationBuilder.CreateTable(
                name: "Customer_Wishlist",
                columns: table => new
                {
                    WishlistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cwgroupname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer_Wishlist", x => x.WishlistId);
                });

            migrationBuilder.CreateTable(
                name: "Donation_Master",
                columns: table => new
                {
                    RequestFundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestFundGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    Title = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Administrativefee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaypalID = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AdminStatus = table.Column<bool>(type: "bit", nullable: true),
                    AdminRemarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donation_Master", x => x.RequestFundId);
                });

            migrationBuilder.CreateTable(
                name: "Donation_Received",
                columns: table => new
                {
                    DonationReceivedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestFundGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdministrativeFeesPer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdministrativeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaypalId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donation_Received", x => x.DonationReceivedId);
                });

            migrationBuilder.CreateTable(
                name: "Ecollab_Post_Comment",
                columns: table => new
                {
                    EcollabCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EcollabReferenceId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    readstatus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ecollab_Post_Comment", x => x.EcollabCommentId);
                });

            migrationBuilder.CreateTable(
                name: "Ecollab_Post_Like",
                columns: table => new
                {
                    EcollablikeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EcollabReferenceId = table.Column<int>(type: "int", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    readstatus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ecollab_Post_Like", x => x.EcollablikeId);
                });

            migrationBuilder.CreateTable(
                name: "Ecollab_Post_Question",
                columns: table => new
                {
                    EcollabPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EcollabPostGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: true),
                    postview = table.Column<int>(type: "int", nullable: true),
                    image = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ModuleReferenceID = table.Column<int>(type: "int", nullable: true),
                    ModuleReferenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    subcatid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ecollab_Post_Question", x => x.EcollabPostId);
                });

            migrationBuilder.CreateTable(
                name: "Email_Setup",
                columns: table => new
                {
                    EmailSettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailFrom = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SMTPPort = table.Column<int>(type: "int", nullable: true),
                    Host = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BCC = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Enablessl = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email_Setup", x => x.EmailSettingId);
                });

            migrationBuilder.CreateTable(
                name: "Email_Setup_Content",
                columns: table => new
                {
                    EmailTextID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ProfileID = table.Column<int>(type: "int", nullable: true),
                    EmailType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email_Setup_Content", x => x.EmailTextID);
                });

            migrationBuilder.CreateTable(
                name: "Email_Setup_Notifications",
                columns: table => new
                {
                    EmailNotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<int>(type: "int", nullable: true, comment: "Registration(Profileid) or OrderID (Order Confirmation) or OrderStatusid (order status change)"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Isemailsent = table.Column<bool>(type: "bit", nullable: true),
                    ismobilenotificationsent = table.Column<bool>(type: "bit", nullable: true),
                    EmailBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNotification = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RedirectURL = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email_Setup_Notifications", x => x.EmailNotificationId);
                });

            migrationBuilder.CreateTable(
                name: "General_Setup",
                columns: table => new
                {
                    GeneralSetupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralSetupType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneralSetupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneralSetupImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GeneralSetupIcon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    Insertdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_General_Setup", x => x.GeneralSetupId);
                });

            migrationBuilder.CreateTable(
                name: "License_Setup",
                columns: table => new
                {
                    LicenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellingPlatform = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LicenseKeyByAdmin = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LicenseKeyByUser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DomainName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    SaleDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_License_Setup", x => x.LicenseId);
                });

            migrationBuilder.CreateTable(
                name: "Logo",
                columns: table => new
                {
                    LogoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    logowidth = table.Column<int>(type: "int", nullable: true),
                    logoheight = table.Column<int>(type: "int", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SupportContact = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WebsiteName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FooterLogo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Favicon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Keyword = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logo", x => x.LogoId);
                });

            migrationBuilder.CreateTable(
                name: "MainPageCategory",
                columns: table => new
                {
                    MainPageCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainPageCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainPageCategory", x => x.MainPageCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Media_notes",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TitleSEO = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fileattached = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    section = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    isaddonslider = table.Column<bool>(type: "bit", nullable: true),
                    categoryid = table.Column<int>(type: "int", nullable: true),
                    isaddonhomepage = table.Column<bool>(type: "bit", nullable: true),
                    externalurl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media_notes", x => x.MediaId);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MesageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    senddate = table.Column<DateTime>(type: "datetime", nullable: true),
                    recieveddate = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    senderid = table.Column<int>(type: "int", nullable: true),
                    receiverid = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    sample = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MesageId);
                });

            migrationBuilder.CreateTable(
                name: "NewsLetter",
                columns: table => new
                {
                    NewsletterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsLetter", x => x.NewsletterID);
                });

            migrationBuilder.CreateTable(
                name: "Order_Charges_V2",
                columns: table => new
                {
                    OrderChargeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderChargeGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    OrderMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChargeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    VendorNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Charges_V2", x => x.OrderChargeId);
                });

            migrationBuilder.CreateTable(
                name: "Order_Delivery_V2",
                columns: table => new
                {
                    OrderShippingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderShippingGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    OrderMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerAddressGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    VendorNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentReferenceId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiscountCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Delivery_V2", x => x.OrderShippingId);
                });

            migrationBuilder.CreateTable(
                name: "Order_Disbursement",
                columns: table => new
                {
                    OrderEarningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisburseDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DisbursementMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DisbursementId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Disbursement", x => x.OrderEarningId);
                });

            migrationBuilder.CreateTable(
                name: "Order_Master_V2",
                columns: table => new
                {
                    OrderMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    OrderMasterCustomGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    BuyerId = table.Column<int>(type: "int", nullable: true),
                    OrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastOrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BuyerNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VendorNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    ActualPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductAttributePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QTY = table.Column<int>(type: "int", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentReferenceId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CommissionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CommisionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsCoupon = table.Column<bool>(type: "bit", nullable: true),
                    CouponCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrencyConversionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConversionCurrencySymbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DeliveryStatusId = table.Column<int>(type: "int", nullable: true),
                    ProductJobAppliedGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GroupOrderGeneratedGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Master_V2", x => x.OrderMasterId);
                });

            migrationBuilder.CreateTable(
                name: "Order_Status_V2",
                columns: table => new
                {
                    OrderStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderStatusGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    OrderMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GeneralSetupId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    VendorNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Statustype = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OrderMasterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Status_V2", x => x.OrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Order_Variation_V2",
                columns: table => new
                {
                    OrderVariationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderVariationGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    OrderMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductAttributeGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Attributeprice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendorNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ProductAttributeOptionId = table.Column<int>(type: "int", nullable: true),
                    ProductAttributeSelectedOption = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Variation_V2", x => x.OrderVariationId);
                });

            migrationBuilder.CreateTable(
                name: "OrderIDGeneration_V2",
                columns: table => new
                {
                    ordergeneratedguidid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    insertdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderIDG__CFBD77621CD6197B", x => x.ordergeneratedguidid);
                });

            migrationBuilder.CreateTable(
                name: "PageCategory",
                columns: table => new
                {
                    PageCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainPageCategoryId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PageCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsURL = table.Column<bool>(type: "bit", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Sortnumber = table.Column<int>(type: "int", nullable: true),
                    SEO_PageName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SEO_Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SEO_Keyword = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SEO_Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageCategory", x => x.PageCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "PageCategoryDetails",
                columns: table => new
                {
                    PageCategoryDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageCategoryId = table.Column<int>(type: "int", nullable: true),
                    PageCategoryDescriptoin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageCategoryDetail", x => x.PageCategoryDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Amenities_Options_V2",
                columns: table => new
                {
                    ProductAmenitiesOptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductAmenitiesId = table.Column<int>(type: "int", nullable: true),
                    ProductAmenitiesIcon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProductAmenitiesName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Amenities_Options", x => x.ProductAmenitiesOptionID);
                });

            migrationBuilder.CreateTable(
                name: "Product_Amenities_View_V2",
                columns: table => new
                {
                    ProductAmenitiesViewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductAmenitiesOptionID = table.Column<int>(type: "int", nullable: true),
                    ProductAmenitiesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Amenities_View_V2", x => x.ProductAmenitiesViewID);
                });

            migrationBuilder.CreateTable(
                name: "Product_AmenitiesQuestion_V2",
                columns: table => new
                {
                    ProductAmenitiesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductAmenitiesHeading = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    SortNumber = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ControlType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Amenities_V2", x => x.ProductAmenitiesId);
                });

            migrationBuilder.CreateTable(
                name: "Product_AttributeOption_V2",
                columns: table => new
                {
                    ProductAttributeOptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductAttributeGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OptionText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    attributeprice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    attributeimage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_AttributeOption_V2", x => x.ProductAttributeOptionId);
                });

            migrationBuilder.CreateTable(
                name: "Product_AttributeQuestion_V2",
                columns: table => new
                {
                    ProductAttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductAttributeGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SortNumber = table.Column<int>(type: "int", nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttribute_Master", x => x.ProductAttributeId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Boost",
                columns: table => new
                {
                    ProductBoostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Fees = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Boost", x => x.ProductBoostId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Brands",
                columns: table => new
                {
                    ProductBrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Brand", x => x.ProductBrandId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Comparision_V2",
                columns: table => new
                {
                    ProductComparisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductComparisionGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Comparision_V2", x => x.ProductComparisionId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Coupon",
                columns: table => new
                {
                    ProductCouponId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCouponGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    CouponTypeId = table.Column<int>(type: "int", nullable: true),
                    CouponName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    NoofCoupon = table.Column<int>(type: "int", nullable: true),
                    PerPersonUsed = table.Column<int>(type: "int", nullable: true),
                    DiscountType = table.Column<int>(type: "int", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SellerId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Coupon", x => x.ProductCouponId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Coupon_Child",
                columns: table => new
                {
                    ProductCouponChildId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCouponChildGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    ProductCouponGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    CouponTypeId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Coupon_Child", x => x.ProductCouponChildId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Discount",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiscountOffer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DiscountEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModuleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModuleID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Discount", x => x.DiscountId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Images",
                columns: table => new
                {
                    ProductImagesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AltText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Displayorder = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Images", x => x.ProductImagesId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Inventory",
                columns: table => new
                {
                    ProductInventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Inventory = table.Column<int>(type: "int", nullable: true),
                    PurchaseCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    purchasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    profileid = table.Column<int>(type: "int", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "datetime", nullable: true),
                    modifiedby = table.Column<int>(type: "int", nullable: true),
                    vendor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Inventory", x => x.ProductInventoryId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Job_Post",
                columns: table => new
                {
                    ProductJobPostID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductJobPostGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    ProductTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ProductImage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    IsBudget = table.Column<bool>(type: "bit", nullable: true),
                    PriceStart = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceEnd = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsJobAllocated = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Job_Post", x => x.ProductJobPostID);
                });

            migrationBuilder.CreateTable(
                name: "Product_Job_Post_Apply",
                columns: table => new
                {
                    ProductJobAppliedID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductJobAppliedGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    ProductJobPostGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProfileID = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    JobApplication = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PriceBid = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsJobAssigned = table.Column<bool>(type: "bit", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AssignedNotes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CancelledReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Job_Post_Apply", x => x.ProductJobAppliedID);
                });

            migrationBuilder.CreateTable(
                name: "Product_Question",
                columns: table => new
                {
                    ProductQAId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Question_Answer", x => x.ProductQAId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Question_Answer",
                columns: table => new
                {
                    ProductAnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductQAId = table.Column<int>(type: "int", nullable: true),
                    QAnswer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Question_Answer_1", x => x.ProductAnswerId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Recently_Viewed",
                columns: table => new
                {
                    RecentlyViewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Insertdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Recently_Viewed", x => x.RecentlyViewId);
                });

            migrationBuilder.CreateTable(
                name: "Product_RelatedProducts",
                columns: table => new
                {
                    ProductRelatedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedProductID = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_RelatedProducts", x => x.ProductRelatedId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Review_V2",
                columns: table => new
                {
                    ReviewProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderMasterId = table.Column<int>(type: "int", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    StartRating = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Review_V2", x => x.ReviewProductId);
                });

            migrationBuilder.CreateTable(
                name: "Product_SEO",
                columns: table => new
                {
                    SEOId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SEO_PageName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SEO_MetaTItle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SEO_Keywords = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SEO_Metadescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_SEO", x => x.SEOId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Tags",
                columns: table => new
                {
                    ProductTagsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductTags = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Tags", x => x.ProductTagsId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Tax_V2",
                columns: table => new
                {
                    ProductTaxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxClassId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Tax_V2", x => x.ProductTaxId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Type_Setup",
                columns: table => new
                {
                    ProductTypeSetupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Type_Setup", x => x.ProductTypeSetupId);
                });

            migrationBuilder.CreateTable(
                name: "Product_Voucher",
                columns: table => new
                {
                    VoucherID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Voucher", x => x.VoucherID);
                });

            migrationBuilder.CreateTable(
                name: "Setup_AWS",
                columns: table => new
                {
                    AWSID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SecretyKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ServerPoint = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    URLPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BucketName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setup_AWS", x => x.AWSID);
                });

            migrationBuilder.CreateTable(
                name: "Setup_TaxClass",
                columns: table => new
                {
                    TaxClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxClassGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    TaxClass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: true),
                    IsAdminApproved = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setup_TaxClass", x => x.TaxClassID);
                });

            migrationBuilder.CreateTable(
                name: "SocialMedia",
                columns: table => new
                {
                    SocialMediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icon = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    URL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedia", x => x.SocialMediaId);
                });

            migrationBuilder.CreateTable(
                name: "Specification_Setup",
                columns: table => new
                {
                    SpecificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: true),
                    SpecificationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SpecificiatoinDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specification_Setup", x => x.SpecificationId);
                });

            migrationBuilder.CreateTable(
                name: "User_Notification",
                columns: table => new
                {
                    UserNotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageBody = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReceiverID = table.Column<int>(type: "int", nullable: true),
                    SenderID = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true),
                    Module = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RedirectPage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    isemailsent = table.Column<bool>(type: "bit", nullable: true),
                    ismobilenotificationsent = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Notification", x => x.UserNotificationId);
                });

            migrationBuilder.CreateTable(
                name: "UserActivation",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ActivationCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivation", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    GUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    registertype = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    logintype = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    loginchannel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Users_LoyaltyPoint",
                columns: table => new
                {
                    UserLoyaltyPointID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderMasterCustomGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LoyaltyPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LoyaltyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_LoyaltyPoint", x => x.UserLoyaltyPointID);
                });

            migrationBuilder.CreateTable(
                name: "Users_Membership",
                columns: table => new
                {
                    MembershipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    IsFree = table.Column<bool>(type: "bit", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    MembershipFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MembershipStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CancelStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cancellationdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_Membership", x => x.MembershipId);
                });

            migrationBuilder.CreateTable(
                name: "Users_Profile",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AdminStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShopName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShopURLPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CoverImage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ShopDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PaypalId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsMembershipFree = table.Column<bool>(type: "bit", nullable: true),
                    stripeid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    latitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    longitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    firstname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    aboutshop = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    isprofilecompleted = table.Column<bool>(type: "bit", nullable: true),
                    affiliateid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_Profile", x => x.ProfileId);
                });

            migrationBuilder.CreateTable(
                name: "Vendor_Follow",
                columns: table => new
                {
                    FollowerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    VendorId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor_Follow", x => x.FollowerId);
                });

            migrationBuilder.CreateTable(
                name: "Vendor_Membership_Package",
                columns: table => new
                {
                    Membershipid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Membershiptype = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RecurringPeriod = table.Column<int>(type: "int", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    Fees = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Sortoption = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor_Membership", x => x.Membershipid);
                });

            migrationBuilder.CreateTable(
                name: "Vendor_WareHouse",
                columns: table => new
                {
                    VendorWareHouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WareHoueName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WareHouseContact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    CityPerKM = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CountryPerKM = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OutCountryPerKM = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor_WareHouse", x => x.VendorWareHouseId);
                });

            migrationBuilder.CreateTable(
                name: "Website_Setup",
                columns: table => new
                {
                    WebsiteSetupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ItemDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeductionType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Setup", x => x.WebsiteSetupId);
                });

            migrationBuilder.CreateTable(
                name: "Website_Setup_CMS",
                columns: table => new
                {
                    CMSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CMSKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CMSContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Setup_CMS", x => x.CMSId);
                });

            migrationBuilder.CreateTable(
                name: "Website_Setup_Holiday",
                columns: table => new
                {
                    HolidayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Ispriorrelease = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    todate = table.Column<DateTime>(type: "datetime", nullable: true),
                    daterange = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    profileid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Setup_Holiday", x => x.HolidayId);
                });

            migrationBuilder.CreateTable(
                name: "Website_Setup_LoyaltyPoint",
                columns: table => new
                {
                    LoyaltyPointId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoyaltyPointGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsPurchasedThreshold = table.Column<bool>(type: "bit", nullable: true),
                    PurchaseThresholdAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsDiscountedItemAllowed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Setup_LoyaltyPoint", x => x.LoyaltyPointId);
                });

            migrationBuilder.CreateTable(
                name: "Website_Setup_Product_Detail",
                columns: table => new
                {
                    WebsiteSetupProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebsiteSetupPageId = table.Column<int>(type: "int", nullable: true),
                    SetupProductID = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Setup_Product_Detail", x => x.WebsiteSetupProductId);
                });

            migrationBuilder.CreateTable(
                name: "Website_Setup_Product_Setting",
                columns: table => new
                {
                    WebsiteSetupPageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebsiteSetupPage_Key = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    Backgroundcolor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ProductBoxCount = table.Column<int>(type: "int", nullable: true),
                    ProductViewQty = table.Column<int>(type: "int", nullable: true),
                    Banner = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: true),
                    ShowTitle = table.Column<bool>(type: "bit", nullable: true),
                    ShowBanner = table.Column<bool>(type: "bit", nullable: true),
                    ShowItemAsSlider = table.Column<bool>(type: "bit", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    WebsiteItemSetupId = table.Column<int>(type: "int", nullable: true),
                    SellingTypeId = table.Column<int>(type: "int", nullable: true),
                    PageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsEditable = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Setup_Product_Setting", x => x.WebsiteSetupPageId);
                });

            migrationBuilder.CreateTable(
                name: "Website_Setup_Script",
                columns: table => new
                {
                    Scriptid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Scriptname = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Setup_Script", x => x.Scriptid);
                });

            migrationBuilder.CreateTable(
                name: "Website_Setup_Theme_Option",
                columns: table => new
                {
                    WebsiteSetupThemeOptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeSetupOption = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    WebsiteThemeSelectionId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Setup_Theme_Option", x => x.WebsiteSetupThemeOptionID);
                });

            migrationBuilder.CreateTable(
                name: "Website_Setup_ThemeSelection",
                columns: table => new
                {
                    WebsiteThemeSelectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThemeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThemeCSS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website_Setup_ThemeSelection", x => x.WebsiteThemeSelectionId);
                });

            migrationBuilder.CreateTable(
                name: "Websitesetup_Library",
                columns: table => new
                {
                    WebsitesetupLibraryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AltTag = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ImageLink = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    imagekey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websitesetup_Library", x => x.WebsitesetupLibraryId);
                });

            migrationBuilder.CreateTable(
                name: "Websitesetup_Partner",
                columns: table => new
                {
                    PartnerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    Image = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ParnertName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Insertdate = table.Column<DateTime>(type: "date", nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: true),
                    ProfileID = table.Column<int>(type: "int", nullable: true),
                    sortorder = table.Column<int>(type: "int", nullable: true),
                    partnerurl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    showonhomepage = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websitesetup_Partner", x => x.PartnerID);
                });

            migrationBuilder.CreateTable(
                name: "Product_Basic_V2",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    ProductTypeId = table.Column<int>(type: "int", nullable: true),
                    ProductSellingId = table.Column<int>(type: "int", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProductSEOURL = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CategoryArray = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EANCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BrandName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DetailDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductImage = table.Column<string>(type: "nchar(1000)", fixedLength: true, maxLength: 1000, nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: true),
                    Isoutofstock = table.Column<bool>(type: "bit", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    IsAdminApproved = table.Column<bool>(type: "bit", nullable: true),
                    DigitalFile = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AuctionStartDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuctionEndDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    AuctionStartPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AuctionReservedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ismanageinventory = table.Column<bool>(type: "bit", nullable: true),
                    mincartqty = table.Column<int>(type: "int", nullable: true),
                    maxcartqty = table.Column<int>(type: "int", nullable: true),
                    isfreeshipping = table.Column<bool>(type: "bit", nullable: true),
                    shipping_weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    shipping_length = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    shipping_width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    shipping_height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    shipping_AddonCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    cancelpolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    returnpolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    producttags = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsProductVideo = table.Column<bool>(type: "bit", nullable: true),
                    ProductVideoURL = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsCustomProduct = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StarRating = table.Column<int>(type: "int", nullable: true),
                    followers = table.Column<int>(type: "int", nullable: true),
                    GeneralSetupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Basic_V2", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Basic_V2_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Basic_V2_General_Setup_GeneralSetupId",
                        column: x => x.GeneralSetupId,
                        principalTable: "General_Setup",
                        principalColumn: "GeneralSetupId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Basic_V2_CurrencyId",
                table: "Product_Basic_V2",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Basic_V2_GeneralSetupId",
                table: "Product_Basic_V2",
                column: "GeneralSetupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ads_Detail");

            migrationBuilder.DropTable(
                name: "Ads_PageName");

            migrationBuilder.DropTable(
                name: "Ads_Placement");

            migrationBuilder.DropTable(
                name: "Alphabet");

            migrationBuilder.DropTable(
                name: "Blog_Setup");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "category_master");

            migrationBuilder.DropTable(
                name: "Contact_Master");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "COUNTRY_code");

            migrationBuilder.DropTable(
                name: "Customer_Address");

            migrationBuilder.DropTable(
                name: "Customer_Wishlist");

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
                name: "Email_Setup");

            migrationBuilder.DropTable(
                name: "Email_Setup_Content");

            migrationBuilder.DropTable(
                name: "Email_Setup_Notifications");

            migrationBuilder.DropTable(
                name: "License_Setup");

            migrationBuilder.DropTable(
                name: "Logo");

            migrationBuilder.DropTable(
                name: "MainPageCategory");

            migrationBuilder.DropTable(
                name: "Media_notes");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "NewsLetter");

            migrationBuilder.DropTable(
                name: "Order_Charges_V2");

            migrationBuilder.DropTable(
                name: "Order_Delivery_V2");

            migrationBuilder.DropTable(
                name: "Order_Disbursement");

            migrationBuilder.DropTable(
                name: "Order_Master_V2");

            migrationBuilder.DropTable(
                name: "Order_Status_V2");

            migrationBuilder.DropTable(
                name: "Order_Variation_V2");

            migrationBuilder.DropTable(
                name: "OrderIDGeneration_V2");

            migrationBuilder.DropTable(
                name: "PageCategory");

            migrationBuilder.DropTable(
                name: "PageCategoryDetails");

            migrationBuilder.DropTable(
                name: "Product_Amenities_Options_V2");

            migrationBuilder.DropTable(
                name: "Product_Amenities_View_V2");

            migrationBuilder.DropTable(
                name: "Product_AmenitiesQuestion_V2");

            migrationBuilder.DropTable(
                name: "Product_AttributeOption_V2");

            migrationBuilder.DropTable(
                name: "Product_AttributeQuestion_V2");

            migrationBuilder.DropTable(
                name: "Product_Basic_V2");

            migrationBuilder.DropTable(
                name: "Product_Boost");

            migrationBuilder.DropTable(
                name: "Product_Brands");

            migrationBuilder.DropTable(
                name: "Product_Comparision_V2");

            migrationBuilder.DropTable(
                name: "Product_Coupon");

            migrationBuilder.DropTable(
                name: "Product_Coupon_Child");

            migrationBuilder.DropTable(
                name: "Product_Discount");

            migrationBuilder.DropTable(
                name: "Product_Images");

            migrationBuilder.DropTable(
                name: "Product_Inventory");

            migrationBuilder.DropTable(
                name: "Product_Job_Post");

            migrationBuilder.DropTable(
                name: "Product_Job_Post_Apply");

            migrationBuilder.DropTable(
                name: "Product_Question");

            migrationBuilder.DropTable(
                name: "Product_Question_Answer");

            migrationBuilder.DropTable(
                name: "Product_Recently_Viewed");

            migrationBuilder.DropTable(
                name: "Product_RelatedProducts");

            migrationBuilder.DropTable(
                name: "Product_Review_V2");

            migrationBuilder.DropTable(
                name: "Product_SEO");

            migrationBuilder.DropTable(
                name: "Product_Tags");

            migrationBuilder.DropTable(
                name: "Product_Tax_V2");

            migrationBuilder.DropTable(
                name: "Product_Type_Setup");

            migrationBuilder.DropTable(
                name: "Product_Voucher");

            migrationBuilder.DropTable(
                name: "Setup_AWS");

            migrationBuilder.DropTable(
                name: "Setup_TaxClass");

            migrationBuilder.DropTable(
                name: "SocialMedia");

            migrationBuilder.DropTable(
                name: "Specification_Setup");

            migrationBuilder.DropTable(
                name: "User_Notification");

            migrationBuilder.DropTable(
                name: "UserActivation");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Users_LoyaltyPoint");

            migrationBuilder.DropTable(
                name: "Users_Membership");

            migrationBuilder.DropTable(
                name: "Users_Profile");

            migrationBuilder.DropTable(
                name: "Vendor_Follow");

            migrationBuilder.DropTable(
                name: "Vendor_Membership_Package");

            migrationBuilder.DropTable(
                name: "Vendor_WareHouse");

            migrationBuilder.DropTable(
                name: "Website_Setup");

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
                name: "Website_Setup_ThemeSelection");

            migrationBuilder.DropTable(
                name: "Websitesetup_Library");

            migrationBuilder.DropTable(
                name: "Websitesetup_Partner");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "General_Setup");
        }
    }
}
