using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Blog_Setup")]
public partial class BlogSetup
{
    [Key]
    [Column("BlogSetup_id")]
    public int BlogSetupId { get; set; }


    [Column("BlogCategoryId")]
    public int BlogCategoryId { get; set; }

    [Column("URLPath")]
    [StringLength(1000)]
    public string? Urlpath { get; set; }

    [Column("parent_category_id")]
    public int? ParentCategoryId { get; set; }

    public string? Description { get; set; }

    [StringLength(1000)]
    public string? Icon { get; set; }

    [StringLength(1000)]
    public string? Banner { get; set; }

    public bool? IsPublished { get; set; }

    public bool? IsShowHomePage { get; set; }

    public bool? IsIncludeMenu { get; set; }

    [Column("SEO_PageName")]
    [StringLength(100)]
    public string? SeoPageName { get; set; }

    [Column("SEO_Title")]
    [StringLength(100)]
    public string? SeoTitle { get; set; }

    [Column("SEO_Keyword")]
    [StringLength(1000)]
    public string? SeoKeyword { get; set; }

    [Column("SEO_Description")]
    [StringLength(50)]
    public string? SeoDescription { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [StringLength(256)]
    public string? UserName { get; set; }

    public int? DisplayOrder { get; set; }

    [Column("categoryvalue")]
    [StringLength(50)]
    public string? Categoryvalue { get; set; }

    [Column("keyid")]
    public int? Keyid { get; set; }
}
