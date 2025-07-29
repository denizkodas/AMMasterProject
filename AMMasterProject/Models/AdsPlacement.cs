using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Ads_Placement")]
public partial class AdsPlacement
{
    [Key]
    public int AdsPlacementId { get; set; }

    [StringLength(50)]
    public string? PlacementName { get; set; }

    [StringLength(50)]
    public string? Description { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }
}
