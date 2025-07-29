using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMMasterProject.Models
{
    public class OrderDetail
    {
        [Key]
        [Column("OrderDetailID")]
        public int OrderDetailID { get; set; }

        public int OrderSummaryID { get; set; }  // Updated to match the OrderSummary model

        public int ItemID { get; set; }

        public ItemMetaData ItemMetaData { get; set; }

        public OrderChargesMetaData ChargesMetaData { get; set; } // if its each item related
        public OrderVariationMetaData VariationMetaData { get; set; }

        [Column("ItemType")]
        [StringLength(100)]
        public string Status { get; set; }

        public int Quantity { get; set; }

        public decimal ActualAmount { get; set; }  // Amount in the item's original currency
        public decimal ConversionAmount { get; set; }  // Amount in the user's selected currency

        public string ActualCurrency { get; set; }  // Currency in which the item is priced
        public string ConversionCurrency { get; set; }  // Currency selected by the user

        public decimal SubTotalActualAmount { get; set; }  // Quantity * (ActualAmount + VariationPrice)
        public decimal SubTotalConversionAmount { get; set; }  // Quantity * (ConversionAmount + VariationPriceConverted)

        public decimal? ShippingCostOnEachItem { get; set; }  //this property is only used if shipping cost is on each item

        public ShippingTypeMetaData ShippingTypeMetaData { get; set; } //this is in future if each item to give the express, etc delivery
    }

}
