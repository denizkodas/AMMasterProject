using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace AMMasterProject.ViewModel
{
    public class MembershipViewModel
    {
       

    }

    #region CreditPackageViewModel
    public class CreditPackageViewModel
    {
        public int RevenueCreditID { get; set; }
        public string RevenueCreditName { get; set; }
        public int NoofCredit { get; set; }

     

        public decimal CreditAmount { get; set; }

        public string Currency { get; set; }

     

        public string Description { get; set; }

        public string? CreditImage { get; set; }

        public bool IsExpiry { get; set; }

        public int? NoofExpiryDays { get; set; }

        public bool IsRecommended { get; set; }


    }
    #endregion



    #region SubscriptionPackageViewModel
    public class SubscriptionPackageViewModel
    {
        public int RevenueSubscriptionPackageID { get; set; }
        public string RevenuePackageName { get; set; }

        public int RecurringPeriodInDays { get; set; }

        public decimal CreditAmount { get; set; }

        public string Currency { get; set; }



        public string Description { get; set; }

        public string? CreditImage { get; set; }

      

        public bool IsRecommended { get; set; }


    }
    #endregion


    #region MembershipBasketSummary
    public class MembershipBasketSummary
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string Currency { get; set; }

        public decimal Pricing { get; set; }

     


    }
    #endregion

    #region CreditCounterViewModel
    public class CreditCounterViewModel
    {
       public bool IsEnable { get; set; }

        public int CreditAvailable { get; set; }
    }


    #endregion

    #region SubscriptionActiveViewModel
    public class SubscriptionActiveViewModel
    {
        public bool IsEnable { get; set; }  //check if admin has enabled then only show it

       public  SubscriptionPurchaseViewModel  subscriptionviewmodel { get; set; }
    }


    #endregion


    #region CreditPurchaseUserBillingModel
    public class CreditPurchaseViewModel
    {
        public int Credit { get; set; }

      
        public ItemMetaData ItemMetaData { get; set; }


        public CreditUsageMetaDataViewModel ItemUsageMetaData { get; set; }
        public PaymentMetaDataViewModel PaymentMetaData { get; set; }


        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }

        public bool IsExpiry { get; set; }


        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExiryDate { get; set; }

        public string InvoiceNumber { get; set; }

        public string TransactionType { get; set; }



    }
    #endregion


    #region SubscriptionPurchaseUserBillingModel
    public class SubscriptionPurchaseViewModel
    {
       

        public SubscriptionMetaDataViewModel ItemMetaData { get; set; }
        public PaymentMetaDataViewModel PaymentMetaData { get; set; }


        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }

        public bool IsExpiry { get; set; }


        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }

        public string InvoiceNumber { get; set; }

        public string TransactionType { get; set; }



    }
    #endregion


    #region PurchaseBillingModel
    //public class PurchaseHistoryViewModel
    //{


    //    public PurchaseMetaDataViewModel ItemMetaData { get; set; }
    //    public PaymentMetaDataViewModel PaymentMetaData { get; set; }


    //    [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime? PurchaseDate { get; set; }

    //    public bool IsExpiry { get; set; }


    //    [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
    //    public DateTime? ExiryDate { get; set; }

    //    public string InvoiceNumber { get; set; }

    //    public string TransactionType { get; set; }

    //    public string IncomingOutgoing { get; set; } 

    //    public string ItemType { get; set; } 

    //}
    #endregion



    /// <summary>
    /// User Billing
    /// </summary>
    #region PurchaseHistoryMetaDataForOrder
    //public class PurchaseMetaDataViewModel
    //{

    //    public int ID { get; set; }
    //    public string Name { get; set; }
    

   
    //    public bool IsExpiry { get; set; }
    //    public int? NoOfExpiryDays { get; set; }


    //    public string InvoiceNumber { get; set; }


    //}
    #endregion



    /// <summary>
    /// Order Creation
    /// </summary>
    #region CreditItemMetaDataForOrder
    //public class CreditMetaDataViewModel
    //{

    //    public int  RevenueCreditId { get; set; }
    //    public string Name { get; set; }
    //    public int NoOfCredit { get; set; }

    //    public int Quantity { get; set; }

    //    public decimal ActualAmount { get; set; }

    //    public string ActualCurrency { get; set; }


    //    public decimal ConversionAmount { get; set; }

    //    public string ConversionCurrency { get; set; }
    //    public bool IsExpiry { get; set; }
    //    public int? NoOfExpiryDays { get; set; }

    //    public DateTime? ExpiryDate { get; set; }   
    //    public string InvoiceNumber { get; set; }


    //}
    #endregion


    #region CreditUsageMetaData
    public class CreditUsageMetaDataViewModel
    {

        public int UsageID { get; set; }

        public int NoOfCredit { get; set; }
        public string Description { get; set; }

        public string ItemURL { get; set; }
     
        public DateTime? UsageDate { get; set; }
     

    }
    #endregion


    #region CreditLifeTimeUsed
    public class CreditLifeTimeMetaDataViewModel
    {
        public string ActualCurrency { get; set; }
        public decimal ActualCreditPurchaseAmount { get; set; }


        //public string Currency { get; set; }
        public decimal CreditPurchasedAmount { get; set; }
        public int CreditQTY { get; set; }

        public int CreditUsed { get; set; }


        ///
        public string MonthsPurchasedJson {get;set;}

        public string MonthsUsedJson { get; set; }


        public string LastFiveMonthsName { get; set; }

    }
    #endregion


    #region SubscriptionLifeTimeUsed
    public class SubscriptionLifeTimeMetaDataViewModel
    {
        public string ActualCurrency { get; set; }
   

        //public string Currency { get; set; }
        public decimal ActualPurchasedAmount { get; set; }
 
      

        ///
        public string PackageName { get; set; }


        public DateTime SubscriptionDate { get; set; }

        public DateTime RenewDate { get; set; }

    }
    #endregion
    /// <summary>
    /// Order Creation
    /// </summary>
    #region SubscriptionItemMetaDataForOrder
    public class SubscriptionMetaDataViewModel
    {

        public int RevenueSubscriptionPackageID { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }
        public int RecurringPeriodInDays { get; set; }

        public DateTime ExpiryDate { get; set; }

        public decimal ActualAmount { get; set; }

        public string ActualCurrency { get; set; }


        public decimal ConversionAmount { get; set; }

        public string ConversionCurrency { get; set; }
      
     
        public string InvoiceNumber { get; set; }


    }
    #endregion


    #region CreditAssignByAdmin
    public class CreditAssignByAdminViewModel
    {

        [Required(ErrorMessage = "Assign Credit is required")]
        public int AssignCredit { get; set; }

        [Required(ErrorMessage = "Payment Reference is required")]
        [StringLength(1000, ErrorMessage = "Payment Reference must not exceed 1000 characters")]
        public string PaymentReference { get; set; }

        [Required(ErrorMessage = "Payment Date is required")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Payment Method is required")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Amount Paid is required")]
        public decimal AmountPaid { get; set; }







    }

    
    public class CreditAssignUserViewModel
    {
        public int ProfileID { get; set; }
        public Guid ProfileGUID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public string Description { get; set; }

        public int AvailableCredits { get; set; }

        public string Email { get; set; }

    }
 
    #endregion
}
