using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace AMMasterProject.ViewModel
{

    #region OrderViewModel
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public int BuyerID { get; set; }

        public int SellerID { get; set; }

        public string ItemType { get; set; }
        public string OrderStatus { get; set; }

        public string OrderProcessStatus { get; set; }


        public DateTime  OrderDateDT { get; set; }
        public string OrderDate { get; set; }

        public string UpdateDate { get; set; }

        public string VendorNotes { get; set; }

        public string AdminNotes { get; set; }

        public string InvoiceNumber { get; set; }

        public string PaymentStatus { get; set; }

        public string TransactionType { get; set; }

        public string IncomingOutgoing { get; set; }

        //public ItemDetailMetaData ItemDetailMetaData { get; set; }

        public ItemMetaData ItemDetailMetaData { get; set; }

        public VariationDetailMetaData VariationMetaData { get; set; }

        public ChargesDetailMetaData ChargesDetailMetaData { get; set; }

        public PaymentMetaDataViewModel PaymentMetaData { get; set; }

        public List<OrderStatusMetaData> OrderStatusMetaData { get; set; }

        //public SummaryOrderMetaData SummaryOrderMetaData { get; set; }

        public SummaryModel SummaryOrderMetaData { get; set; }

        public ShippingDetailMetaData ShippingDetailMetaData { get; set; }
        public SellerViewModel SellerViewModel { get; set; }
        public ClientViewModel BuyerViewModel { get; set; }

        public List<ReviewMetaDataViewModel> ReviewMetaData { get; set; }

       public List<LoyaltyPointsViewModel> LoyaltyPointMetaData { get; set; }

    }
    #endregion



    #region PaymentMetaViewModel

    public class PaymentMetaDataViewModel:OrderPaymentModel
    {


        public string PaymentMethod { get; set; }


        public string PaymentStatus { get; set; }

        public string PaymentReference { get; set; }

        public string PaymentReferenceFile { get; set; }


        public string PayerID { get; set; }

        //public decimal ActualAmount { get; set; }

        //public string ActualCurrency { get; set; }


        //public decimal ConversionAmount { get; set; }

        //public string ConversionCurrency { get; set; }


        public decimal PaidAmount { get; set; }

        public string PaidCurrency { get; set; }

        public decimal WalletDeduction { get; set; }


        public DateTime? PaymentDate { get; set; }

        public string InvoiceNumber { get; set; }

        public string PaymentStructure { get; set; } ///full, partial, installment

        public string PayerEmailID { get; set; }

    }

    #endregion


    #region CartModel


    //list of item
    //public class ItemDetailMetaData: OrderPaymentModel
    //{
    //    public int OrderId { get; set; }

    //    public int SellerID { get; set; }
    //    public int BuyerID { get; set; }
    //    public int ItemID { get; set; }
    //    public string Name { get; set; }


    //    public string sellingtype { get; set; }
    //    public string listingtype { get; set; }
    //    public string ItemImage { get; set; }

    //    public int Quantity { get; set; }

    //    public string Instruction { get; set; }



    //    public string InvoiceNumber { get; set; }


    //    public string CouponID { get; set; }
    //    public string CouponName { get; set; }

    //    public decimal ActualCouponAmount { get; set; }


    //    public decimal CouponAmount { get; set; }


   
    //    public List<VariationDetailMetaData> variationDetailMetaData { get; set; }


    //    public ShippingFeesMetaData ShippingFeesMetaData { get;set; }

    //    public List<ProductDigitalMetaData> productDigitalMetadata { get; set; }

       

    //}


    public class VariationDetailMetaData
    {
       
        public string VariationName { get; set; }

        public decimal ActualAmount { get; set; }
        public decimal ConversionVariationAmount { get; set; }


    }

    public class ShippingFeesMetaData
    {
        public decimal ShippingActualAmount { get; set; }
        public decimal ShippingConversionAmount { get; set; }
    }







    public class ChargesDetailMetaData
    {
        public int OrderID { get; set; }
        public string ChargesName { get; set; }

        public decimal ActualChargesAmount { get; set; }

        public string ActualChargesCurrency { get; set; }


        public decimal ConversionChargesAmount { get; set; }

        public string ConversionChargesCurrency { get; set; }



        public string InvoiceNumber { get; set; }

    }

    //public class CouponDetailViewModel
    //{
    //    public string CouponID { get; set; }
    //    public string CouponName { get; set; }

    //    public decimal ActualCouponAmount { get; set; }


    //    public decimal CouponAmount { get; set; }




    //    public string InvoiceNumber { get; set; }

    //}
    public class ShippingDetailMetaData
    {
        public string FullName { get; set; }

        public string AddressType { get; set; }

        public string ShippingAddress { get; set; }
        public string ShippingEmail { get; set; }
        public string ShippingPhone { get; set; }

        public string ShippingLatitude { get; set; }
        public string ShippingLongitude { get; set; }

        public string NearestLandMark { get; set; }
    }


    public class OrderStatusMetaData
    {
        public int OrderStatusID { get; set; }

        public string OrderStatusName { get; set; }

        public DateTime OrderStatusDate { get; set; }

        public string OrderStatusNotes { get; set; }
        public string InvoiceNumber { get; set; }


    }
    public class SummaryOrderMetaData
    {

        public string Currency { get; set; } ///
        public decimal ItemTotal { get; set; }  /// A. Item Price * Qty
        public decimal ItemQtyTotal { get; set; }


        public decimal VariationTotal { get; set; }//B. variation price  * qty
        public decimal VariationQtyTotal { get; set; }//B. variation price  * qty

        public decimal ChargesTotal { get; set; }   ///C. charges * qty

        public decimal ChargesQtyTotal { get; set; }   ///C. charges * qty

        public decimal ShippingCost { get; set; }   ///D. shipping * qty

        public decimal ShippingQtyCost { get; set; }   ///D. shipping * qty

        public decimal Wallet { get; set; }//deduction
        public decimal CouponTotal { get; set; } //deduction  ///E. A+B -C
        public decimal Total { get; set; }   //A+B+C+D-E

        public decimal TotalQty { get; set; }   //A+B+C+D-E
    }


    public class OrderOtherMetaData
    {

        
       public WalletUsedItemMetaData WalletUsedMetaData { get; set; }

        public CouponMetaData coupon { get; set; }
    }
    #endregion



    #region OrderStatus-Setup-ViewModel
    public class OrderStatusSetupViewModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string StatusType { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }

    #endregion



    #region ItemReview

    public class ReviewMetaDataViewModel
    {
        public string ReviewerType { get; set; }

        public int ReviewerID { get; set; }
        public string ReviewName { get; set; }

        public string ReviewerImage { get; set; }
        public decimal StarRating { get; set; }

        public decimal AverageRating { get; set; }  
        public string Description { get; set; }

        public string Attachment { get; set; }

       public DateTime ReviewDate { get; set; }

    }



    #endregion


    #region Advertise-ViewModel
    public class AdvertiseViewModel
    {
     
        public Guid ItemID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public string Description { get; set; }


        public string SeoURL { get; set; }
    }
    #endregion


    #region ItemBoost-ItemMetaDataForOrder
    public class ItemBoostMetaDataViewModel
    {
        public string itemtype { get; set; }
        public Guid ItemBoostGUID { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public string SEOURL { get; set; }


        public string PaymentType { get; set; }
        public int NoOfCredit { get; set; }

        public bool IsCustomizedPlan { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
      
        public decimal ActualAmount { get; set; }

        public string ActualCurrency { get; set; }


        public decimal ConversionAmount { get; set; }

        public string ConversionCurrency { get; set; }
     
        public string InvoiceNumber { get; set; }


    }
    #endregion



    #region AdvertiseBoost-ViewModel
    public class AdvertiseBoostViewModel
    {
      
        public int BuyerId { get; set; }
        public ItemMetaData ItemMetaData { get; set; }

       
        
        public PaymentMetaDataViewModel PaymentMetaData { get; set; }


        //[DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }

        public bool IsExpiry { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        //[DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public string ExiryDate { get; set; }

        public string InvoiceNumber { get; set; }

        public string BoostType { get; set; }


        public BoostOtherMetaData BoostMetaData { get; set; }
    }
    #endregion



    #region ORDER METADATA V2
    public class ItemMetaData
    {
        public ItemBasicModel basicModel { get; set; }
        public ItemPaymentModel paymentModel { get; set; }

        public List<ItemVariationModel> variationModel { get; set; }

        public List<ItemChargesModel> chargesModel { get; set; }
        public List<ItemSalesChargesModel> saleschargesModel { get; set; }

        public List<ProductDigitalMetaData > productDigitalMetaData { get; set; }

        public ItemCreditModel creditModel { get; set; }

        public ItemSubscriptionModel subscriptionModel { get; set; }

        //for wallet the existing model can be used, just type and transaction type will be wallet

      
    }

    public class ItemBasicModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string ItemImage { get; set; }

        public int Quantity { get; set; }
        public string Description { get; set; }

        public string ItemURL { get; set;}


        public string SellingType { get; set; }///sell, classified, auction, penny auction
        public string ListingType { get; set; }///digital, physical, service, course, credit, subscription

        public string InvoiceNumber { get; set; }


    }

    public class ItemPaymentModel
    {
        public decimal ConversionRate { get; set; }
        public decimal ConversionAmount { get; set; }

        public string ConversionCurrency { get; set; }


        public decimal ActualAmount { get; set; }

        public string ActualCurrency { get; set; }
    }

    public class ItemCreditModel
    {
        public int NoOfCredit { get; set; }

        public bool IsExpiry { get; set; }
        public int? NoOfExpiryDays { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }

    public class ItemSubscriptionModel
    {
        public int RecurringPeriodInDays { get; set; }
        public DateTime ExpiryDate { get; set; }

    }

    public class ItemVariationModel:ItemPaymentModel
    {

        public string VariationName { get; set; }

      


    }

    public class ItemChargesModel : ItemPaymentModel
    {
      
        public string ChargesName { get; set; }

    
    }

    public class ItemSalesChargesModel : ItemPaymentModel
    {

        public string SalesChargeName { get; set; }


    }

    public class SummaryModel
    {
        public string BaseCurrency { get; set; }
        public string Currency { get; set; } ///
        public decimal ItemTotal { get; set; }  /// Item Price
        public decimal ItemQtyTotal { get; set; }  //Item * Qty


        public decimal VariationTotal { get; set; }//variation price  
        public decimal VariationQtyTotal { get; set; }//variation price  * qty

        public decimal ChargesTotal { get; set; }   //charges 

        public decimal ChargesQtyTotal { get; set; }  //charges * qty

        public decimal SaleChargesTotal { get; set; }   //sales commission 

        public decimal SaleChargesQtyTotal { get; set; }  //sales commission  * qty


        public decimal ShippingCost { get; set; }   //shipping

        public decimal ShippingQtyCost { get; set; }   ///shipping * qty

        public decimal WalletAmount { get; set; }//deduction
        public decimal CouponAmount { get; set; } //deduction  

      
        public int? CouponId { get; set; } //coupon used id 
        public string? CouponCode { get; set; }



        public decimal DiscountAmount { get; set; } //deduction  
        public decimal Total { get; set; }   

        public decimal TotalQty { get; set; }


        public decimal GrandTotal { get; set; }



        public decimal Commission { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal BaseGrandTotal { get; set; }

      
    }

    #endregion

    #region CouponValidation
    public class CouponValidationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public decimal DiscountedAmount { get; set; }

        public int CouponId { get; set; }

       
    }
    #endregion

    #region LoyaltyPointsValidation
    public class LoyaltyPointsViewModel
    {
        public decimal Points { get; set; }
        public DateTime ExpirtyDate { get; set; }

        public string PointsType { get; set; } //add or substraction


    }


    public class LoyaltyPointsAvailableViewModel
    {
        public decimal AvailablePoints { get; set; }
         public decimal MinRedeemPoints { get; set; }

        public bool IsEnabled { get; set; }


    }
    #endregion

}
