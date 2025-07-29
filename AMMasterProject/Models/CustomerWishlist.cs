using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Customer_Wishlist")]
public partial class CustomerWishlist
{
    [Key]
    public int WishlistId { get; set; }

    public int? ProductId { get; set; }

    [StringLength(256)]
    public int? UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    [Column("IP")]
    [StringLength(50)]
    public string? Ip { get; set; }

    [Column("cwgroupname")]
    [StringLength(100)]
    public string? Cwgroupname { get; set; }
}
