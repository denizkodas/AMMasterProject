using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_Amenities_Options_V2")]
public partial class ProductAmenitiesOptionsV2
{
    [Key]
    [Column("ProductAmenitiesOptionID")]
    public int ProductAmenitiesOptionId { get; set; }

    public int? ProductAmenitiesId { get; set; }

    [StringLength(1000)]
    public string? ProductAmenitiesIcon { get; set; }

    [StringLength(300)]
    public string? ProductAmenitiesName { get; set; }

    public bool? IsPublish { get; set; }
}
