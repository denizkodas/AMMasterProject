using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Setup_TaxClass")]
public partial class SetupTaxClass
{
    [Key]
    [Column("TaxClassID")]
    public int TaxClassId { get; set; }

    [Column("TaxClassGUID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? TaxClassGuid { get; set; }

    [StringLength(50)]
    [DisplayName("Tax Class")]
    [Required(ErrorMessage = "Tax Class Is Required")]
    public string TaxClass { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }


    [DisplayName("Seller ID")]
    public int ProfileId { get; set; }

    [Column(TypeName = "datetime")]
   
    
    public DateTime? InsertDate { get; set; }

    public bool IsPublished { get; set; }

    public bool IsAdminApproved { get; set; }
}
