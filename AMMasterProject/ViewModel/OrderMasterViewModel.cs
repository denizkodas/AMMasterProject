namespace AMMasterProject.ViewModel
{
    public class OrderMasterViewModel
    {
    }

    //public class CouponMetaData
    //{
    //    public int CouponID { get; set; }
    //    public string CouponCode { get; set; }
    //    public decimal CouponActualAmount { get; set; }
    //    public decimal CouponConversionAmount { get; set; }
    //    public string CouponActualCurrency { get; set; }
    //    public string CouponConversionCurrency { get; set; }
    //}

    public class DiscountMetaData
    {
        public int DiscountID { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; }
    }

    public class WalletMetaData
    {
        public int PointsUsed { get; set; }
        public decimal PointsConversionAmount { get; set; }
    }

    //public class OrderStatusMetaData
    //{
    //    public int OrderStatusID { get; set; }
    //    public string OrderStatus { get; set; }
    //    public DateTime OrderStatusDate { get; set; }
    //    public string PaymentStatus { get; set; }  // Paid, Pending
    //}

    public class PaymentMetaData
    {
        public int PaymentID { get; set; }
        public string TransactionID { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class LoyaltyPointsMetaData
    {
        public int PointsEarned { get; set; }
        public int PointsRedeemed { get; set; }
    }

    public class ShippingMetaData
    {
        public int ShippingAddressID { get; set; }
        public string ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
    }

    public class ShippingTypeMetaData
    {
        public string ShippingType { get; set; }
        public string ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public int DeliveryDays { get; set; }

        public string FromCountry { get; set; }

        public string ToCountry { get; set; }

        public string VehicleType { get; set; } //by air, road, etc
    }

    public class TaxMetaData
    {
        public int TaxID { get; set; }
        public decimal TaxAmount { get; set; }
        public string TaxType { get; set; }
    }


    public class OrderItemMetaData
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemCurrency { get; set; }
    }

    public class OrderChargesMetaData
    {
        public int ChargeID { get; set; }
        public decimal ChargeAmount { get; set; }
        public string ChargeType { get; set; }
    }

    public class OrderVariationMetaData
    {
        public int VariationID { get; set; }
        public string VariationName { get; set; }
        public string VariationValue { get; set; }
    }
}
