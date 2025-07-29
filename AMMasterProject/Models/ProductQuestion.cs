using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_Question")]
public partial class ProductQuestion
{
    [Key]
    [Column("ProductQAId")]
    public int ProductQaid { get; set; }

    public int? ProductId { get; set; }

    [StringLength(1000)]
    public string? Question { get; set; }

    public int? ProfileId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    public bool? IsActive { get; set; }

    
}
