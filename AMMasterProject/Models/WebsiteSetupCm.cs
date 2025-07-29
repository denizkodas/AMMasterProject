using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Website_Setup_CMS")]
public partial class WebsiteSetupCm
{
    [Key]
    [Column("CMSId")]

    public int Cmsid { get; set; }

    [Column("CMSKey")]
    [StringLength(50)]
    public string? Cmskey { get; set; }

    [Column("CMSContent")]
    [DisplayName("Content")]
    [Required(ErrorMessage = "Content Is Required")]
    public string Cmscontent { get; set; }

    [Column("IsActive")]
    [DefaultValue(true)]
    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    [DisplayName("Insert Date")]
    public DateTime? InsertDate { get; set; }
}
