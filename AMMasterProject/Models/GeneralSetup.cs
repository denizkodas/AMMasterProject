using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("General_Setup")]
public partial class GeneralSetup
{
    [Key]
    public int GeneralSetupId { get; set; }

    [StringLength(50)]
    public string? GeneralSetupType { get; set; }

    [StringLength(50)]
    public string? GeneralSetupName { get; set; }

    [StringLength(1000)]
    public string? GeneralSetupImage { get; set; }

    [StringLength(1000)]
    public string? GeneralSetupIcon { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Insertdate { get; set; }


    public virtual ICollection<ProductBasicV2> ProductBasicV2s { get; set; }
}
