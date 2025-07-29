using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_Boost")]
public partial class ProductBoost
{
    [Key]
    public int ProductBoostId { get; set; }

    //[Column("ProductGUID")]
    //public Guid? ProductGuid { get; set; }

    [Column("ItemID")]
    
    public int ItemID { get; set; }


    [Column("ItemBoostGUID")]
    public Guid ItemBoostGUID { get; set; }

    [StringLength(50)]
  
    public string BoostType { get; set; }


    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [StringLength(200)]

    public string InvoiceNumber { get; set; }



    public string? BoostMetaData { get; set; }

}
