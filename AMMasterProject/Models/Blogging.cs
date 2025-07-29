using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Bloging")]
public partial class Blogging
{
    [Key]
    public int BlogId { get; set; }

    [StringLength(100)]
    [DisplayName("Title")]
   
    [Required(ErrorMessage = "Title Is Required")]
    public string Title { get; set; }

    [Column("categoryid")]
    [DisplayName("Category")]
    [Required(ErrorMessage = "Category Is Required")]
    public int Categoryid { get; set; }


  

  

    [Column("Image")]
    [StringLength(1000)]
    [DisplayName("Image")]
    [Required(ErrorMessage = "Image Is Required")]
    public string Image { get; set; }

    [Column("Summary")]
    [StringLength(1000)]
    [DisplayName("Summary")]
    [Required(ErrorMessage = "Summary Is Required")]
    public string Summary { get; set; }


    [Column("Description")]
   
    [DisplayName("Description")]
    [Required(ErrorMessage = "Description Is Required")]

    public string Description { get; set; }

    [Column("Fileattached")]
    [StringLength(1000)]
    [DisplayName("File attached")]
   
    public string? Fileattached { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

  
    public int ProfileId { get; set; }

    public bool IsPublish { get; set; }

    

    



    [Column("isaddonhomepage")]
    [DefaultValue(false)]   
    public bool Isaddonhomepage { get; set; }

    [Column("isfeatured")]
    [DefaultValue(false)]
    public bool isfeatured { get; set; }

    [Column("externalurl")]
    [StringLength(200)]
    [DisplayName("External url")]
    [Url(ErrorMessage = "Invalid URL")]
    public string? Externalurl { get; set; }


    [Column("SEOPageName")]
    [StringLength(100)]
    [DisplayName("SEO Page Name")]
    [Required(ErrorMessage = "SEO Page Name Is Required")]
    public string SeoPageName { get; set; }

    [Column("SEOTitle")]
    [StringLength(200)]
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


}
