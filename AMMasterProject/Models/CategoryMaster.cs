using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("category_master")]
public partial class CategoryMaster
{
    [Key]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("category_name")]
    [StringLength(200)]
    [DisplayName("Category Name")]
    [RegularExpression("^[A-Za-z0-9 ]*$", ErrorMessage = "Only characters, numbers, and spaces are allowed.")]
    [Required(ErrorMessage = "Category Name Is Required")]
    public string CategoryName { get; set; }

    [Column("URLPath")]
    [StringLength(500)]
    [DisplayName("URL Path")]
   
    
    public string? Urlpath { get; set; }

    [Column("parent_category_id")]
    [DisplayName("Parent Category")]
    [Required(ErrorMessage = "Parent Category Is Required")]
    public int ParentCategoryId { get; set; }

    [Column("Description")]
    [StringLength(500)]
    [DisplayName("Description")]
    public string? Description { get; set; }

    [StringLength(1000)]
    [DisplayName("Icon")]
    
    public string? Icon { get; set; }

    [StringLength(1000)]
    [DisplayName("Banner")]
   
    public string? Banner { get; set; }


    [DisplayName("Is Published")]
    [Required(ErrorMessage = "Is Published Is Required")]
    [DefaultValue(true)]
    public bool IsPublished { get; set; }


    [DisplayName("Is Show Home Page")]
    [Required(ErrorMessage = "Is Show Home Page Is Required")]
    [DefaultValue(false)]
    public bool IsShowHomePage { get; set; }


    [DisplayName("Is Include Menu")]
    [Required(ErrorMessage = "Is Include Menu Is Required")]
    [DefaultValue(false)]
    public bool IsIncludeMenu { get; set; }

    [Column("IsDeleted")]
    
    [DisplayName("IsDeleted")]

    [DefaultValue(false)]
    public bool IsDeleted { get; set; }

    public string? SeoPageName { get; set; }

    [Column("SEOTitle")]
    [StringLength(100)]
    [DisplayName("SEO Title Name")]
   
    public string? SeoTitle { get; set; }

    [Column("SEOKeyword")]
    [StringLength(200)]
    [DisplayName("SEO Keyword")]
    
    public string? SeoKeyword { get; set; }

    [Column("SEODescription")]
    [StringLength(200)]
    [DisplayName("SEO Description")]
   
    public string? SeoDescription { get; set; }

    [Column(TypeName = "datetime")]
    [DisplayName("Insert Date")]
    public DateTime? InsertDate { get; set; }

    [Column(TypeName = "datetime")]
    [DisplayName("Modified Date")]
    public DateTime? ModifiedDate { get; set; }

    [Column("ProfileId")]
    [DisplayName("ProfileId")]
    public int ProfileId { get; set; }

    [Column("Sortnumber")]
    [DisplayName("Sort Order")]
    [Required(ErrorMessage = "Sort Order Is Required")]
    public int Sortnumber { get; set; }



    public int SellingTypeID { get; set; }   ///sell, classified, auction,
    public int ListingTypeID { get; set; }//physical, digital, course etc

    //[Column("CategoryTypeId")]
    //[DisplayName("Listing Type")]
    //[Required(ErrorMessage = "Listing Type Is Required")]
    //public int? CategoryTypeId { get; set; }

    //[Column("Sellingtypeid")]
    //[DisplayName("Selling Type")]
    //[Required(ErrorMessage = "Selling Type Is Required")]
    //public int? Sellingtypeid { get; set; }




}
