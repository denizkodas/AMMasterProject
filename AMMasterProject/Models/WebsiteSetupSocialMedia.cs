using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

public partial class WebsiteSetupSocialMedia
{
    [Key]
    [Column("SocialMediaId")]
    
    public int SocialMediaId { get; set; }


    [StringLength(100)]
    [Column("Name")]
    [DisplayName("Name")]
    [Required(ErrorMessage = "Name Is Required")]
    public string Name { get; set; }


    [StringLength(1000)]
    [Column("Icon")]
    [DisplayName("Icon")]
    [Required(ErrorMessage = "Icon Is Required")]
    public string Icon { get; set; }

    [Column("URL")]
    [StringLength(200)]
    [DisplayName("URL")]
    [Required(ErrorMessage = "URL Is Required")]
    [Url(ErrorMessage = "Invalid URL")]
    public string Url { get; set; }

    [Column("IsPublish")]
    [DefaultValue(true)]
    public bool IsPublish { get; set; }
}
