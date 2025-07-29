using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Ads_PageName")]
public partial class AdsPageName
{
    [Key]
    public int AdsPageId { get; set; }

    public int? AdsPlacementId { get; set; }

    [StringLength(50)]
    public string? PageName { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }
}
