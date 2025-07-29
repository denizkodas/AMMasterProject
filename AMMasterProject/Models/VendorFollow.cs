using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Vendor_Follow")]
public partial class VendorFollow
{
    [Key]
    public int FollowerId { get; set; }

    public int? ProfileId { get; set; }

    public int? VendorId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }
}
