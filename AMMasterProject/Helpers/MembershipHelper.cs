using Amazon;
using Amazon.S3.Model;
using AMMasterProject.Models;
using AMMasterProject.ViewModel;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PayPal.Api;
using System.Data;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace AMMasterProject.Helpers
{
    public class MembershipHelper
    {

        private readonly MyDbContext _dbContext;
        private readonly WebsettingHelper _websettinghelper;
        private readonly GlobalHelper _globalhelper;
        //private readonly OrderHelper _orderHelper;
        public MembershipHelper(MyDbContext context, WebsettingHelper websettinghelper, GlobalHelper globalHelper)
        {
            _dbContext = context;
            _websettinghelper = websettinghelper;
            _globalhelper = globalHelper;
            //_orderHelper = orderHelper;
        }


        #region CreditPackage
        public List<CreditPackageViewModel> CreditPackageList()
        {

            var userselectedcurrency = _globalhelper.GetUserCurrency();
            var conversionrate = _globalhelper.ConversionRate(userselectedcurrency);


            var creditpackagelist = (from credit in _dbContext.RevenueCreditPackage
                                     join currency in _dbContext.Currencies on credit.CurrencyID equals currency.CurrencyId
                                     orderby credit.Sortnumber
                                     where credit.IsDeleted == false && credit.IsPublish == true
                                     select new CreditPackageViewModel
                                     {
                                         RevenueCreditID = credit.RevenueCreditID,
                                         RevenueCreditName = credit.RevenueCreditName,
                                         NoofCredit = credit.NoofCredit,
                                         CreditAmount = Math.Round(credit.CreditAmount * conversionrate, 2), // Round to 2 decimal places
                                         Currency = userselectedcurrency, //currency.CurrencyName,
                                         Description = credit.Description,
                                         CreditImage = credit.CreditImage,
                                         IsExpiry = credit.IsExpiry,
                                         NoofExpiryDays = credit.NoofExpiryDays,
                                         IsRecommended = credit.IsRecommended,
                                     });

            return creditpackagelist.ToList();
        }

        #endregion




        #region SubscriptionPackage

        public List<SubscriptionPackageViewModel> SubscriptionPackageList()
        {



            var subscriptionpackagelist = (from subscription in _dbContext.RevenueSubscriptionPackage
                                           join currency in _dbContext.Currencies on subscription.CurrencyID equals currency.CurrencyId
                                           orderby subscription.Sortnumber
                                           where subscription.IsDeleted == false && subscription.IsPublish == true
                                           select new SubscriptionPackageViewModel
                                           {
                                               RevenueSubscriptionPackageID = subscription.RevenueSubscriptionPackageID,
                                               RevenuePackageName = subscription.RevenuePackageName,
                                               RecurringPeriodInDays = subscription.RecurringPeriodInDays,
                                               CreditAmount = subscription.CreditAmount,
                                               Currency = currency.CurrencyName,
                                               Description = subscription.Description,
                                               CreditImage = subscription.SubscriptionImage,
                                               IsRecommended = subscription.IsRecommended



                                           }


                                     );

            return subscriptionpackagelist.ToList();
        }
        #endregion


        #region MembershipBasketSummary  depreciated
      
        
        public MembershipBasketSummary MembershipBasketSummary(string ID, string membershiptype)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            //int ID = 0;
            //string membershiptype = "credit";  ///subscription
            MembershipBasketSummary basketsummary = null;


            if (membershiptype == "credit")
            {
                int creditpackageid = int.Parse(ID.ToString());
                basketsummary = (from credit in CreditPackageList()
                                 where credit.RevenueCreditID == creditpackageid
                                 select new MembershipBasketSummary
                                 {
                                     ID = credit.RevenueCreditID.ToString(),
                                     Name = credit.RevenueCreditName,
                                     Currency = credit.Currency,
                                     Pricing = credit.CreditAmount,



                                 }

                                 ).FirstOrDefault();
            }
            else if (membershiptype == "subscription")
            {
                int subscriptionpackageid = int.Parse(ID.ToString());
                basketsummary = (from subscription in SubscriptionPackageList()
                                 where subscription.RevenueSubscriptionPackageID == subscriptionpackageid
                                 select new MembershipBasketSummary
                                 {
                                     ID = subscription.RevenueSubscriptionPackageID.ToString(),
                                     Name = subscription.RevenuePackageName,
                                     Currency = subscription.Currency,
                                     Pricing = subscription.CreditAmount,



                                 }

                                ).FirstOrDefault();
            }

            else if (membershiptype == "item")
            {
               
                //basketsummary = (from order in _orderHelper.GetOrdersItem()
                //                 where order.InvoieNumber == ID
                //                 select new MembershipBasketSummary
                //                 {
                //                     ID = ID,
                //                     Name = "Item Purchased",
                //                     Currency = order.ItemDetailMetaData.Currency,
                //                     Pricing = order.SummaryOrderMetaData.ItemTotal,



                //                 }

                //                ).FirstOrDefault();
            }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.



            return basketsummary;
        }

        #endregion


        #region CreditonSignup
        public int CreditOnSignup(string usertype)
        {
            int freecredit = 0;
            var _creditsystemSettings = _websettinghelper.GetWebsettingJson("CreditSystemSettings");
            if (_creditsystemSettings != null && !string.IsNullOrEmpty(_creditsystemSettings))
            {

                var json = JsonConvert.DeserializeObject<CreditSystemSettingsModel>(_creditsystemSettings);

                if (json != null)
                {
                    if (json.IsEnable == true)
                    {


                        if (usertype == "Client")
                        {
                            freecredit = json.FreeCreditForBuyer;
                        }
                        else if (usertype == "Vendor")
                        {
                            freecredit = json.FreeCreditForSeller;
                        }
                    }


                }


            }
            return freecredit;
        }


        #endregion

        #region CreditAvailable
        public int CreditAvailable(int profileId)
        {
            // Retrieve orderMasters objects from the database
            // Retrieve orderMasters objects from the database
            var orderMasters = _dbContext.OrderMasters
        .Where(u => u.BuyerId == profileId && u.IsDeleted == false && u.ItemType == "credit" && u.OrderStatus == "confirm" && u.OrderProcessStatus== "completed" && (u.ExpiryDate == null || u.ExpiryDate >= DateTime.Now))
        .ToList();

            // Deserialize the JSON data and calculate the sum of NoOfCredit
            int totalNoOfCredits = orderMasters
                .Select(o =>
                {
                    var metadata = JsonConvert.DeserializeObject<ItemMetaData>(o.ItemMetaData);

                    return metadata.creditModel.NoOfCredit;

                })
                .Sum();
           

            return totalNoOfCredits;
        }
        #endregion


        #region CreditPurchaseLifeTime
        public CreditLifeTimeMetaDataViewModel CreditPurchaseLifeTime(int profileId)
        {
            CreditLifeTimeMetaDataViewModel creditMetaData = new CreditLifeTimeMetaDataViewModel();

            var orderMasters = _dbContext.OrderMasters
                .Where(u => u.BuyerId == profileId && u.IsDeleted == false && u.ItemType == "credit")
                .ToList();

            // Calculate the sum of NoOfCredit by deserializing and summing the metadata for each order
            int totalNoOfCreditsPurchased = orderMasters
                .Where(o => o.TransactionType == "purchased" || o.TransactionType =="free")
                .Sum(o =>
                {
                    var metadata = JsonConvert.DeserializeObject<ItemMetaData>(o.ItemMetaData);
                    return metadata.creditModel.NoOfCredit;
                });

            int totalNoOfCreditsUsed = orderMasters
              .Where(o => o.TransactionType == "used")
              .Sum(o =>
              {
                  var metadata = JsonConvert.DeserializeObject<ItemMetaData>(o.ItemMetaData);
                  return metadata.creditModel.NoOfCredit;
              });

            decimal totalAmount = orderMasters
             .Where(o => o.TransactionType == "purchased" || o.TransactionType == "free")
             .Sum(o =>
             {
                 var metadata = JsonConvert.DeserializeObject<SummaryModel>(o.ItemMetaData);
                 return metadata.GrandTotal;
             });

            // Assign the calculated totalNoOfCredits to the creditMetaData
            creditMetaData.CreditQTY = totalNoOfCreditsPurchased;
            creditMetaData.CreditUsed = totalNoOfCreditsUsed;
            creditMetaData.CreditPurchasedAmount = totalAmount;
            creditMetaData.ActualCurrency = _globalhelper.BaseCurrency();


            //Calculate the last 5 months' NoOfCredit values and store them as a comma-separated string
            //List<int> last5MonthsNoOfCredit = orderMasters
            //    .Where(o => o.TransactionType == "purchased")
            //    .OrderByDescending(o => o.OrderDate)
            //    .Take(5)
            //    .Select(o =>
            //    {
            //        var metadata = JsonConvert.DeserializeObject<CreditMetaDataViewModel>(o.ItemMetaData);
            //        return metadata.NoOfCredit;
            //    })
            //    .ToList();
            List<string> last5MonthsNoOfCredit = orderMasters
      .Where(o => o.TransactionType == "purchased" || o.TransactionType == "free")
      .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
      .OrderByDescending(g => g.Key.Year)
      .ThenByDescending(g => g.Key.Month)
      .TakeWhile((g, index) => index <= 4) // Take the last 5 months, including the current month
      .Select(g =>
      {
          var totalNoOfCredit = g.Sum(o =>
          {
              var metadata = JsonConvert.DeserializeObject<ItemMetaData>(o.ItemMetaData);
              return metadata.creditModel.NoOfCredit;
          });

          var month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy");
          return $"{month}: {totalNoOfCredit}";
      })
      .ToList();


            creditMetaData.MonthsPurchasedJson = JsonConvert.SerializeObject(last5MonthsNoOfCredit);

            List<string> last5MonthsNoOfCreditUsed = orderMasters
                .Where(o => o.TransactionType == "used")
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .OrderByDescending(g => g.Key.Year)
                .ThenByDescending(g => g.Key.Month)
                .TakeWhile((g, index) => index < DateTime.Now.Month) // Take months up to and including the current month
                .Select(g =>
                {
                    var totalNoOfCredit = g.Sum(o =>
                    {
                        var metadata = JsonConvert.DeserializeObject<ItemMetaData>(o.ItemMetaData);
                        return metadata.creditModel.NoOfCredit;
                    });

                    var month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy");
                    return $"{month}: {Math.Abs(totalNoOfCredit)}";
                })
                .ToList();



            //creditMetaData.MonthsUsedJson = string.Join(", ", last5MonthsNoOfCreditUsed);

            creditMetaData.MonthsUsedJson = JsonConvert.SerializeObject(last5MonthsNoOfCreditUsed);



            return creditMetaData;
        }
        #endregion



        #region SubscriptionActive
        public SubscriptionPurchaseViewModel SubscriptionActive(int profileId)
        {

            var activesubscription = (from order in _dbContext.OrderMasters

                                      orderby order.OrderDate descending
                                      where order.BuyerId == profileId
                                            && order.IsDeleted == false && order.ItemType == "subscription"

                                      select new SubscriptionPurchaseViewModel
                                      {


                                          ItemMetaData = ParseItemMetaDataSubscription(order.ItemMetaData),
                                          PurchaseDate = order.OrderDate,
                                          IsExpiry = order.IsExpiry,
                                          ExpiryDate = order.ExpiryDate,
                                          PaymentMetaData = ParsePaymentMetaData(order.PaymentMetaData),
                                          TransactionType = order.TransactionType
                                      }).FirstOrDefault();


            return activesubscription;

        }
        #endregion

        #region CreditPurchaseList


        public static ItemMetaData Parsecreditmetadata(string json)
        {
           
            var parsedData = JsonConvert.DeserializeObject<ItemMetaData>(json);
            return parsedData;
        }

        public static CreditUsageMetaDataViewModel ParseItemUsageMetaData(string json)
        {
            // Perform the necessary JSON parsing and mapping here
            // You can use JsonConvert.DeserializeObject or any other JSON library of your choice

            // Example: Assuming the JSON structure matches the desired CreditCommentViewModel properties
            var parsedData = JsonConvert.DeserializeObject<CreditUsageMetaDataViewModel>(json);
            return parsedData;
        }

        public List<CreditPurchaseViewModel> CreditPurchaseList(int profileid)
        {
            var creditpurchaselist = (from order in _dbContext.OrderMasters
                                      orderby order.OrderId descending
                                      where order.BuyerId == profileid
                                            && order.IsDeleted == false && order.ItemType == "credit" && order.OrderStatus=="confirm"
                                      select new CreditPurchaseViewModel
                                      {



                                          ItemMetaData = Parsecreditmetadata(order.ItemMetaData),
                                          ItemUsageMetaData = ParseItemUsageMetaData(order.ItemMetaData),
                                          PurchaseDate = order.OrderDate,
                                          IsExpiry = order.IsExpiry,
                                          ExiryDate = order.ExpiryDate,
                                          PaymentMetaData = ParsePaymentMetaData(order.PaymentMetaData),
                                          TransactionType = order.TransactionType
                                      });

            return creditpurchaselist.ToList();
        }
        #endregion


        #region SubscriptionPurchaseList

        public static SubscriptionMetaDataViewModel ParseItemMetaDataSubscription(string json)
        {
            // Perform the necessary JSON parsing and mapping here
            // You can use JsonConvert.DeserializeObject or any other JSON library of your choice

            // Example: Assuming the JSON structure matches the desired CreditCommentViewModel properties
            var parsedData = JsonConvert.DeserializeObject<SubscriptionMetaDataViewModel>(json);
            return parsedData;
        }



        public List<SubscriptionPurchaseViewModel> SubscriptionPurchaseList(int profileid)
        {
            var subscriptionpurchaselist = (from order in _dbContext.OrderMasters
                                            orderby order.OrderId descending
                                            where order.BuyerId == profileid && order.IsDeleted == false
                                             && order.ItemType == "subscription" && order.OrderStatus == "confirm"
                                            select new SubscriptionPurchaseViewModel
                                            {

                                                ItemMetaData = ParseItemMetaDataSubscription(order.ItemMetaData),
                                                PurchaseDate = order.OrderDate,
                                                IsExpiry = order.IsExpiry,
                                                ExpiryDate = order.ExpiryDate,
                                                PaymentMetaData = ParsePaymentMetaData(order.PaymentMetaData),
                                                TransactionType = order.TransactionType
                                            });

            return subscriptionpurchaselist.ToList();
        }
        #endregion


        #region SubscriptionLifeTimeData
        public List<SubscriptionLifeTimeMetaDataViewModel> SubscriptionPurchaseLifeTimeList(int profileid)
        {
            var orderMasters = _dbContext.OrderMasters
                .Where(u => u.BuyerId == profileid && u.IsDeleted == false && u.ItemType == "subscription" && u.OrderStatus=="confirm")
                .ToList();

            var subscriptionData = orderMasters
                .Where(o => o.TransactionType == "purchased")
                .Select(o =>
                {
                    var metadata = JsonConvert.DeserializeObject<SubscriptionMetaDataViewModel>(o.ItemMetaData);
                    return new SubscriptionLifeTimeMetaDataViewModel
                    {
                        PackageName = metadata.Name,
                        SubscriptionDate = o.OrderDate,
                        RenewDate = o.ExpiryDate.HasValue ? o.ExpiryDate.Value : default(DateTime)
                    };
                })
                .ToList();

            return subscriptionData;
        }


        #endregion


        #region PurchaseHistory

        //public static PurchaseMetaDataViewModel ParseItemMetaDataPurchase(string json)
        //{

        //    var parsedData = JsonConvert.DeserializeObject<PurchaseMetaDataViewModel>(json);
        //    return parsedData;
        //}



        //public List<PurchaseHistoryViewModel> PurchaseHistoryList(int profileid)
        //{
        //    var result = _dbContext.OrderMasters
        //        .Where(order => order.BuyerId == profileid &&
        //                        order.IsDeleted == false &&
        //                        ((order.TransactionType == "purchased" && order.OrderStatus == "confirm" && order.PaymentStatus == "paid") ||
        //                         (order.TransactionType == "wallet" && order.OrderStatus == "confirm" && order.PaymentStatus == "received")))
        //        .Select(order => new PurchaseHistoryViewModel
        //        {
        //            ItemMetaData = ParseItemMetaDataPurchase(order.ItemMetaData),
        //            PurchaseDate = order.OrderDate,
        //            PaymentMetaData = ParsePaymentMetaData(order.PaymentMetaData),
        //            TransactionType = order.TransactionType,
        //            IncomingOutgoing = DetermineIncomingOutgoing(order.TransactionType),
        //            InvoiceNumber = order.InvoiceNumber,
        //            ItemType = order.ItemType
        //        })
        //        .OrderByDescending(o => o.PurchaseDate)
        //        .ToList();

        //    return result;
        //}

     


        #endregion





        #region CreditDeductionRequired
        public int CreditDeductionRequired(string creditType)
        {
            int requiredcredit = 0;
            var _creditsystemSettings = _websettinghelper.GetWebsettingJson("CreditSystemSettings");
            if (_creditsystemSettings != null && !string.IsNullOrEmpty(_creditsystemSettings))
            {

                var json = JsonConvert.DeserializeObject<CreditSystemSettingsModel>(_creditsystemSettings);



                if (json != null)
                {
                    if (json.IsEnable == true)
                    {

                        //1. Check in settting how much required required
                        if (creditType == "address")
                        {
                            requiredcredit = json.DeductionOnAddressView;
                        }
                        else if (creditType == "contact")
                        {
                            requiredcredit = json.DeductionOnContactView;
                        }

                        else if (creditType == "email")
                        {
                            requiredcredit = json.DeductionOnEmailView;
                        }





                    }
                    else
                    {
                        requiredcredit = 0;
                    }


                }
            }



            return requiredcredit;

        }
        #endregion



        #region PaymentMetadaJsonConversion
        public static PaymentMetaDataViewModel ParsePaymentMetaData(string json)
        {
            if (json == null)
            {
                return new PaymentMetaDataViewModel(); // Return an empty model
            }
            // Perform the necessary JSON parsing and mapping here
            // You can use JsonConvert.DeserializeObject or any other JSON library of your choice

            // Example: Assuming the JSON structure matches the desired CreditCommentViewModel properties
            var parsedData = JsonConvert.DeserializeObject<PaymentMetaDataViewModel>(json);
            return parsedData;
        }
        #endregion

        #region Jsoncreator-CreditMetaData

        //public string CreditMetaData(int revenuecreditid, string invoicenumber, string custom = null)
        //{

        //    string ItemMetaData = string.Empty;

        //    string name = string.Empty;
        //    int noofcredit = 0;
        //    int currencyid = 0;
        //    string currencyname = string.Empty;
        //    decimal creditamount = 0;
        //    bool isexpiry = false;
        //    int noofexpirydays = 0;

        //    if (revenuecreditid == 0) //free sign up credit
        //    {
        //        string[] customValues = custom.Split(',');

        //        name = customValues[0].Trim();
        //        noofcredit = int.Parse(customValues[1].Trim());
        //        creditamount= decimal.Parse(customValues[2].Trim());



        //    }
            

        //    else
        //    {
        //        RevenueCreditPackage package = _dbContext.RevenueCreditPackage.FirstOrDefault(u => u.RevenueCreditID == revenuecreditid);

        //        if (package == null)
        //        {
        //            return "fail - Package does not exist ID: " + revenuecreditid;
        //        }
        //        name = package.RevenueCreditName + " - No. of Credits: "+ package.NoofCredit;
        //        noofcredit = package.NoofCredit;
        //        currencyid = package.CurrencyID;
        //        creditamount = package.CreditAmount;
        //        isexpiry = package.IsExpiry;

        //        if (package.IsExpiry == true)
        //        {
        //            noofexpirydays = int.Parse(package.NoofExpiryDays.ToString());
        //        }





        //    }

        //    #region CreateJsonForComments


        //    string actualcurrency = _globalhelper.GetCurrentCurrency(currencyid);
        //    string userselectedcurrency = _globalhelper.GetUserCurrency();
        //    // Create a new instance of CreditCommentViewModel and assign the values
        //    var metadata = new CreditMetaDataViewModel
        //    {
        //        RevenueCreditId = revenuecreditid,
        //        Name = name,
        //        NoOfCredit = noofcredit,
        //        Quantity=1,

        //        ActualCurrency = actualcurrency,
        //        ActualAmount = creditamount,

        //        ConversionAmount = _globalhelper.GetCurrencyConversion(actualcurrency, userselectedcurrency, creditamount.ToString()),
        //        ConversionCurrency = userselectedcurrency,

        //        IsExpiry = isexpiry,
        //        NoOfExpiryDays = noofexpirydays,

        //        ExpiryDate = isexpiry ? DateTime.Now.AddDays(noofexpirydays) : (DateTime?)null,


        //        InvoiceNumber = invoicenumber,
        //    };

        //    // Serialize the object to JSON
        //    ItemMetaData = JsonConvert.SerializeObject(metadata);
        //    #endregion

        //    return ItemMetaData;

        //}
        #endregion

        #region Jsoncreator-CreditUsageMetaData
        public string CreditUsageMetaData(string description, int noofcredit, string ItemURL)
        {

            string ItemMetaData = string.Empty;


            #region CreateJsonForComments



            // Create a new instance of CreditCommentViewModel and assign the values
            var metadata = new CreditUsageMetaDataViewModel
            {
                UsageID = int.Parse(GlobalHelper.RandomNumber().ToString()),
                Description = description,
                NoOfCredit = -noofcredit,
                ItemURL = ItemURL,
                UsageDate = DateTime.Now

            };

            // Serialize the object to JSON
            ItemMetaData = JsonConvert.SerializeObject(metadata);
            #endregion

            return ItemMetaData;

        }

        #endregion

        #region Jsoncreator-SubscriptionMetaData
        public string SubscriptionMetaData(int RevenueSubscriptionPackageID, string invoicenumber, string custom = null)
        {

            string ItemMetaData = string.Empty;

            string name = string.Empty;

            int currencyid = 0;
            string currencyname = string.Empty;
            decimal creditamount = 0;



            RevenueSubscriptionPackage package = _dbContext.RevenueSubscriptionPackage.FirstOrDefault(u => u.RevenueSubscriptionPackageID == RevenueSubscriptionPackageID);

            if (package == null)
            {
                return "fail - Package does not exist ID: " + RevenueSubscriptionPackageID;
            }
            name = package.RevenuePackageName;

            currencyid = package.CurrencyID;
            creditamount = package.CreditAmount;





            #region CreateJsonForComments


            string actualcurrency = _globalhelper.GetCurrentCurrency(currencyid);
            string userselectedcurrency = _globalhelper.GetUserCurrency();
            // Create a new instance of CreditCommentViewModel and assign the values
            var metadata = new SubscriptionMetaDataViewModel
            {
                RevenueSubscriptionPackageID = RevenueSubscriptionPackageID,
                Name = name + " Expiry Date: " + DateTime.Now.AddDays(package.RecurringPeriodInDays),
                RecurringPeriodInDays = package.RecurringPeriodInDays,
                ActualCurrency = actualcurrency,
                ActualAmount = creditamount,
                ConversionAmount = _globalhelper.GetCurrencyConversion(actualcurrency, userselectedcurrency, creditamount.ToString()),
                ConversionCurrency = userselectedcurrency,
                InvoiceNumber = invoicenumber,
                ExpiryDate = DateTime.Now.AddDays(package.RecurringPeriodInDays),
                Quantity = 1,

            };

            // Serialize the object to JSON
            ItemMetaData = JsonConvert.SerializeObject(metadata);
            #endregion

            return ItemMetaData;

        }
        #endregion



    }
}
