using Amazon.S3.Model;
using AMMasterProject.Helpers;
using AMMasterProject.Migrations;
using AMMasterProject.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PayPal.Api;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace AMMasterProject.ViewModel
{
    public class ProductViewModel
    {

       

        #region Product
        public Guid profileguid { get; set; }
        public int ProfileId { get; set; }
        public string Shopurlpath { get; set; }
        public int ProductId { get; set; }

        public Guid ProductGUID { get; set; }
        public string ProductSeourl { get; set; }

        public string ProductImage { get; set; }

        public string ProdutAltText { get; set; }

        public string ProductName { get; set; }

        public string ProductUnit { get; set; }

        public string BrandName { get; set; }

        public string SKU { get; set; }
        public string EANCode { get; set; }
        public string ShortDescription { get; set; }
        public string DetailDescription { get; set; }
        public string ReturnPolicy { get; set; }
        public string CancelPolicy { get; set; }

        public bool IsOutofStock { get; set; }
        public bool IsVideo { get; set; }
        public List<ProductVideoMetaData> ListofVideo { get; set; }
       

        public bool IsManagedInventory { get; set; }
        public int MinQty { get; set; }
        public int MaxQty { get; set; }

        public string Currency { get; set; }
        public decimal Price { get; set; }

        public decimal ActualPrice { get; set; }
        public int ActualCurrency { get; set; }


        public bool IsDiscounted { get; set; }

        public decimal DiscountAmount { get; set; }


        public decimal PriceBeforeDiscount { get; set; }

        //public decimal OldPrice { get; set; }


        public string insertdate { get; set; }



          public bool ispublish { get; set; }

        public string? itemaddress { get; set; }
        public string? itemcontact { get; set; }
        public string? itememail { get; set; }



        public int GeneralSetupId { get; set; }
        public string ListingType { get; set; }
     

        public string SellingType { get; set; }

       



        public string wishlistgroupname { get; set; }

        public int IsAttributeExist { get; set; }

       

        public List<ProductViewModel> ProductList { get; set; }
        public ProductViewModel ProductDetail { get; set; }

        public List<ProductAttributeViewModel> ProductAttributeViewModel { get; set; }

    

        public ProductShippingMetaData productShippingMetaData { get; set; }

        #endregion

        #region ProductImages
        public List<ItemSliderViewModel> ListofImages { get; set; }
        #endregion


        #region Category

        public string Categoryarrary { get; set; }

        public List<ProductCategoryMetaData> ListProductCategoryMetaData { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        #endregion


        #region Amenity

       public  AmenitiesViewModel AmenityViewModel { get; set; }

        public List<ProductAmenitiesMetaData> ListAmenityMetaData { get; set; }


        //obselete
        public List<ProductViewModel> ProductAmenityHeader { get; set; }

        //obselect
        public List<ProductViewModel> ProductAmenityChild { get; set; }
        public string AmenitiesHeader { get; set; }
        public int Amenitiesid { get; set; }

        public string AmenitiesOptionName { get; set; }
        public string AmenitiesIcon { get; set; }


        public int OptionAmenityId { get; set; }
        #endregion



        #region Others
        //public int Totalreviews { get; set; }

        public int Totalorders { get; set; }
        //public int Starrating_average { get; set; }
        //public int Followers { get; set; }

        public bool loginuserfavorite { get; set; }

        public int Favorite { get; set; }

        public int ComparisionExist { get; set; }
        #endregion

        #region UserProfile
        public UserOtherMetaData userothermetadata { get; set; }





        #endregion
        public ItemOtherMetaData itemothermetadata { get; set; }

        public List<ProductDigitalMetaData> itemdigitalmetadata { get; set; }



        public List<ProductOtherPropertiesMetaData> itemotherpropertieslist { get; set; }

    }


    #region Attribute-ViewModel
    public class ProductAttributeViewModel
    {
        public ProductAttributeQuestionV2 Question { get; set; }
        public List<AttributeOptionViewModel> Options { get; set; }
    }

    public class AttributeOptionViewModel
    {
        public decimal ActualAttributeprice { get; set; }
        public decimal ConversionAttributeprice { get; set; }
        public string OptionText { get; set; }  

        public string Attributeimage { get; set; }
    }

    #endregion


    #region CategoryViewModel
    public class CategoryViewModel
    {
      public int  ParentCategoryId { get; set; }
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Icon { get; set; }
        public string Banner { get; set; }

        public string Urlpath { get; set; }
        public List<SecondCategoryViewModel> SecondLevel { get; set; }

       

    }

    public class SecondCategoryViewModel
    {
        public int ParentCategoryId { get; set; }
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Icon { get; set; }

        public List<ThirdCategoryViewModel> ThirdLevel { get; set; }
    }

    public class ThirdCategoryViewModel
    {
        public int ParentCategoryId { get; set; }
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Icon { get; set; }


    }
    #endregion

    #region ProductAmenitiesQuestion
    public class ProductReviews
    {


        public string ReviewName { get; set; }


        public string Remarks { get; set; }
        public int rating { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd,yyyy}")]
        public string insertdate { get; set; }
        public string attachment { get; set; }
    }
    #endregion

    #region ProductQAView
    public class ProductQAViewModel
    {
        public int QuestionId { get; set; }
        public int? productid { get; set; }
        public string question { get; set; }
        public string answer { get; set; }


    
        public DateTime? questiondate { get; set; }

       
        public DateTime? replydate { get; set; }


        public string name { get; set; }

        public string replyby { get; set; }


        public int? postedbyid { get; set; }

        public int? sellerid { get; set; }

        public string usertype { get; set; }


        public ProductBasicMetaData ItemMetaData { get; set; }

      
    }
    #endregion


    #region Wislist



    public class WishlistViewModel
    {

     

        public IEnumerable<SelectListItem> wishlistGroup { get; set; }

        public List<ProductViewModel> productViewModel { get; set; }
    }
    #endregion


    #region CouponChild
    public class CouponChildViewModel
    {
        public int ReferenceId { get; set;}
        public string ReferenceName { get; set; }   

       
    }

    #endregion


    #region ListingItem - For Only Form Validations
    public class ProductBasicViewModel
    {
        [Key]
        public int ItemId { get; set; }

        [Column("ProductGUID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ItemGuid { get; set; }

        [DisplayName("Product Type")]
        [Required(ErrorMessage = "Product Type Is Required")]
        public string  SellingType { get; set; }   //sell, classified, auction penny aucntion



        [DisplayName("Selling Type")]
        [Required(ErrorMessage = "Selling Type Is Required")]
        public string ListingType { get; set; }///physical digital service course


        [StringLength(100)]
        [DisplayName("Name")]
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }


        [StringLength(1000)]
      
        public string SEOURL { get; set; }




        [StringLength(50)]
        [Required(ErrorMessage = "All Categories Required")]
        [DisplayName("Category")]
        public string? CategoryArray { get; set; }

        [StringLength(200)]
        [DisplayName("Short Description")]
        [Required(ErrorMessage = "Short Description Is Required")]
        public string ShortDescription { get; set; }


        [DisplayName("Currency")]
        [Required(ErrorMessage = "Currency Is Required")]
        public int CurrencyId { get; set; }



        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Old Price")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number with up to 2 decimal places")]
        public decimal Price { get; set; }


        [StringLength(1000)]
        [DisplayName("Item Image")]
        [Required(ErrorMessage = "Item Image Is Required")]
        public string ProductImage { get; set; }


        public bool IsPublish { get; set; }

        [StringLength(100)]
        [DisplayName("Contact Number")]
        [Required(ErrorMessage = "Contact Number Is Required")]
        public string ContactNumber { get; set; }


        [StringLength(300)]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }

     
        [StringLength(1000)]
        [DisplayName("Address")]
        
        public string? Address { get; set; }

       


        //[StringLength(1000)]
        //[DisplayName("Digital File")]
        //[Required(ErrorMessage = "File Is Required")]
        //public string DigitalFileLink { get; set; }


        
        [DisplayName("Start Date")]
        [Required(ErrorMessage = "Start Date Is Required")]
        public DateTime StartDate { get; set; }


        [DisplayName("End Date")]
        [Required(ErrorMessage = "End Date Is Required")]
        public DateTime EndDate { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Start Price")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number with up to 2 decimal places")]
        [Required(ErrorMessage = "Start Price Is Required")]
        public decimal StartPrice { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("End Price")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number with up to 2 decimal places")]
        [Required(ErrorMessage = "End Price Is Required")]
        public decimal EndPrice { get; set; }

        public bool IsIncrementByUser { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Increment Amount")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number with up to 2 decimal places")]
        [Required(ErrorMessage = "Increment Amount Is Required")]
        public decimal IncrementAmount { get; set; }




        [DisplayName("No of Real Bots")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid whole number")]
        [Required(ErrorMessage = "Real Bids Is Required")]
        public int NoofRealBids { get; set; }

   
        public string? BotArray { get; set; }

        [StringLength(20)]
        [DisplayName("Unit")]

        public string? Unit { get; set; }


        [StringLength(300)]
        [DisplayName("BrandName")]
        public string? BrandName { get; set; }


       

        [Column("SEO_MetaTItle")]
        [StringLength(200)]
        //[Required(ErrorMessage = "SEO Title Is Required")]
        public string? SeoMetaTitle { get; set; }

        [Column("SEO_Keywords")]
        [StringLength(300)]
        //[Required(ErrorMessage = "SEO Keyword Is Required")]
        public string? SeoKeywords { get; set; }

        [Column("SEO_Metadescription")]
        [StringLength(500)]
        //[Required(ErrorMessage = "SEO Description Is Required")]
        public string? SeoMetadescription { get; set; }

        public List<ProductOtherPropertiesForListingViewModel> ProductOtherProperties { get; set; }

        //public ProductDigitalMetaData ProductDigitalMetaData { get; set; }

        //public List<ProductDigitalMetaData> ProductDigitalMetaDataList { get; set; }
    }


    public class ProductDetailViewModel
    {

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description Is Required")]
        public string DetailDescription { get; set; }

       
        [StringLength(100)]
        [DisplayName("SKU")]
        
        public string? Sku { get; set; }

        [StringLength(13)]
        [DisplayName("Eancode")]

        public string? Eancode { get; set; }




    }

    public class ProductPolicyViewModel
    {
        [DisplayName("ReturnPolicy")]
        [Required(ErrorMessage = "Return Policy Is Required")]
        public string ReturnPolicy { get; set; }


        [DisplayName("Cancellation Policy")]
        [Required(ErrorMessage = "Cancellation Policy Is Required")]
        public string CancellationPolicy { get; set; }

    }


    public class ProductShippingViewModel
    {

      
      
        public bool IsFreeShipping { get; set; }


        [Required(ErrorMessage = "Shipping Weight Is Required")]
        public string ShippingWeight { get; set; }

        [Required(ErrorMessage = "Shipping Length Is Required")]
        public string ShippingLength { get; set; }

        [Required(ErrorMessage = "Shipping Width Is Required")]
        public string ShippingWidth { get; set; }

        [Required(ErrorMessage = "Shipping Height Is Required")]
        public string ShippingHeight { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Old Price")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number with up to 2 decimal places")]
        public decimal ShippingAddOnCharges { get; set; }
    }


    public class ProductImagesViewModel
    {
        public int ID { get; set; }
        public string Image { get; set; }

        public string ImageName { get; set; }

        
    }


    public class ProductAmenitiesViewModel
    {
        public int ID { get; set; }
        public int OptionID { get; set; }

        public int AmenityID { get; set; }


    }



    public class ProductRelatedViewModel
    {
        public int ID { get; set; }
        public int RelatedProductid { get; set; }

        public int RelatedType { get; set; }
    }



    public class ProductVideoViewModel
    {

        public int ID { get; set; }


        [Required(ErrorMessage = "Select Provider")]
        public string Provider { get; set; }


        [Required(ErrorMessage = "Source Is Required")]
        public string Source { get; set; }


        [Required(ErrorMessage = "Poster Is Required")]
        public string Poster { get; set; }

      
        public string URL { get; set; }

    }

    //this is for admin to create in websetting
    public class ProductOtherPropertiesViewModel
    {

        public int ID { get; set; }


        [Required(ErrorMessage = "Label Provider")]
        public string LabelName { get; set; }



      


    }


    //this is for item listing basic page
    public class ProductOtherPropertiesForListingViewModel
    {

        public int ID { get; set; }


        [Required(ErrorMessage = "Label Provider")]
        public string LabelName { get; set; }


        [Required(ErrorMessage = "Value Provider")]
        public string ValueName { get; set; }



    }
    public class ProductInventoryViewModel
    {
        [DisplayName("SKU")]
        [Required(ErrorMessage = "SKU Is Required")]
        public string SKU { get; set; }


        [DisplayName("EAN Code")]
        //[Required(ErrorMessage = "EAN Code Is Required")]
        public string? EANCode { get; set; }

        [DisplayName("Min QTY")]
        [Required(ErrorMessage = "Min QTY Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Min QTY must be a positive whole number")]
        public int MINQTY { get; set; }


        [DisplayName("Max QTY")]
        [Required(ErrorMessage = "Max QTY Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Max QTY must be a positive whole number")]
        public int MAXQTY { get; set; }


        public bool IsManagedInventory { get; set; }

        public bool IsOutOfStock { get; set; }

    }

    #endregion



    #region ItemHomePageDesign
    public class ItemHomePageDesignViewModel
    {

        public int ItemDesignId { get; set; }



   


        [StringLength(100)]
        [DisplayName("Title")]
        [Required(ErrorMessage = "Title Is Required")]
        public string Title { get; set; }



        [DisplayName("Sort Order")]
        [Required(ErrorMessage = "Sort Order Is Required")]
        public int SortOrder { get; set; }

        public bool IsPublish { get; set; }






        public ItemHomePageDesignMetaData  ItemDesignMetaData { get; set; }


        public ItemHomePageDesignSelectionMetaData SelectionMetaData { get; set; }


       

    }
    #endregion


    #region MetaDataModels


    #region ProductBasic


    public class ProductBasicMetaData
    {
        public int SellingTypeID { get; set; }
        public int ListingTypeID { get; set; }
        //public int SellingType { get; set; }
        //public int ListingType { get; set; }
        public string Name { get; set; }

        public string SEOURL { get; set; }
        public string ShortDescription { get; set; }
        public int CurrencyId { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        public string ProductCategoryArray { get; set; }

        public string? Unit { get; set; }

        public string? Brand { get; set; }



      
        public string? SeoMetaTitle { get; set; }

        public string? SeoKeywords { get; set; }

        public string? SeoMetadescription { get; set; }
    }

    public class ProductCategoryMetaData
    {
        public int ID { get; set; }
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }

    public class ProductClassifiedMetaData
    {

        public string? Address { get; set; }
        
        public string? Latitude { get; set; }

        public string? Longitude { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
    }

    public class ProductDigitalMetaData
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string DigitalLink { get; set; }
    }

    public class ProductSellerDiscountMetaData
    {
      public DiscountSellerAlwaysMetaData DiscountSellerAlwaysMetaData { get; set; }
      public List<DiscountSellerCustomMetaData> DiscountSellerCustomMetaData { get; set; }



    }

    public class DiscountSellerAlwaysMetaData
    {
        public bool IsAlways { get; set; }

        [Required(ErrorMessage = "DiscountType is required")]
      
        public string DiscountType { get; set; } // value percentage

        [Required(ErrorMessage = "DiscountAmount is required")]
        public decimal DiscountAmount { get; set; }
    }

    public class DiscountSellerCustomMetaData
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "Star Date is required")]
        public DateTime DiscountStartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateTime DiscountEndDate { get; set; }
      

        [Required(ErrorMessage = "DiscountType is required")]
     
        public string DiscountType { get; set; } //value percentage

        [Required(ErrorMessage = "DiscountAmount is required")]
        public decimal DiscountAmount { get; set; }

        public bool IsActive { get; set; }
    }
    public class ProductAuctionMetaData
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal StartPrice { get; set; }
        public decimal EndPrice { get; set; }
        public bool IsIncrementByUser { get; set; }
        public decimal IncrementAmount { get; set; }
    }

    public class ProductPennyAuctionMetaData
    {
        public int NoofRealBids { get; set; }
        public List<ProductBotItemMetaData> BotList { get; set; }
    }

    public class ItemSliderViewModel
    {
        public int ID { get; set; }
        public string Image { get; set; }

        public string Poster { get; set; }
        public string Source { get; set; }
    }

    public class ProductBotItemMetaData
    {
        public int ID { get; set; }
        public int BotId { get; set; }
    }
    public class ImageItemMetaData
    {
        public int ID { get; set; }
        public string Image { get; set; }

        public string ImageName { get; set; }
    }

    public class ProductAmenitiesMetaData
    {
        public int ID { get; set; }
        public int OptionID { get; set; }

        public int AmenityID { get; set; }
    }


    public class ProductRelatedMetaData
    {
        public int ID { get; set; }
        public int RelatedProductid { get; set; }

        public int RelatedType { get; set; }  //0 RP  , 1= CP
    }

    #endregion


    #region ProductDetail


    public class ProductDetailMetaData
    {
        
        public string DetailDescription { get; set; }

        
        //public string? Unit { get; set; }

    

        //public string? Sku { get; set; }

      
        //public string? Eancode { get; set; }


      
        //public string? BrandName { get; set; }
    }

    #endregion


    #region ProductPolicy
    public class ProductOtherPropertiesMetaData
    {
        public int ID { get; set; }
        public string LabelName { get; set; }



        public string ValueName { get; set; }
    }

    #endregion



    #region ProductPolicy
    public class ProductPolicyMetaData
    {
        
        public string ReturnPolicy { get; set; }


     
        public string CancellationPolicy { get; set; }
    }

    #endregion

    #region Product-Shipping

    public class ProductShippingMetaData
    {
        
        public bool IsFreeShipping { get; set; }


        public string ShippingWeight { get; set; }
        public string ShippingLength { get; set; }

        public string ShippingWidth { get; set; }
        public string ShippingHeight { get; set; }

        public decimal ShippingAddOnCharges { get; set; }

    }
    #endregion

    #region Product-Video

    public class ProductVideoMetaData
    {

       public int ID { get; set; }

       public string Provider { get; set; }

       public string Source { get; set; }


        public string Poster { get; set; }

        public string URL { get; set; }

    }
    #endregion

    #region Product-Inventory
    public class ProductInventoryMetaData
    {

        public bool IsOutOfStock { get; set; }
        public bool IsManagedInventory { get; set; }
        public string SKU { get; set; }


        public string EANCode { get; set; }

        public int MINQTY { get; set; }


       
        public int MAXQTY { get; set; }

    }

    #endregion


    public class ItemMetaDataUpsert
    {
        public int ID { get; set; }
        public Guid ItemGUID { get; set; }

        public int ProfileID { get; set; }

        public DateTime InsertDate { get; set; }

        public bool IsPublish { get; set; }

        public bool IsAdminLocked { get; set; }

      


        public ProductBasicMetaData ProductBasicMetaData { get; set; }

     


        public ProductClassifiedMetaData ProductClassifiedMetaData { get; set; }
      
       

        public ProductAuctionMetaData ProductAuctionMetaData { get; set; }
        public ProductPennyAuctionMetaData ProductPennyAuctionMetaData { get; set; }

       

      
    }

    public class ItemJsonList
    {
        public int ID { get; set; }
        public Guid ItemGUID { get; set; } 

        public int ProfileID { get; set; }

        public DateTime InsertDate { get; set; }

        public bool IsPublish { get; set; }

        public bool IsAdminLocked { get; set; }

        public string ImagesMetaData { get; set; }
        public string VideoMetaData { get; set; }

        public string AmenityMetaData { get; set; }

     
        public int IsAttributeExist { get; set; }

        //public string productDigitalMetaData { get; set; }

        //public decimal AverageRating { get; set; }
       
        public ProductBasicMetaData ProductBasicMetaData { get; set; }

        public ProductDetailMetaData ProductDetailMetaData { get; set; }
        //public List<ProductCategoryMetaData> ProductCategoryMetaData { get; set; }


        public ProductClassifiedMetaData ProductClassifiedMetaData { get; set; }
        public List<ProductDigitalMetaData> ProductDigitalMetaData { get; set; }
        public List<ProductRelatedMetaData> ProductRelatedMetaData { get; set; }

        
        public ProductAuctionMetaData ProductAuctionMetaData { get; set; }
        public ProductPennyAuctionMetaData ProductPennyAuctionMetaData { get; set; }
       
        public ProductPolicyMetaData  productPolicyMetaData { get; set; }
      
        
        public ProductShippingMetaData productShippingMetaData { get; set; }

      
        public List<ImageItemMetaData> imageItemMetaDatas { get; set; }

        public List<ProductVideoMetaData> productVideoMetaDatas { get; set; }

        public List<ProductAmenitiesMetaData> AmenityMetaDatas { get; set; }

     
        
        public ProductInventoryMetaData productInventoryMetaData { get; set; }


       public ItemOtherMetaData ItemOtherMetaData { get; set; }  


        public string SellerDiscountMetaData { get; set; }
    }


    public class HomePageFirstLevel: ItemHomePageDesignViewModel
    {
       


        //in the list generate only those record which itemdesignid exist in itempagedesignchild
        public List<HomePageSecondLevel> ItemPageSecondLevel { get; set; }

        public List<ProductViewModel> productviewModel { get; set; }

    }
    public class HomePageSecondLevel : ItemPageDesignChild
    {
        //public List<ProductViewModel> productviewModel { get; set; }
    }
    


    #region Productsetup


    public class SellingTypeMetaData
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

         public bool IsPublish { get; set; }

        public List<ListingTypeMetaData> ListingType { get; set; }
    }

    public class ListingTypeMetaData
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPublish { get; set; }
    }

    #endregion


    #region ItemHomePageDesignMetaData
    public class ItemHomePageDesignMetaData
    {

     


     
        public int SellingType { get; set; }   //sell, classified, auction penny aucntion










        public int Style { get; set; }



        public string PreselectedCategory { get; set; }


        public int NoofItemsDisplay { get; set; }




        public string? Background { get; set; }




        public string? Banner { get; set; }


        public bool IsURL { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid URL")]
        public string URL { get; set; }

        public bool ShowTitle { get; set; }
        public bool ShowBanner { get; set; }

        public bool ShowItemSlider { get; set; }



    }


    public class ItemHomePageDesignSelectionMetaData
    {
        public int SelectionID { get; set; }

        public string Name { get; set; }
    }
    #endregion


    #region ItemOtherMetaDataViewModel
    public class ItemOtherMetaData
    {

        public int TotalCompletedOrders { get; set; }
        public int TotalReviews { get; set; }
        public decimal ItemAverageRating { get; set; }

        public int TotalViews { get; set; }

        public int TotalClicks { get; set;}


    }
    #endregion

    #region BoostMetaDataViewModel
    public class BoostOtherMetaData
    {

      
        public int TotalViews { get; set; }

        public int TotalClicks { get; set; }


    }
    #endregion

    #endregion



    #region Category-ViewModel

    public class CategorViewModel
    {
        public int CategoryId { get; set; }


        public int ParentCategoryId { get; set; }
        public string Urlpath { get; set; }

        public string Icon   { get; set; }   
        public string CategoryName { get; set; }

        public int Sortnumber { get; set; } 

        public int LevelNumber { get; set; }

        public bool Selected { get; set; }
    }


    #endregion


    #region AmenitiesViewModel
    public class AmenitiesViewModel
    {
        public List<AmenitiesHeader> amenityheader { get; set; }
        public List<AmenitiesChild> amenitychild { get; set; }

    }


    public class AmenitiesHeader
    {
        public string  AmenityName { get; set; }
         public int Amenitiesid { get; set; }
    }

    public class AmenitiesChild
    {
        public string AmenitiesOptionName { get; set; }
        public string AmenitiesIcon { get; set; }
        public int OptionAmenityId { get; set; }
    }
    #endregion
}
