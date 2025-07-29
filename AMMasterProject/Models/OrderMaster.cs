using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Threading.Channels;
using PayPal.Api;

namespace AMMasterProject
{
    public class OrderMaster
    {

        #region Int

      
        [Key]
        [Column("OrderId")]
        public int OrderId { get; set; }


        [Required(ErrorMessage = "BuyerId")]
        public int BuyerId { get; set; }

        [Required(ErrorMessage = "SellerId")]
        public int SellerId { get; set; }

        #endregion

        #region String
        [Column("ItemType")]
        [StringLength(20)]
      
        public string ItemType { get; set; } // credit,subscription, item, learning, service


        [Column("TransactionType")]
        [StringLength(20)]

        public string TransactionType { get; set; } // free,purchased, used


        #region Status
        [Column("OrderStatus")]
        [StringLength(100)]
       
        public string OrderStatus { get; set; }  //cart - confirm - cancel this is for user Cart to confirm only payment is 

        



        [Column("OrderProcessStatus")]
        [StringLength(100)]
      
        public string? OrderProcessStatus { get; set; }  // processing - completed - cancelled  -- it should have only 3 status  this is for admin


        [Column("PaymentStatus")]
        [StringLength(100)]

        public string? PaymentStatus { get; set; }  // Pending - Paid - Refund  -- 


        #endregion


        #region Notes

        [Column("VendorNotes")]
        [StringLength(1000)]
        

        public string? VendorNotes { get; set; }

        [Column("AdminNotes")]
        [StringLength(1000)]
      

        public string? AdminNotes { get; set; }


        [Column("InvoiceNumber")]
        [StringLength(200)]
       
        public string? InvoiceNumber { get; set; }

        #endregion




        [Column("OrderDate")]
       
        public DateTime OrderDate { get; set; }

        [Column("UpdateDate")]
       
        public DateTime? UpdateDate { get; set; }


        #endregion


        #region MetaData

        [Column("ItemMetaData")]

        public string? ItemMetaData { get; set; }

        [Column("VariationMetaData")]

        public string? VariationMetaData { get; set; }

        [Column("OrderChargesMetaData")]

        public string? OrderChargesMetaData { get; set; }

        [Column("OtherMetaData")]

        public string? OtherMetaData { get; set; }


        [Column("ShippingMetaData")]

        public string? ShippingMetaData { get; set; }


        [Column("OrderStatusMetaData")]

        public string? OrderStatusMetaData { get; set; }


        [Column("PaymentMetaData")]

        public string? PaymentMetaData { get; set; }


        [Column("ReviewMetaData")]

        public string? ReviewMetaData { get; set; }


        [Column("SummaryMetaData")]

        public string? SummaryMetaData { get; set; }



        [Column("LoyaltyPointsMetaData")]

        public string? LoyaltyPointsMetaData { get; set; }
        #endregion


        #region Bool


        [Column("IsDeleted")]
        [DisplayName("IsDeleted")]

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }


        [DefaultValue(false)]
        public bool IsExpiry { get; set; }

        public DateTime? ExpiryDate { get; set; }


        #endregion








    }
}