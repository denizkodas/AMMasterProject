using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_RelatedProducts")]
public partial class ProductRelatedProduct
{
    [Key]
    public int ProductRelatedId { get; set; }

    [Column("ProductGUID")]
    public Guid? ProductGuid { get; set; }

    [Column("RelatedProductID")]
    public int? RelatedProductId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    [Column("type")]
    [StringLength(50)]
    public string? Type { get; set; }
}
