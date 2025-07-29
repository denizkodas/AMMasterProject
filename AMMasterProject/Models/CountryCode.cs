using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("CountryCode")]
public partial class CountryCode
{
    [Key]
    [Column("CountryID")]
    public int CountryID { get; set; }

    [Column("Name")]
    [StringLength(250)]
 
    public string Name { get; set; }

    

    [Column("MobileCode")]
    [StringLength(10)]
    public string MobileCode { get; set; }


    [Column("CountryCode2Digit")]
    [StringLength(10)]
    public string? CountryCode2Digit { get; set; }

    [Column("CountryCode3Digit")]
    [StringLength(10)]
    public string? CountryCode3Digit { get; set; }


    [Column("CurrencyName")]
    [StringLength(30)]
    public string? CurrencyName { get; set; }

    [Column("CurrencyCode")]
    [StringLength(20)]
    public string? CurrencyCode { get; set; }

    [Column("CurrencySymbol")]
    [StringLength(20)]
    public string? CurrencySymbol { get; set; }

    [Column("FlagPath")]
    [StringLength(200)]
    public string Flagpath { get; set; }

    [Column("IsPublish")]
    [DefaultValue(false)]
    public bool IsPublish { get; set; }


    [Column("IsCurrencyPublish")]
    [DefaultValue(false)]
    public bool IsCurrencyPublish { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ConversionRate { get; set; }


    public DateTime? ConversionUpdatedDate { get; set; }

}
