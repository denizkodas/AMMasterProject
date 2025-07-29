using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Ads_Detail")]
public partial class AdsDetail
{
    [Key]
    public int AdId { get; set; }

    public int AdsPlacementId { get; set; }  // 600 by 600, etc.

    public int AdsPageId { get; set; } // search, default, etc.

    [StringLength(1000)]
    [DisplayName("Image")]
    [Required(ErrorMessage = "Image Is Required")]
    public string Image { get; set; }

    [StringLength(500)]
    [DisplayName("Description")]
    public string? Description { get; set; }

   
    

    [StringLength(50)]
    [DisplayName("Status")]
    [Required(ErrorMessage = "Status Is Required")]
    public string Status { get; set; } // approved, reject, pending, review

    [StringLength(50)]
    [DisplayName("Remarks")]
    public string? Remarks { get; set; } // any comment by admin

    public int ProfileId { get; set; }

    [DefaultValue (false)]
    public bool IsUrl { get; set; }

    //[Column("URL")]
    //[StringLength(500)]
    //[Required(ErrorMessage = "Url Is Required")]
    //public string Url { get; set; }



}
