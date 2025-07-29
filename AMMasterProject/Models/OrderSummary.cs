using AMMasterProject.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMMasterProject.Models
{
    public class OrderSummary
    {
        [Key]
        [Column("OrderSummaryID")]
        public int OrderSummaryID { get; set; }

        public string InvoiceNumber { get; set; }

        [Column("ItemType")]
        [StringLength(20)]
        public string OrderType { get; set; }  // Item, Credit, Subscription

        public decimal TotalActualAmount { get; set; }  // Total item prices in the original currencies of the items
        public decimal TotalConversionAmount { get; set; }  // Total item prices in the user's selected currency

        public decimal TotalVariationActualAmount { get; set; }  // Total variation prices in the original currencies
        public decimal TotalVariationConversionAmount { get; set; }  // Total variation prices in the user's selected currency

        public decimal TotalChargesActualAmount { get; set; }  // Total charges prices in the original currencies
        public decimal TotalChargesConversionAmount { get; set; }  // Total charges prices in the user's selected currency

        public string ActualCurrency { get; set; }  // This may be multiple currencies if items are from different sellers
        public string ConversionCurrency { get; set; }  // T
        public CouponMetaData CouponMetaData { get; set; }

        // Discount MetaData
        public DiscountMetaData DiscountMetaData { get; set; }

        // Wallet MetaData (only if it's used)
        public WalletMetaData WalletMetaData { get; set; }

        public DateTime OrderDate { get; set; }

        // Order Status MetaData
        public OrderStatusMetaData OrderStatusMetaData { get; set; }

        [Column("PaymentMode")]
        [StringLength(100)]
        public string PaymentMode { get; set; }  // Cash, Paypal, Stripe, etc.

        public PaymentMetaData PaymentMetaData { get; set; }

        public int BuyerID { get; set; }
        public int SellerID { get; set; }

        [Column("OrderIsCart")]
        [StringLength(100)]
        public string OrderIsCart { get; set; }  // cart, confirm

        [Column("OrderIsCompleted")]
        [StringLength(100)]
        public string OrderIsCompleted { get; set; }  // once order detail all item not pending a single order so its completed

        public DateTime UpdatedDate { get; set; }

        [Column("VendorNotes")]
        [StringLength(1000)]
        public string VendorNotes { get; set; }

        [Column("AdminNotes")]
        [StringLength(1000)]
        public string AdminNotes { get; set; }

        // Loyalty Points MetaData (only if it's used)
        public LoyaltyPointsMetaData LoyaltyPointsMetaData { get; set; }

        public decimal? ShippingCostOneTime { get; set; }  //this property is only used if shipping cost is only one for all cart item
        public decimal? ShippingCostOnEachItem { get; set; }  //this property is only used if shipping cost is on each item

        // Shipping MetaData
        public ShippingMetaData ShippingMetaData { get; set; } //shippping address

        // Tax MetaData
        public TaxMetaData TaxMetaData { get; set; } //only applied if one time platform fees or any other tax
    }

}
