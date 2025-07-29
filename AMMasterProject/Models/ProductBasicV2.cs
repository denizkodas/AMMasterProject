using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace AMMasterProject;

[Table("Product_Basic_V2")]
public partial class ProductBasicV2
{


    #region Required


    #region int

    [Key]
    public int ProductId { get; set; }

    [Column("ProductGUID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? ProductGuid { get; set; }


   
    public int ProductTypeId { get; set; }




    
    public int ProductSellingId { get; set; }

   
    public int CategoryId { get; set; }


   
    public int CurrencyId { get; set; }

    public virtual Currency? Currency { get; set; }
    #endregion

    #region string
   
    public string ProductName { get; set; }



  
    public string ShortDescription { get; set; }

    
    public string ProductImage { get; set; }



  
    public string? CategoryArray { get; set; }

    #endregion



    #region decimal
  
    public decimal Price { get; set; }
    #endregion











    #endregion

    #region String
    [Column("ProductSEOURL")]
    [StringLength(300)]
    public string? ProductSeourl { get; set; }

    [StringLength(50)]
    [DisplayName("Unit")]
    public string? Unit { get; set; }


   

    [Column("SKU")]
    [StringLength(50)]
    [DisplayName("SKU")]
    public string? Sku { get; set; }

    [Column("EANCode")]
    [StringLength(50)]
    [DisplayName("EAN Code")]
    public string? Eancode { get; set; }

    [StringLength(100)]

    [DisplayName("Brand Name")]
    public string? BrandName { get; set; }


    [DisplayName("Description")]
    public string? DetailDescription { get; set; }

    [StringLength(1000)]

    [DisplayName("Digital File")]
    public string? DigitalFile { get; set; }



    [Column("cancelpolicy")]
    [DisplayName("Cancel Policy")]
    public string? Cancelpolicy { get; set; }

    [Column("returnpolicy")]
    [DisplayName("Return Policy")]
    public string? Returnpolicy { get; set; }

    [Column("producttags")]
    [StringLength(2000)]
    [DisplayName("Item Tags")]

    public string? Producttags { get; set; }



    [Column("ProductVideoURL")]
    [StringLength(1000)]
    [DisplayName("Video URL")]
   
    public string? ProductVideoUrl { get; set; }


    [StringLength(500)]

    [DisplayName("Address")]
    public string? Address { get; set; }

    [StringLength(50)]
    [DisplayName("Latitude")]
    public string? Latitude { get; set; }

    [StringLength(50)]
    [DisplayName("Longitude")]
    public string? Longitude { get; set; }



    #region SEO
    [Column("SEO_PageName")]
    [StringLength(200)]
  
    public string? SeoPageName { get; set; }

    [Column("SEO_MetaTItle")]
    [StringLength(200)]
  
    public string? SeoMetaTitle { get; set; }

    [Column("SEO_Keywords")]
    [StringLength(300)]
   
    public string? SeoKeywords { get; set; }

    [Column("SEO_Metadescription")]
    [StringLength(500)]
   
    public string? SeoMetadescription { get; set; }
    #endregion

    #endregion

    #region Decimal
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayName("Old Price")]
  
    public decimal? OldPrice { get; set; }


    [Column(TypeName = "decimal(18, 2)")]
    [DisplayName("Auction Start Price")]
    public decimal? AuctionStartPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [DisplayName("Auction Reserve Price")]
    public decimal? AuctionReservedPrice { get; set; }




    [Column("shipping_weight", TypeName = "decimal(18, 2)")]
    [DisplayName("Shipping Weight")]
   
    public decimal? ShippingWeight { get; set; }

    [Column("shipping_length", TypeName = "decimal(18, 2)")]
    [DisplayName("Shipping Length")]
   
    public decimal? ShippingLength { get; set; }

    [Column("shipping_width", TypeName = "decimal(18, 2)")]
    [DisplayName("Shipping Width")]
   
    public decimal? ShippingWidth { get; set; }


    [Column("shipping_height", TypeName = "decimal(18, 2)")]
    [DisplayName("Shipping Height")]
    
    public decimal? ShippingHeight { get; set; }

    [Column("shipping_AddonCharge", TypeName = "decimal(18, 2)")]
    [DisplayName("Shipping Add On Charge")]
    
    public decimal? ShippingAddonCharge { get; set; }

    #endregion

    #region Int  
    [DisplayName("Seller ID")]
    public int ProfileId { get; set; }


    [DisplayName("Modified ID")]
    public int? ModifiedById { get; set; }


    [Column("mincartqty")]
    [DisplayName("Min Cart Qty")]

    //[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid integer value.")]
    public int? Mincartqty { get; set; }

    [Column("maxcartqty")]
    [DisplayName("Max Cart Qty")]
    //[Range(0, int.MaxValue, ErrorMessage = "Please enter a valid integer value.")]
    public int? Maxcartqty { get; set; }



    [DefaultValue(0)]
    [DisplayName("Star Rating")]
    public int StarRating { get; set; }

    [DefaultValue(0)]
    [DisplayName("Total Reviews")]
    public int TotalReviews { get; set; }



    #endregion

    #region DateTime

    [Column(TypeName = "datetime")]
    [DisplayName("Created Date")]
    public DateTime? InsertDate { get; set; }

    [Column(TypeName = "datetime")]
    [DisplayName("Modified Date")]
    public DateTime? ModifiedDate { get; set; }



    [Column(TypeName = "datetime")]

    [DisplayName("Auction Start Date")]
    public DateTime? AuctionStartDateTime { get; set; }

    [Column(TypeName = "datetime")]

    [DisplayName("Auction End Date")]
    public DateTime? AuctionEndDateTime { get; set; }


    #endregion


    #region Bool


    [DisplayName("Is Publish")]
    [DefaultValue(true)]
    public bool IsPublish { get; set; }


    [DisplayName("Is Out Of Stock")]
    public bool Isoutofstock { get; set; }

    [DisplayName("Is Approved By Admin")]
    public bool IsAdminApproved { get; set; }


    [Column("ismanageinventory")]
    [DisplayName("Is Manage Inventory")]
    public bool Ismanageinventory { get; set; }

    [Column("isfreeshipping")]
    [DisplayName("Is Free Shipping")]
    [DefaultValue(false)]
    public bool Isfreeshipping { get; set; }

    [DisplayName("Is Item Video")]
    public bool IsProductVideo { get; set; }


    [DisplayName("Is Custom Item")]
    [DefaultValue(false)]
    public bool? IsCustomProduct { get; set; }
    #endregion


































































    //public WebsiteSetupProductDetail Website_Setup_Product_Detail { get; set; }

    //public int GeneralSetupId { get; set; }
    //public GeneralSetup ProductType { get; set; }


    //public GeneralSetup SellingType { get; set; }

}

