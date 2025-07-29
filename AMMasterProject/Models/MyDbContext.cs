using System;
using System.Collections.Generic;
using AMMasterProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

public partial class MyDbContext : DbContext
{
   

    private readonly IConfiguration _configuration;

    public MyDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        
        if (!optionsBuilder.IsConfigured)
        {
            var connectionStringName = "DefaultConnection";

            // Get the theme setting from the configuration
            var theme = _configuration["Theme"];
            if(theme == "EzyCommerce")
            {
                connectionStringName = "EzyCommerceLive";
            }

          

            // Use the selected connection string
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString(connectionStringName));
        }
    }

    public virtual DbSet<Advert> Advert { get; set; }
   

    public virtual DbSet<Alphabet> Alphabets { get; set; }


    public virtual DbSet<AnnouncementNotification> AnnouncementNotification { get; set; }

    //public virtual DbSet<BlogSetup> BlogSetups { get; set; }


    public virtual DbSet<Blogging> Bloggings { get; set; }

    public virtual DbSet<BlogCategory> BlogCategories { get; set; }

    //public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<CategoryMaster> CategoryMasters { get; set; }

    //public virtual DbSet<CompanySetup> CompanySetups { get; set; }
    //public virtual DbSet<ContactMaster> ContactMasters { get; set; }

    //public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CountryCode> Countries { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public virtual DbSet<CustomerWishlist> CustomerWishlists { get; set; }


    public virtual DbSet<Career> Careers { get; set; }
    public virtual DbSet<CareerCategory> CareerCategories { get; set; }

    public virtual DbSet<CareerApplication> CareerApplications { get; set; }

    //public virtual DbSet<DonationMaster> DonationMasters { get; set; }

    //public virtual DbSet<DonationReceived> DonationReceiveds { get; set; }

    //public virtual DbSet<EcollabPostComment> EcollabPostComments { get; set; }

    //public virtual DbSet<EcollabPostLike> EcollabPostLikes { get; set; }

    //public virtual DbSet<EcollabPostQuestion> EcollabPostQuestions { get; set; }

    //public virtual DbSet<EmailSetup> EmailSetups { get; set; }

    //public virtual DbSet<EmailSetupContent> EmailSetupContents { get; set; }

    //public virtual DbSet<EmailSetupNotification> EmailSetupNotifications { get; set; }


    public virtual DbSet<EventCategory> EventCategories { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<FAQ> FAQs { get; set; }

    public virtual DbSet<FormDetail> FormDetails { get; set; }
    

    //public virtual DbSet<GeneralSetup> GeneralSetups { get; set; }



    public virtual DbSet<ItemListing> ItemListings { get; set; }
    public virtual DbSet<ItemPageDesign> ItemPageDesign { get; set; }

    public virtual DbSet<ItemPageDesignChild> ItemPageDesignChild { get; set; }

    //public virtual DbSet<ItemReview> ItemReview { get; set; }

    //public virtual DbSet<LicenseSetup> LicenseSetups { get; set; }

  

  

  

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageMaster> MessageMaser { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    //public virtual DbSet<NewsLetter> NewsLetters { get; set; }


    //public virtual DbSet<NotificationContent> NotificationContent { get; set; }


    public virtual DbSet<NotificationRelay> NotificationRelays { get; set; }

  
    public virtual DbSet<OrderMaster> OrderMasters { get; set; }
    

    public virtual DbSet<PageCategory> PageCategories { get; set; }
    public virtual DbSet<PageName> PageNames { get; set; }

    //public virtual DbSet<PageDetail> PageDetails { get; set; }

    
    
    public virtual DbSet<ProductAmenitiesOptionsV2> ProductAmenitiesOptionsV2s { get; set; }

    public virtual DbSet<ProductAmenitiesQuestionV2> ProductAmenitiesQuestionV2s { get; set; }

    //public virtual DbSet<ProductAmenitiesViewV2> ProductAmenitiesViewV2s { get; set; }

    public virtual DbSet<ProductAttributeOptionV2> ProductAttributeOptionV2s { get; set; }

    public virtual DbSet<ProductAttributeQuestionV2> ProductAttributeQuestionV2s { get; set; }

    //public virtual DbSet<ProductBasicV2> ProductBasicV2s { get; set; }

    public virtual DbSet<ProductBoost> ProductBoosts { get; set; }

    //public virtual DbSet<ProductBrand> ProductBrands { get; set; }

    //public virtual DbSet<ProductComparisionV2> ProductComparisionV2s { get; set; }

    public virtual DbSet<ProductCoupon> ProductCoupons { get; set; }

    public virtual DbSet<ProductCouponChild> ProductCouponChildren { get; set; }

    //public virtual DbSet<ProductDiscount> ProductDiscounts { get; set; }

    //public virtual DbSet<ProductImage> ProductImages { get; set; }

    //public virtual DbSet<ProductInventory> ProductInventories { get; set; }

    //public virtual DbSet<ProductJobPost> ProductJobPosts { get; set; }

    //public virtual DbSet<ProductJobPostApply> ProductJobPostApplies { get; set; }

    public virtual DbSet<ProductQuestion> ProductQuestions { get; set; }

    public virtual DbSet<ProductQuestionAnswer> ProductQuestionAnswers { get; set; }

    //public virtual DbSet<ProductRecentlyViewed> ProductRecentlyVieweds { get; set; }

    public virtual DbSet<ProductRelatedProduct> ProductRelatedProducts { get; set; }

    //public virtual DbSet<ProductReviewV2> ProductReviewV2s { get; set; }

  
    //public virtual DbSet<ProductTag> ProductTags { get; set; }

    public virtual DbSet<ProductTaxV2> ProductTaxV2s { get; set; }

    //public virtual DbSet<ProductTypeSetup> ProductTypeSetups { get; set; }

    //public virtual DbSet<ProductVoucher> ProductVouchers { get; set; }

   
    public virtual DbSet<RevenueCreditPackage> RevenueCreditPackage { get; set; } 
    
    public virtual DbSet<RevenueSubscriptionPackage> RevenueSubscriptionPackage { get; set; }


    //public virtual DbSet<SetupAw> SetupAws { get; set; }

    public virtual DbSet<SetupTaxClass> SetupTaxClasses { get; set; }

  
    //public virtual DbSet<SpecificationSetup> SpecificationSetups { get; set; }

    

    //public virtual DbSet<UserActivation> UserActivations { get; set; }

    //public virtual DbSet<UserNotification> UserNotifications { get; set; }

    //public virtual DbSet<UsersLoyaltyPoint> UsersLoyaltyPoints { get; set; }

    //public virtual DbSet<UsersMembership> UsersMemberships { get; set; }

    public virtual DbSet<UsersProfile> UsersProfiles { get; set; }
    public virtual DbSet<UserWallet> UserWallets { get; set; }

    //public virtual DbSet<UserMembership> UserMemberships { get; set; }

    public virtual DbSet<VendorFollow> VendorFollows { get; set; }

    //public virtual DbSet<VendorMembershipPackage> VendorMembershipPackages { get; set; }

    //public virtual DbSet<VendorWareHouse> VendorWareHouses { get; set; }

    //public virtual DbSet<WebsiteSetup> WebsiteSetups { get; set; }

    //public virtual DbSet<WebsiteSetupCm> WebsiteSetupCms { get; set; }

    //public virtual DbSet<WebsiteSetupHoliday> WebsiteSetupHolidays { get; set; }

    //public virtual DbSet<WebsiteSetupLoyaltyPoint> WebsiteSetupLoyaltyPoints { get; set; }

    //public virtual DbSet<WebsiteSetupProductDetail> WebsiteSetupProductDetails { get; set; }

    //public virtual DbSet<WebsiteSetupProductSetting> WebsiteSetupProductSettings { get; set; }

    //public virtual DbSet<WebsiteSetupScript> WebsiteSetupScripts { get; set; }
    public virtual DbSet<WebsiteSetupSocialMedia> WebsiteSetupSocialMedia { get; set; }


    //public virtual DbSet<WebsiteSetupThemeOption> WebsiteSetupThemeOptions { get; set; }

    //public virtual DbSet<WebsiteSetupThemeSelection> WebsiteSetupThemeSelections { get; set; }

    //public virtual DbSet<WebsitesetupLibrary> WebsitesetupLibraries { get; set; }

    public virtual DbSet<WebsitesetupPartner> WebsitesetupPartners { get; set; }

    public virtual DbSet<WebsitesetupTeam> WebsitesetupTeams { get; set; }


    public virtual DbSet<Websetting> Websettings { get; set; }


    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=DESKTOP-0K5LGQH;Database=ammultivendorv3;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        //  modelBuilder.Entity<GeneralSetup>()
        //.HasMany(g => g.ProductBasicV2s)
        //.WithOne(p => p.ProductType)
        //.HasForeignKey(p => p.GeneralSetupId)
        //.OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<AnnouncementNotification>(entity =>
        {
            entity.Property(e => e.AnnouncementGUID).HasDefaultValueSql("(newid())");
        });

        //modelBuilder.Entity<ContactMaster>(entity =>
        //{
        //    entity.Property(e => e.ContactMasterGuid).HasDefaultValueSql("(newid())");
        //});

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.Property(e => e.CustomerAddressGuid).HasDefaultValueSql("(newid())");
        });

       
        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.ChatId).HasDefaultValueSql("(newid())");
        });

       

     

        modelBuilder.Entity<ProductAmenitiesOptionsV2>(entity =>
        {
            entity.HasKey(e => e.ProductAmenitiesOptionId).HasName("PK_Product_Amenities_Options");
        });

        modelBuilder.Entity<ProductAmenitiesQuestionV2>(entity =>
        {
            entity.HasKey(e => e.ProductAmenitiesId).HasName("PK_Product_Amenities_V2");
        });

        modelBuilder.Entity<ProductAttributeQuestionV2>(entity =>
        {
            entity.HasKey(e => e.ProductAttributeId).HasName("PK_ProductAttribute_Master");

            entity.Property(e => e.ProductAttributeGuid).HasDefaultValueSql("(newid())");
        });

     

        modelBuilder.Entity<ItemListing>(entity =>
        {
           
            entity.Property(e => e.ItemGuid).HasDefaultValueSql("(newid())");
           
        });


        //modelBuilder.Entity<ProductBrand>(entity =>
        //{
        //    entity.HasKey(e => e.ProductBrandId).HasName("PK_Product_Brand");
        //});

        //modelBuilder.Entity<ProductComparisionV2>(entity =>
        //{
        //    entity.Property(e => e.ProductComparisionGuid).HasDefaultValueSql("(newid())");
        //});

        modelBuilder.Entity<ProductCoupon>(entity =>
        {
            entity.Property(e => e.ProductCouponGuid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<ProductCouponChild>(entity =>
        {
            entity.Property(e => e.ProductCouponChildGuid).HasDefaultValueSql("(newid())");
        });

      


        modelBuilder.Entity<ProductQuestion>(entity =>
        {
            entity.HasKey(e => e.ProductQaid).HasName("PK_Product_Question_Answer");
        });

        modelBuilder.Entity<ProductQuestionAnswer>(entity =>
        {
            entity.HasKey(e => e.ProductAnswerId).HasName("PK_Product_Question_Answer_1");
        });

        modelBuilder.Entity<RevenueCreditPackage>()
       .Property(p => p.CreditAmount)
       .HasPrecision(10, 2);

        modelBuilder.Entity<RevenueSubscriptionPackage>()
    .Property(p => p.CreditAmount)
    .HasPrecision(10, 2);

        modelBuilder.Entity<SetupTaxClass>(entity =>
        {
            entity.Property(e => e.TaxClassGuid).HasDefaultValueSql("(newid())");
        });

        //modelBuilder.Entity<User>(entity =>
        //{
        //    entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
        //});

        //modelBuilder.Entity<UserActivation>(entity =>
        //{
        //    entity.Property(e => e.UserId).ValueGeneratedNever();
        //});

        modelBuilder.Entity<UsersProfile>(entity =>
        {
            entity.Property(e => e.ProfileGuid).HasDefaultValueSql("(newid())");
        });

        //modelBuilder.Entity<VendorMembershipPackage>(entity =>
        //{
        //    entity.HasKey(e => e.Membershipid).HasName("PK_Vendor_Membership");
        //});

        //modelBuilder.Entity<WebsiteSetupLoyaltyPoint>(entity =>
        //{
        //    entity.Property(e => e.LoyaltyPointGuid).HasDefaultValueSql("(newid())");
        //});

        modelBuilder.Entity<WebsitesetupPartner>(entity =>
        {
            entity.Property(e => e.PartnerGuid).HasDefaultValueSql("(newid())");
        });


    //    modelBuilder.Entity<ItemReview>()
    //  .Property(x => x.BuyerAverageRating)
    //  .HasColumnType("decimal(18, 2)");

    //    modelBuilder.Entity<ItemReview>()
    //.Property(x => x.SellerAverageRating)
    //.HasPrecision(18, 2);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
