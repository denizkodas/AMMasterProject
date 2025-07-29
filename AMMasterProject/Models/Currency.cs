using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Currency")]
public partial class Currency
{
    [Key]
    public int CurrencyId { get; set; }

    [StringLength(50)]
    public string? CurrencyName { get; set; }

    public bool? IsPublished { get; set; }

    [Column(TypeName = "date")]
    public DateTime? InsertDate { get; set; }

    [StringLength(256)]
    public string? UserName { get; set; }
}
