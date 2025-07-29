using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("PageName")]
public partial class PageName
{
    [Key]
    [Column("PageNameId")]
    public int PageNameId { get; set; }

    [Column("PageCategoryId")]
    [DisplayName("Page Category")]
    //[Required(ErrorMessage = "Page Category Is Required")]
    public int PageCategoryId { get; set; }

    [StringLength(50)]
    [Column("Type")]
    [DisplayName("Type")]
    [Required(ErrorMessage = "Type Is Required")]
    public string Type { get; set; }

    [Column("PageName")]
    [StringLength(50)]
    [DisplayName("Page Name")]
    [Required(ErrorMessage = "Page Name Is Required")]
    public string Name { get; set; }


    [Column("IsPublish")]
    [DefaultValue(true)]
    public bool IsPublish { get; set; }


    [Column("IsAdminDefine")]
    [DefaultValue(false)]
    public bool IsAdminDefine { get; set; }


    [StringLength(50)]
    [Column("PageType")]
    [DisplayName("PageType")]
    //[Required(ErrorMessage = "Page Type Is Required")]
    public string? PageType { get; set; }  //CMS or Page


    [Column("IsURL")]
    [DefaultValue(false)]
    public bool IsUrl { get; set; }

    [Column("URL")]
    [StringLength(200)]
    //[Url(ErrorMessage = "Invalid URL")]
    public string? Url { get; set; }

    [Column("InsertDate")]
    [DisplayName("Insert Date")]
    public DateTime? InsertDate { get; set; }


    [Column("Sortnumber")]
    [DisplayName("Sort Order")]
    [Required(ErrorMessage = "Sort Order Is Required")]
    public int Sortnumber { get; set; }

    [Column("ProfileId")]
    [DisplayName("ProfileId")]
   
    public int ProfileId { get; set; }

    [Column("SEOPageName")]
    [StringLength(100)]
    [DisplayName("SEO Page Name")]
    [Required(ErrorMessage = "SEO Page Name Is Required")]
    public string SeoPageName { get; set; }

    [Column("SEOTitle")]
    [StringLength(100)]
    [DisplayName("SEO Title Name")]
    [Required(ErrorMessage = "SEO Title Is Required")]
    public string SeoTitle { get; set; }

    [Column("SEOKeyword")]
    [StringLength(200)]
    [DisplayName("SEO Keyword")]
    [Required(ErrorMessage = "SEO Keyword Is Required")]
    public string SeoKeyword { get; set; }

    [Column("SEODescription")]
    [StringLength(200)]
    [DisplayName("SEO Description")]
    [Required(ErrorMessage = "SEO Description Is Required")]
    public string SeoDescription { get; set; }


    [StringLength(100)]
    public string? CMSKey { get; set; }


    [Column("PageDescription")]
    [DisplayName("Page Description")]
    //[Required(ErrorMessage = "Page Description Is Required")]
    public string? PageDescription { get; set; }


    [Column("PageHTML")]
    [DisplayName("Page HTML")]
    //[Required(ErrorMessage = "Page HTML Is Required")]
    public string? PageHTML { get; set; }

    [Column("PageCSS")]
    [DisplayName("Page CSS")]
    //[Required(ErrorMessage = "Page CSS Is Required")]
    public string? PageCSS { get; set; }

    [Column("PageJson")]
    [DisplayName("Page Json")]
    //[Required(ErrorMessage = "Page Json Is Required")]
    public string? PageJson { get; set; }
}
