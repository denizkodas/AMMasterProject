using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_Coupon")]
public partial class ProductCoupon
{
    [Key]
    public int ProductCouponId { get; set; }

    [Column("ProductCouponGUID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProductCouponGuid { get; set; }


    [Column("CouponTypeId")]
    [DisplayName("Coupon Applied On")]
    [Required(ErrorMessage = "Coupon Applied On Is Required")]
    public int CouponTypeId { get; set; }    ///Vendor, Seller, Category,4. All, coming from general setup

    [StringLength(100)]
    [Column("CouponName")]
    [DisplayName("Coupon Name On")]
    [Required(ErrorMessage = "Coupon Name Is Required")]
    public string CouponName { get; set; }

    [StringLength(200)]
    [Column("CouponDescription")]
    [DisplayName("Coupon Description")]
  
    public string? CouponDescription { get; set; }



    [Column(TypeName = "datetime")]
    [DisplayName("Start Date")]
    //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd,yyyy}")]
    public DateTime? StartDate { get; set; }


    [Column(TypeName = "datetime")]
   
    [DisplayName("End Date")]
    //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd,yyyy}")]
    public DateTime? EndDate { get; set; }


    [Column("NoofCoupon")]
    [DisplayName("No. of Coupon On")]
    [Required(ErrorMessage = "No. of Coupon Is Required")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numbers are allowed.")]
    public int NoofCoupon { get; set; }


    [Column("PerPersonUsed")]
    [DisplayName("Per Person Used")]
    [Required(ErrorMessage = "Per Person Used Is Required")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numbers are allowed.")]
    public int PerPersonUsed { get; set; }


    [Column("DiscountType")]
    [DisplayName("Discount Type")]
    [Required(ErrorMessage = "Discount Type Is Required")]
    public string DiscountType { get; set; }  //percentage amount



    [Column(TypeName = "decimal(18, 2)")]
    [DisplayName("Discount")]
    [Required(ErrorMessage = "Discount Is Required")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number with up to 2 decimal places")]
    public decimal Discount { get; set; }


    [Column(TypeName = "decimal(18, 2)")]
    [DisplayName("Seller Percentage")]
    [Required(ErrorMessage = "Seller Percentage Required")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number with up to 2 decimal places")]
    public decimal SellerPercentage { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    [DisplayName("Admin Percentage")]
    [Required(ErrorMessage = "Seller Percentage Required")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number with up to 2 decimal places")]
    public decimal AdminPercentage { get; set; }

    public int ProfileId { get; set; }

    [Column(TypeName = "datetime")]
    [DisplayName("Created Date")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd,yyyy}")]
    public DateTime? InsertDate { get; set; }


    [DisplayName("Is Publish")]
    [DefaultValue(true)]
    public bool IsPublish { get; set; }
}
