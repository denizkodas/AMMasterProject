using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Product_Coupon_Child")]
public partial class ProductCouponChild
{
    [Key]
    public int ProductCouponChildId { get; set; }

    [Column("ProductCouponChildGUID")]
    public Guid? ProductCouponChildGuid { get; set; }



    [Column("ProductCouponId")]
 
    public int ProductCouponId { get; set; }


    [Column("ReferenceId")]
    public int ReferenceId { get; set; }  ///productid, categoryid , sellerid


  
    [Column("ReferenceTypeID")]
    public int ReferenceTypeID { get; set; }  //8= product, 9= category, 10=seller 

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }
}
