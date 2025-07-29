using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Websitesetup_Partner")]
public partial class WebsitesetupPartner
{
    [Key]
    [Column("PartnerID")]
    public int PartnerId { get; set; }

    [Column("PartnerGUID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid PartnerGuid { get; set; }



 
    [Column("Image")]
    [StringLength(1000)]
    [DisplayName("Image")]
    [Required(ErrorMessage = "Image Is Required")]
    public string Image { get; set; }


    [Column("ParnertName")]
    [StringLength(200)]
    [DisplayName("Parner Name")]
    [Required(ErrorMessage = "Parner Name Is Required")]
    public string ParnerName { get; set; }

    [Column(TypeName = "date")]
    public DateTime? Insertdate { get; set; }

    public bool IsPublish { get; set; }

    [Column("ProfileID")]
    public int ProfileId { get; set; }

    [Column("sortorder")]
    [DisplayName("Sort Order")]
    [Required(ErrorMessage = "Sort Order Is Required")]
    public int Sortorder { get; set; }

    [Column("partnerurl")]
    [StringLength(300)]
    [Url(ErrorMessage = "Invalid URL")]
    public string? Partnerurl { get; set; }

    [Column("showonhomepage")]
   
    public bool Isaddonhomepage { get; set; }
}
