using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_Tax_V2")]
public partial class ProductTaxV2
{
    [Key]
    public int ProductTaxId { get; set; }

    [Column("ProductGUID")]
    public Guid? ProductGuid { get; set; }

    public int? TaxClassId { get; set; }

    [StringLength(20)]
    public string? Type { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Value { get; set; }

    public bool? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }
}
