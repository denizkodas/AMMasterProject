using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_Brands")]
public partial class ProductBrand
{
    [Key]
    public int ProductBrandId { get; set; }

    [Column("ProductGUID")]
    public Guid? ProductGuid { get; set; }

    public int? BrandId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    [StringLength(256)]
    public string? UserName { get; set; }
}
