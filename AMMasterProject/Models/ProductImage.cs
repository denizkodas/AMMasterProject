using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_Images")]
public partial class ProductImage
{
    [Key]
    public int ProductImagesId { get; set; }

    [Column("ProductGUID")]
    public Guid? ProductGuid { get; set; }

    [StringLength(1000)]
    public string? Image { get; set; }

    [StringLength(500)]
    public string? AltText { get; set; }

    [StringLength(500)]
    public string? Title { get; set; }

    public int? Displayorder { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    [StringLength(256)]
    public string? UserName { get; set; }
}
