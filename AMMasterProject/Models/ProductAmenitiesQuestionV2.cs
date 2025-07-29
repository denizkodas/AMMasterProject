using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_AmenitiesQuestion_V2")]
public partial class ProductAmenitiesQuestionV2
{
    [Key]
    public int ProductAmenitiesId { get; set; }

    [StringLength(300)]
    public string? ProductAmenitiesHeading { get; set; }

    public bool IsPublish { get; set; }

    public int? ProfileId { get; set; }

    public int? SortNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }

    [StringLength(20)]
    public string? ControlType { get; set; }
}
