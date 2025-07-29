using Amazon;
using Amazon.S3.Model;
using AMMasterProject.Models;
using AMMasterProject.ViewModel;
using Google.Apis.Storage.v1.Data;
using Google.Apis.Util;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using Razorpay.Api;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace AMMasterProject.Helpers
{
    public class OrderHelper
    {
        #region Model


        private readonly MyDbContext _dbContext;
        ////private readonly MembershipHelper _membershipHelper;
        private readonly NotificationHelper _notificationhelper;
        private readonly ProductHelper _productHelper;
        private readonly GlobalHelper _globalhelper;
        private readonly UserHelper _userhelper;
        private readonly WebsettingHelper _websettinghelper;

        public string ChargesMetaData { get; set; }
        public int SellerID { get; set; }
        #endregion

        #region DI


        public OrderHelper(MyDbContext context, MembershipHelper membershipHelper, NotificationHelper notificationHelper, ProductHelper productHelper, GlobalHelper globalhelper, UserHelper userhelper,    WebsettingHelper websettinghelper)
        {
            _dbContext = context;
            //_membershipHelper = membershipHelper;
            _notificationhelper = notificationHelper;
            _productHelper = productHelper;
            _globalhelper = globalhelper;
            _userhelper = userhelper;
            _websettinghelper = websettinghelper;
        }
        #endregion

        #region Order-Creation  1

        //if there is already active ITEM only in cart so get that invoice number
        public string invoicenumberactive(int buyerid, string Id, string type)
        {

            string InvoiceNumber= "";

            var ordersToUpdate = _dbContext.OrderMasters.FirstOrDefault(u => u.BuyerId == buyerid && u.OrderStatus == "cart");
            if(ordersToUpdate == null)
            {
                InvoiceNumber = GlobalHelper.GetInvoiceNumber(Id, type);
            }
            else
            {
                InvoiceNumber = ordersToUpdate.InvoiceNumber;
            }

            return InvoiceNumber;

        }

        public string OrderCreation(int buyerid, int sellerid, string itemtype, string orderStatus, string transactiontype, string orderprocessstatus, string invoicenumber, string itemmetadata, string paymentmetdata, string paymentstatus, string? summarymetadata="", string? ChargesMetaData="")
        {

            try
            {

              

          
                OrderMaster om = new OrderMaster();

                om.BuyerId = buyerid;
               
                om.ItemType = itemtype;    //credit, subscription item, itemboost, wallet
                om.TransactionType = transactiontype;    //free, purchased, used
                om.OrderStatus = orderStatus;  // Cart Confirm Cancel  this is for user CArt to confirm only payment is processed
                om.OrderProcessStatus = orderprocessstatus; //Processing Completed  Cancelled-- it should have only 3 status this is for admin
                om.PaymentStatus = paymentstatus;
                om.InvoiceNumber = invoicenumber;
                om.ItemMetaData = itemmetadata;
                om.PaymentMetaData = paymentmetdata;
                om.SellerId = sellerid;
                //om.LoyaltyPointsMetaData = loyaltypointsmetadata;

                string orderStatusDescription = "";
                if (orderStatus == "cart")
                {
                    orderStatusDescription = "item added to cart";
                    om.OrderStatusMetaData = OrderStatusmetadata(orderStatus, DateTime.Now, orderStatusDescription, invoicenumber, null);
                    
                }
                else if(itemtype == "wallet")
                {
                    orderStatusDescription = "wallet created";
                    om.OrderStatusMetaData = OrderStatusmetadata(orderStatus, DateTime.Now, orderStatusDescription, invoicenumber, null);


                }

                


                //if (ChargesMetaData!="")
                //{
                //    om.OrderChargesMetaData = ChargesMetaData;
                //}

                om.SummaryMetaData = summarymetadata;

                om.OrderDate = DateTime.Now;
                om.IsDeleted = false;




                if (itemtype == "credit" && transactiontype != "used")
                {
                    // Deserialize the JSON string into your model class
                    //CreditMetaDataViewModel creditMetaData = JsonConvert.DeserializeObject<CreditMetaDataViewModel>(om.ItemMetaData);
                    //om.IsExpiry = creditMetaData.IsExpiry;
                    //if (creditMetaData.IsExpiry == true)
                    //{
                    //    om.ExpiryDate = creditMetaData.ExpiryDate;
                    //}

                    ///change it to call only when payment made
                    ItemMetaData creditMetaData = JsonConvert.DeserializeObject<ItemMetaData>(om.ItemMetaData);

                    if (creditMetaData.creditModel.IsExpiry == true)
                    {
                        om.ExpiryDate = creditMetaData.creditModel.ExpiryDate;
                    }
                    _notificationhelper.NotificationSet(buyerid, "Credit Assigned", "Congratulations! Your account has been credited with" + creditMetaData.creditModel.NoOfCredit + " credits.", "", "");
                }
                if (itemtype == "credit" && transactiontype == "used")
                {
                    // Deserialize the JSON string into your model class
                    ItemMetaData creditMetaData = JsonConvert.DeserializeObject<ItemMetaData>(om.ItemMetaData);

                    // when user used, so add it notification
                    _notificationhelper.NotificationSet(buyerid, "Credit Used", creditMetaData.basicModel.Description, "", creditMetaData.basicModel.ItemURL);
                }
                else if (itemtype == "subscription")
                {

                    //SubscriptionMetaDataViewModel subscriptionMetaData = JsonConvert.DeserializeObject<SubscriptionMetaDataViewModel>(om.ItemMetaData);
                    //om.IsExpiry = true;

                    //om.ExpiryDate = subscriptionMetaData.ExpiryDate;

                    //_notificationhelper.NotificationSet(buyerid, "Subscription", "Congratulations! You've successfully purchased the subscription " + subscriptionMetaData.Name, "", "");
                }
                else if (itemtype == "item")
                {

                    //generate session for invoice number
                    ///on payment mode remove invoicen umber from cookie
                    ///
                    //GlobalHelper.RemoveCookie("cartInvoiceNumber");

                    //ItemDetailMetaData creditMetaData = JsonConvert.DeserializeObject<ItemDetailMetaData>(om.ItemMetaData);
                    
                }


                ///Variaion MetaData
                ///Charges MetaData
                ///Shipping MetaData
                ///OtherMetaData Wallet, Coupon, Discount
                ///SummaryMetaData




                _dbContext.OrderMasters.Add(om);
                _dbContext.SaveChanges();


                ///Create Notification for user

                return "success";

            }
            catch (Exception)
            {

                return "fail";
            }


        }





        #endregion

       


        #region Order-PaymentUpdate 2
        //once payment is success, so it will take user login, invoice number and update in loop for all invoice number
        public string OrderPaymentUpdate(string paymentmethod, string paymentstatus, string paymentreference, string payerid, string paidamount, string paidcurrency, string payeremail, string invoicenumber, int buyerid, string OrderProcessStatus)
        {
            try
            {


                var ordersToUpdate = _dbContext.OrderMasters.Where(u => u.BuyerId == buyerid && u.OrderStatus == "cart" && u.InvoiceNumber == invoicenumber).ToList();


                string summaryModel = "";

                foreach (var order in ordersToUpdate)
                {



                    ///determine wallet first
                    ///
                    decimal AvailableWallet = _userhelper.WalletAvailable(buyerid);
                    decimal walletdeduction = 0;
                    SummaryModel SummaryMetaData = JsonConvert.DeserializeObject<SummaryModel>(order.SummaryMetaData);
                    ItemMetaData MetaData = JsonConvert.DeserializeObject<ItemMetaData>(order.ItemMetaData);
                    if (AvailableWallet > 0)
                    {
                        
                       

                        ///after wallet, discount or any thing user is paid this amount

                        decimal itemqtyamount = SummaryMetaData.GrandTotal;


                        ///shifted where wallet is calculated

                        if (AvailableWallet >= itemqtyamount)
                        {
                            walletdeduction = -itemqtyamount;

                            paidamount = itemqtyamount.ToString();

                        }
                        else
                        {
                            walletdeduction = -AvailableWallet;
                            paidamount = (itemqtyamount - AvailableWallet).ToString();

                        }



                        string walletInvoicenumber = GlobalHelper.GetInvoiceNumber(buyerid.ToString(), "wallet");
                        string description = $"wallet deduction of {walletdeduction} for order invoice number {order.InvoiceNumber} and item name {MetaData.basicModel.Name} ";

                        string custombuilder = $"{MetaData.paymentModel.ConversionCurrency}, {walletdeduction}, {walletdeduction},{MetaData.paymentModel.ConversionCurrency}";
                        string itemhelper = ItemMetaData("walletused", "", "", 0, 1, description, walletInvoicenumber,"","",custombuilder);

                        // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                        var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                        string ItemMetadata = itemMetadataResult.updatedjson;
                        string walletSummaryMetaData = itemMetadataResult.updatedsummaryjson;
                        int sellerid = 0;


                       string walletpaymentmetdata= paymentmetadata("wallet", "paid", walletInvoicenumber, string.Empty, DateTime.Now,  order.InvoiceNumber + " Order ID:" + order.OrderId.ToString(), order.ItemMetaData, walletdeduction.ToString(), paidcurrency, "", order.InvoiceNumber, walletdeduction);
                        
                        
                        string message = OrderCreation(buyerid, sellerid, "wallet", "confirm", "used", "confirm", walletInvoicenumber, ItemMetadata, walletpaymentmetdata, "used", walletSummaryMetaData);



                        ///re calculate summary add wallet amount or discount or any upates


                      
                    }
                    //else if(order.couponcodeid!=null)
                    //{
                    //    //similar to wallet calculate coupon discount for each
                    //}
                    else
                    {
                        paidamount = SummaryMetaData.GrandTotal.ToString();
                    }
                    ///update summary json on condition of wallet discount coupon
                    summaryModel = summaryModelMetaData(SummaryMetaData.Currency, SummaryMetaData.ItemTotal, MetaData.basicModel.Quantity,
                      SummaryMetaData.VariationTotal, SummaryMetaData.ChargesTotal, SummaryMetaData.ShippingCost, walletdeduction, SummaryMetaData.CouponAmount, 0, SummaryMetaData.SaleChargesQtyTotal,SummaryMetaData.CouponId,SummaryMetaData.CouponCode);
                    order.SummaryMetaData = summaryModel;

                    // Update the ShippingMetaData column for each order
                    order.PaymentMetaData = paymentmetadata(paymentmethod, paymentstatus, paymentreference, string.Empty, DateTime.Now, payerid, order.ItemMetaData, paidamount, paidcurrency, payeremail, order.InvoiceNumber, walletdeduction);
                    order.OrderStatus = "confirm";
                    order.PaymentStatus = paymentstatus;
                    order.OrderProcessStatus = OrderProcessStatus;   //completed, processing, cancelled,delivered
                   


                    order.UpdateDate = DateTime.Now;
                    order.OrderStatusMetaData = OrderStatusmetadata("processing", DateTime.Now, "Thank you for shopping! Your order has been received.", invoicenumber, order.OrderStatusMetaData);

                    ////if paymentstatus pending so run the order status update

                   
                    //in case of boost deserialied meta data and run product boost update


                    ///depreciated on 25 jan 2024 and shifted in creation of cart for boost
                    //if (order.ItemType == "profileboost" || order.ItemType =="itemboost")
                    //{
                    //    bool isactive = true;

                    //    if(paymentstatus=="pending")
                    //    {
                    //        isactive = false;
                    //    }

                    //     ItemBoostMetaDataViewModel ItemMetData = ParseMetaDataItemBoost(order.ItemMetaData);
                    //    _productHelper.productboost(ItemMetData.ItemBoostGUID, ItemMetData.itemtype, ItemMetData.StartDate, ItemMetData.EndDate, invoicenumber, isactive);
                    //}


                  

                    ///create wallet if itemmeta data has the records
                    ///

                    //string description = "wallet" + " - " + $"Wallet amount {walletamountDeduction} deduction for the order {}";

                    ///Wallet creation


                    //string walletmetdata = Walletmetadata(order.OrderId, order.InvoiceNumber, DateTime.Now, -walletamountDeduction, MetaData.ConversionCurrency, description);
                    //string orderstatus = OrderCreation(order.BuyerId, "wallet", "confirm", "wallet", "completed", invoicenumber, walletmetdata, "", "");

                }

                // Save the changes to the database
                _dbContext.SaveChanges();

                GlobalHelper.RemoveCookie("cartInvoiceNumber");
            }
            catch (Exception ex)
            {

                return "fail";
            }
            return "success";
        }
        #endregion

        #region OrderSummary 3
        public SummaryModel ordersummary(string invoicenumber)
        {
            try
            {
                SummaryModel model = null; // Initialize the model

                var q = _dbContext.OrderMasters.Where(u => u.InvoiceNumber == invoicenumber).ToList();


                foreach (var order in q)
                {

                    model = ParseMetaDataOrderSummary(order.SummaryMetaData);
                }

                return model;
            }
            catch (Exception)
            {
                return null; // Return null in case of an exception
            }
        }

        #endregion

        #region Shipping-Update
        public string ShippingUpdateCart(int buyerid, string shippingmetadata)
        {
            var ordersToUpdate = _dbContext.OrderMasters.Where(u => u.BuyerId == buyerid && u.OrderStatus == "cart").ToList();


            string invoicenumber = GlobalHelper.ReadCookie("cartInvoiceNumber");

            if (invoicenumber == null)
            {
                invoicenumber = GlobalHelper.GetInvoiceNumber(buyerid.ToString(), "Item");

                GlobalHelper.SetCookie("cartInvoiceNumber", invoicenumber);
            }

            foreach (var order in ordersToUpdate)
            {
                // Update the ShippingMetaData column for each order
                order.ShippingMetaData = shippingmetadata;
                order.InvoiceNumber = invoicenumber;
            }





            // Save the changes to the database
            _dbContext.SaveChanges();


            string redirectUrl = $"/Payment/selection/{invoicenumber}/item";
            return redirectUrl;
        }
        #endregion

        #region Order-Update
        public string OrderUpdate(int orderid, string? orderStatus, string? orderprocessstatus, string? ordernotes, string? paymentstatus, string? itemmetadata, string? paymentmetdata)
        {
            OrderMaster om = _dbContext.OrderMasters.FirstOrDefault(u => u.OrderId == orderid);

            if (om != null)
            {
                ///first update the paremater in itemotherdata

                if (orderprocessstatus == "completed" & om.OrderProcessStatus != "completed")
                {

                    ItemMetaData ordermetadata = JsonConvert.DeserializeObject<ItemMetaData>(om.ItemMetaData);
                    SummaryModel summaryMetaData = JsonConvert.DeserializeObject<SummaryModel>(om.SummaryMetaData);
                    string loyaltypoints = OrderLoyaltypointsCreation(om.LoyaltyPointsMetaData, "Add", summaryMetaData.GrandTotal);

                    if (om.PaymentStatus =="pending")
                    {
                        om.PaymentMetaData = UpdatePaymentStatus(om.PaymentMetaData,"paid");
                        om.PaymentStatus = "paid";
                       
                        ordernotes += " ||  Payment received on " + DateTime.Now; 
                    }
                    om.LoyaltyPointsMetaData = loyaltypoints;
                    _productHelper.ItemOtherMetaDataUpdate(ordermetadata.basicModel.ID, "1", "totalcompletedorders");
                }


                if (orderStatus != string.Empty)
                {
                    om.OrderStatus = orderStatus;  // Cart Confirm Cancel  this is for user CArt to confirm only payment is processed

                }
                if (orderprocessstatus != string.Empty)
                {
                    om.OrderProcessStatus = orderprocessstatus;  //Processing Completed  Cancelled-- it should have only 3 status this is for admin
                    om.OrderStatusMetaData = OrderStatusmetadata(orderprocessstatus, DateTime.Now, ordernotes, om.InvoiceNumber, om.OrderStatusMetaData);


                   

                }

                if (itemmetadata != string.Empty)
                {
                    om.ItemMetaData = itemmetadata;
                }

                if (paymentstatus != string.Empty)
                {
                    om.PaymentStatus = paymentstatus;
                }

                if (paymentmetdata != string.Empty)
                {
                    om.PaymentMetaData = paymentmetdata;
                }



                om.UpdateDate = DateTime.Now;



                _dbContext.SaveChanges();


                ///Incase of cancellation create wallet
                ///
                ///in case of cancellation create wallet 
                if (orderprocessstatus== "cancelled" && om.PaymentStatus =="paid")
                {
                    string invoicenumber = GlobalHelper.GetInvoiceNumber(om.BuyerId.ToString(), "wallet");
                    string description = orderprocessstatus + " - " + ordernotes;




                    //ItemMetadata = _orderhelper.ItemOrdermetadata(0, Id, quantity, instruction, invoicenumber, null, VariationMetaData);
                    string itemhelper = ItemMetaData("wallet", "", "", om.OrderId, 1, "", invoicenumber);

                    // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                    var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                   string ItemMetadata = itemMetadataResult.updatedjson;
                   string summarymetadata = itemMetadataResult.updatedsummaryjson;

                   

                    string message = OrderCreation(om.BuyerId, om.SellerId, "wallet", "confirm", "refund", "confirm", invoicenumber, ItemMetadata, om.PaymentMetaData, "received", summarymetadata);



                }

                if (orderprocessstatus == "cancelled" && om.PaymentStatus == "pending")
                {
                    string invoicenumber = GlobalHelper.GetInvoiceNumber(om.BuyerId.ToString(), "wallet");
                    string description = orderprocessstatus + " - " + ordernotes;




                    //ItemMetadata = _orderhelper.ItemOrdermetadata(0, Id, quantity, instruction, invoicenumber, null, VariationMetaData);
                    string itemhelper = ItemMetaData("walletpaymentpending", "", "", om.OrderId, 1, "", invoicenumber);

                    // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                    var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                    string ItemMetadata = itemMetadataResult.updatedjson;
                    string summarymetadata = itemMetadataResult.updatedsummaryjson;



                    string message = OrderCreation(om.BuyerId, om.SellerId, "wallet", "confirm", "refund", "confirm", invoicenumber, ItemMetadata, om.PaymentMetaData, "received", summarymetadata);



                }


            }

            return "success";
        }
        #endregion

        #region CartDelete
        public string CartDelete(int orderid, int buyerid)
        {
            string message = "success";
            try
            {


                OrderMaster om = _dbContext.OrderMasters.FirstOrDefault(u => u.OrderId == orderid && u.BuyerId == buyerid && u.OrderStatus == "cart");

                if (om != null)
                {



                    _dbContext.OrderMasters.Remove(om);
                    _dbContext.SaveChanges();

                    message = "success";
                }
            }
            catch (Exception ex)
            {

                message = "fail:" + ex.Message;
            }
            return message;
        }
        #endregion

        #region QuantityUpdate

        public string CartUpdate(int orderid, int Quantity, string instruction, int buyerid)
        {
            string message = "success";
            try
            {


                OrderMaster om = _dbContext.OrderMasters.FirstOrDefault(u => u.OrderId == orderid && u.BuyerId == buyerid && u.OrderStatus == "cart");

                if (om != null)
                {
                    //ItemMetaData existingMetaData = ParseMetaDataItemDetailViewModel(om.ItemMetaData);
                    string itemhelper = ItemMetaData("", om.ItemMetaData, "quantity", 0, Quantity, "", om.ItemMetaData,"","", om.SummaryMetaData);

                   
                    // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                    var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


              

                    om.ItemMetaData = itemMetadataResult.updatedjson;
                    om.SummaryMetaData = itemMetadataResult.updatedsummaryjson;
                    _dbContext.SaveChanges();
                    message = "success";
                }
            }
            catch (Exception ex)
            {

                message = "fail:" + ex.Message;
            }
            return message;
        }
        #endregion

        #region CouponUpdate
        public string CouponUpdate(string InvoiceNumber, int CouponId, string CouponCode, string instruction, int buyerid,  decimal CouponAmount)
        {
            string message = "success";
            try
            {


                OrderMaster om = _dbContext.OrderMasters.FirstOrDefault(u => u.InvoiceNumber == InvoiceNumber && u.BuyerId == buyerid && u.OrderStatus == "cart");

                if (om != null)
                {
                    //ItemMetaData existingMetaData = ParseMetaDataItemDetailViewModel(om.ItemMetaData);
                    string itemhelper = ItemMetaData("", om.ItemMetaData, "coupon", 0, 0, "", om.ItemMetaData, "", "", om.SummaryMetaData, CouponId, CouponCode, CouponAmount);


                    // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                    var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });




                    om.ItemMetaData = itemMetadataResult.updatedjson;
                    om.SummaryMetaData = itemMetadataResult.updatedsummaryjson;
                    _dbContext.SaveChanges();
                    message = "success";
                }
            }
            catch (Exception ex)
            {

                message = "fail:" + ex.Message;
            }
            return message;
        }
        #endregion





        #region Jsoncreator-PaymentMetaData

        //creator, used it on order creation,
        //then update it once the order is confirm
        //this allows to add wallet, discounts and coupon data with payment model


        public string paymentmetadata(string paymentmethod, string paymentstatus, string paymentreference, string paymentreferencefile, DateTime paymentdate, string payerid, string itemmeta, string paidamount, string paidcurrency, string payeremailid, string invoicenumber, decimal? walletdeduction=0) //itemmeta has paymnetdetail so deserialized it
        {

            #region CreateJsonForComments

            string paymentmetdata = string.Empty;
            string _paymnetstructure = "full";  //full partial installment



            ///deserailzed item meta
            ItemMetaData MetaData = JsonConvert.DeserializeObject<ItemMetaData>(itemmeta);


            //if (MetaData.ConversionAmount > decimal.Parse(paidamount.ToString()))
            //{
            //    _paymnetstructure = "partial";

            //}


            // Create a new instance of CreditCommentViewModel and assign the values
            var metadata = new PaymentMetaDataViewModel
            {
                PaymentMethod = paymentmethod,
                PaymentStatus = paymentstatus,
                PaymentReference = paymentreference,
                PaymentReferenceFile = paymentreferencefile,
                PaymentDate = paymentdate,
                PayerID = payerid,
                ///create payment instance as soon as order is created, then update it on after order confirmation
                ActualAmount = MetaData.paymentModel.ActualAmount,
                ActualCurrency = MetaData.paymentModel.ActualCurrency,
                InvoiceNumber = invoicenumber,
                ConversionAmount = MetaData.paymentModel.ConversionAmount,
                ConversionCurrency = MetaData.paymentModel.ConversionCurrency,

                ///create payment instance as soon as order is created, then update it on after order confirmation

                PaidAmount = decimal.Parse(paidamount.ToString()), ///always show this to user billing
                PaidCurrency = paidcurrency,  ///always show this to user billing



                PaymentStructure = _paymnetstructure,


                PayerEmailID = payeremailid,

                WalletDeduction = (decimal)walletdeduction


            };

            // Serialize the object to JSON
            paymentmetdata = JsonConvert.SerializeObject(metadata);
            #endregion


            return paymentmetdata;
        }


        public string UpdatePaymentStatus(string paymentMetadataJson, string paymentstatus)
        {
            // Deserialize the existing JSON string to PaymentMetaDataViewModel
            PaymentMetaDataViewModel existingMetadata = JsonConvert.DeserializeObject<PaymentMetaDataViewModel>(paymentMetadataJson);

            // Update the PaymentStatus property
            existingMetadata.PaymentStatus = paymentstatus;
            existingMetadata.PaymentDate = DateTime.Now;

            // Serialize the updated object back to JSON
            string updatedPaymentMetadataJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedPaymentMetadataJson;
        }
        #endregion

        #region JsonDeserialized-PaymentMetaData


        public static PaymentMetaDataViewModel ParseMetaDataPaymentMetaData(string json)
        {
            if (json == null)
            {
                return new PaymentMetaDataViewModel(); // Return an empty list
            }

            PaymentMetaDataViewModel parsedData = JsonConvert.DeserializeObject<PaymentMetaDataViewModel>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion

        #region JsonCreatorOrderItem




        //depreciated on 07 dec 2023
        //public string ItemOrdermetadata(int OrderId, int itemid, int quantity, string instruction, string invoicenumber, string existingMetaData, string? VariationMetaData = "")

        //{

        //    ItemDetailMetaData existingMetadata = null;
           
        //    if (!string.IsNullOrEmpty(existingMetaData))
        //    {
        //        existingMetadata = JsonConvert.DeserializeObject<ItemDetailMetaData>(existingMetaData);

        //        // Update existing metadata properties
        //        existingMetadata.Quantity = quantity;
              
        //    }
        //    else
        //    {
        //        var userselectedcurrency = _globalhelper.GetUserCurrency();
        //        var conversionrate = _globalhelper.ConversionRate(userselectedcurrency);
        //        // Get the item details here
        //        ProductViewModel product = _productHelper.productmasterdataV2(0, "idwise", 1, 1, itemid).FirstOrDefault();

        //        string actualcurrency = _globalhelper.GetCurrentCurrency(product.ActualCurrency);



               
        //        ShippingFeesMetaData shippingfeesMetaData = null;
        //        ChargesDetailMetaData existingchargesMetaData = null;
        //        List<VariationDetailMetaData> variationmetadataList = null;


        //        if (VariationMetaData != "")
        //        {
        //            variationmetadataList = JsonConvert.DeserializeObject<List<VariationDetailMetaData>>(VariationMetaData);



        //        }


        //        ///Create shipping fees meta data if exist
        //        ///
        //        ///add shipping charges
        //        ///
        //        if (product.productShippingMetaData != null)
        //        {
        //            if (product.productShippingMetaData.IsFreeShipping == false)
        //            {


        //                shippingfeesMetaData = new ShippingFeesMetaData
        //                {

        //                    ShippingActualAmount = product.productShippingMetaData.ShippingAddOnCharges,


        //                    ShippingConversionAmount = Math.Round((decimal)product.productShippingMetaData.ShippingAddOnCharges * conversionrate, 2),


        //                };

        //            }


        //        }


        //        // Determine the next OrderId (you can adjust this logic if needed)
        //        OrderId = int.Parse(GlobalHelper.RandomNumber().ToString());

        //        // Create a new instance of ItemDetailViewModel
        //        existingMetadata = new ItemDetailMetaData
        //        {
        //            OrderId = OrderId,
        //            ItemID = itemid,
        //            SellerID = product.ProfileId,
        //            Name = product.ProductName,
        //            sellingtype=product.SellingType,
        //            listingtype =product.ListingType,
        //            ItemImage = product.ProductImage,
        //            Quantity = quantity,
        //            Instruction = instruction,
        //            ConversionRate= conversionrate,
        //            ConversionAmount = product.Price,  // Conversion amount (you might want to clarify this)
        //            ConversionCurrency = product.Currency,  // User selected currency (you might want to clarify this)

        //            ActualAmount = product.ActualPrice,
        //            ActualCurrency = actualcurrency,

        //            InvoiceNumber = invoicenumber,

        //            // Coupon
        //            CouponID = "",
        //            CouponName = "",
        //            CouponAmount = 0,
        //            ActualCouponAmount = 0,

        //            variationDetailMetaData = variationmetadataList,
        //            ShippingFeesMetaData= shippingfeesMetaData,
        //            productDigitalMetadata=product.itemdigitalmetadata

        //        };

             
        //        if (existingchargesMetaData != null)
        //        {
        //            ChargesMetaData = JsonConvert.SerializeObject(existingchargesMetaData);
        //        }

        //    }

        //    // Serialize the updated metadata back to JSON
        //    string updatedJson = JsonConvert.SerializeObject(existingMetadata);


        //    return updatedJson;
        //}
       
        
        public string ItemMetaData(string type, string existingMetaData, string updatetype, int itemid, int quantity, string instruction, string invoicenumber, string? VariationMetaData = "", string? ChargesMetaData = "", string? custom = null, int? CouponID=0, string? CouponCode=null, decimal? CouponAmount=0)
        {
            ItemMetaData existingMetadata = null;
            List<ProductDigitalMetaData> productsDitial = new List<ProductDigitalMetaData>();

            string updatedjson = "";
            string loyaltypoints = "";

            string actualcurrency = "";
            string itemname = "";
            string itemurl = "";
            string itemimage = "";
            string sellingtype = "";
            string listingtype = "";
            string description = "";

            decimal conversionrate = 0;
            decimal itemconversionprice = 0;
            decimal itemactualprice = 0;
            string itemcurrency = "";
            decimal variationprice = 0;
            decimal chargesprice = 0;
            decimal salechargesprice = 0;

            int noofcredit = 0;
            bool IsExpiry = false;
            int NoOfExpiryDays = 0;
            DateTime? ExpiryDate = null;



           int recurringperiodindays = 0;


       
            decimal wallet = 0;

            string couponcode = "";
            int couponid = 0;
            decimal coupon = 0;
            decimal discount = 0;
            decimal shippingprice = 0;


           int sellerid = 0;

            if (!string.IsNullOrEmpty(existingMetaData))
            {
                existingMetadata = JsonConvert.DeserializeObject<ItemMetaData>(existingMetaData);
                SummaryModel summaryMetaData = JsonConvert.DeserializeObject<SummaryModel>(custom);

                // Update existing metadata properties

                if (updatetype == "quantity")
                {
                    existingMetadata.basicModel.Quantity = quantity;


                    itemcurrency = summaryMetaData.Currency;
                    itemconversionprice = summaryMetaData.ItemTotal;
                    variationprice = summaryMetaData.VariationTotal;
                    chargesprice = summaryMetaData.ChargesTotal;
                    shippingprice = summaryMetaData.ShippingCost;
                    salechargesprice = summaryMetaData.SaleChargesTotal;



              



                }
                else if(updatetype == "coupon")
                {
                    //existingMetadata.basicModel.Quantity = quantity;
                   couponid = (int)CouponID;
                    coupon = (decimal)CouponAmount;
                    couponcode = CouponCode;



                    quantity = existingMetadata.basicModel.Quantity;
                    itemcurrency = summaryMetaData.Currency;
                    itemconversionprice = summaryMetaData.ItemTotal;
                    variationprice = summaryMetaData.VariationTotal;
                    chargesprice = summaryMetaData.ChargesTotal;
                    shippingprice = summaryMetaData.ShippingCost;
                    salechargesprice = summaryMetaData.SaleChargesTotal;


                }


                updatedjson = JsonConvert.SerializeObject(existingMetadata);
            }
            else
            {
                var userselectedcurrency = _globalhelper.GetUserCurrency();
                conversionrate = _globalhelper.ConversionRate(userselectedcurrency);
                // Get the item details here

                if (type == "item")
                {
                    ProductViewModel product = _productHelper.productmasterdataV2(0, "idwise", 1, 1, itemid).FirstOrDefault();
                    sellerid = product.ProfileId;


                    actualcurrency = _globalhelper.GetCurrentCurrency(product.ActualCurrency);
                    itemname = product.ProductName;
                    description = $"{itemname} purchased on {DateTime.Now}";
                    itemimage = product.ProductImage;
                    itemurl = "/item/"+ product.ProductSeourl;
                    sellingtype = product.SellingType;
                    listingtype = product.ListingType;


                    itemconversionprice = product.Price;
                    itemactualprice = product.ActualPrice;
                    itemcurrency = product.Currency;


                    //shipping meta data 

                    if(product.productShippingMetaData!=null)
                    {
                        shippingprice = product.productShippingMetaData.ShippingAddOnCharges;
                    }


                    if(product.itemdigitalmetadata !=null)
                    {
                        productsDitial = product.itemdigitalmetadata;
                    }
                }
                else if (type == "credit")
                {

                    if (itemid == 0) 
                    {
                        sellerid = 0; //bcs this is seller by admin
                        string[] customValues = custom.Split(',');

                        itemname = customValues[0].Trim();
                        description = $"{itemname} purchased on {DateTime.Now}";
                        noofcredit = int.Parse(customValues[1].Trim());
                        itemactualprice  = decimal.Parse(customValues[2].Trim());
                        itemimage = "~/wwwroot/images/freecredit.png";
                        sellingtype = "sell";
                        listingtype = "credit";






                        actualcurrency = _globalhelper.GetCurrentCurrency(0);
                        itemconversionprice = _globalhelper.GetCurrencyConversion(actualcurrency, userselectedcurrency, itemactualprice.ToString());
                        itemcurrency = userselectedcurrency;

                    }


                    else
                    {
                        RevenueCreditPackage package = _dbContext.RevenueCreditPackage.FirstOrDefault(u => u.RevenueCreditID == itemid);

                        if (package == null)
                        {
                            return "fail - Package does not exist ID: " + itemid;
                        }
                        itemname = package.RevenueCreditName  + "- Number of credits " + package.NoofCredit;
                        description = $"{itemname} purchased on {DateTime.Now}";
                        itemimage = package.CreditImage;
                        sellingtype = "sell";
                        listingtype = "credit";


                        noofcredit = package.NoofCredit;





                        itemactualprice = package.CreditAmount;
                        actualcurrency = _globalhelper.GetCurrentCurrency(0);
                        itemconversionprice = _globalhelper.GetCurrencyConversion(actualcurrency, userselectedcurrency, itemactualprice.ToString());
                        itemcurrency = userselectedcurrency;


                        IsExpiry = package.IsExpiry;


                        if (package.IsExpiry == true)
                        {
                            NoOfExpiryDays = int.Parse(package.NoofExpiryDays.ToString());

                            ExpiryDate = IsExpiry ? DateTime.Now.AddDays(NoOfExpiryDays) : (DateTime?)null;
                        }
                        
                       

                    }




                }
                else if(type == "creditused")
                {
                    sellerid = 0; //bcs this is seller by admin
                    string[] customValues = custom.Split('#');

                    itemname = customValues[0].Trim();
                    description = $"{customValues[0].Trim()} between {customValues[4].Trim()}";
                    noofcredit = -int.Parse(customValues[1].Trim());
                    itemactualprice = decimal.Parse(customValues[2].Trim());
                    itemimage = "~/wwwroot/images/creditused.png";
                    sellingtype = "";
                    listingtype = "";
                    itemurl = customValues[3].Trim();





                    actualcurrency = _globalhelper.GetCurrentCurrency(0);
                    itemconversionprice = _globalhelper.GetCurrencyConversion(actualcurrency, userselectedcurrency, itemactualprice.ToString());
                    itemcurrency = userselectedcurrency;
                }
                else if(type=="subscription")
                {

                    if (itemid == 0)
                    {
                        sellerid = 0; //bcs this is seller by admin
                        string[] customValues = custom.Split(',');

                        itemname = customValues[0].Trim();
                        description = $"{itemname} purchased on {DateTime.Now}";
                        itemimage = "~/wwwroot/images/freecredit.png";
                        sellingtype = "";
                        listingtype = "";



                        itemactualprice = decimal.Parse(customValues[1].Trim());
                        actualcurrency = _globalhelper.GetCurrentCurrency(0);
                        itemconversionprice = _globalhelper.GetCurrencyConversion(actualcurrency, userselectedcurrency, itemactualprice.ToString());
                        itemcurrency = userselectedcurrency;




                        ExpiryDate = DateTime.Now.AddDays(int.Parse(customValues[2].Trim()));

                        recurringperiodindays = int.Parse(customValues[2].Trim());















                    }
                    else
                    {


                        RevenueSubscriptionPackage package = _dbContext.RevenueSubscriptionPackage.FirstOrDefault(u => u.RevenueSubscriptionPackageID == itemid);

                        if (package == null)
                        {
                            return "fail - Package does not exist ID: " + itemid;
                        }
                        if (package == null)
                        {
                            return "fail - Package does not exist ID: " + itemid;
                        }
                        itemname = package.RevenuePackageName;
                        description = $"{itemname} purchased on {DateTime.Now}";
                        itemimage = package.SubscriptionImage;
                        sellingtype = "sell";
                        listingtype = "subscription";

                        itemactualprice = package.CreditAmount;

                        actualcurrency = _globalhelper.GetCurrentCurrency(0);




                        itemconversionprice = _globalhelper.GetCurrencyConversion(actualcurrency, userselectedcurrency, itemactualprice.ToString());
                        itemcurrency = userselectedcurrency;

                        ExpiryDate = DateTime.Now.AddDays(package.RecurringPeriodInDays);

                        recurringperiodindays = package.RecurringPeriodInDays;


                    }
                    

                }

                else if (type == "wallet")
                {
                    OrderMaster om = _dbContext.OrderMasters.FirstOrDefault(u=>u.OrderId ==itemid);

                    if (om!=null)
                    {
                        SummaryModel summaryMetaData = JsonConvert.DeserializeObject<SummaryModel>(om.SummaryMetaData);
                        ItemMetaData walletItemDetails = JsonConvert.DeserializeObject<ItemMetaData>(om.ItemMetaData);

                        sellerid = 0;


                      
                        itemname = $"Refund for item {walletItemDetails.basicModel.Name}.";
                        description = $"Invoice number {om.InvoiceNumber}.";
                        itemimage = "~/wwwroot/images/wallet.png";
                        itemurl = "/user/billing";
                        sellingtype = "";
                        listingtype = "";

                        itemactualprice = walletItemDetails.paymentModel.ActualAmount;
                        itemconversionprice = walletItemDetails.paymentModel.ConversionAmount;

                        actualcurrency = walletItemDetails.paymentModel.ActualCurrency;
                        itemcurrency = walletItemDetails.paymentModel.ConversionCurrency;

                        quantity = 1;


                        if (walletItemDetails.variationModel != null)
                        {
                            VariationMetaData = JsonConvert.SerializeObject(walletItemDetails.variationModel);
                        }

                        if(walletItemDetails.chargesModel != null)
                        {
                          
                            ChargesMetaData = JsonConvert.SerializeObject(walletItemDetails.chargesModel);
                        }

                        //shipping meta data 

                        if (summaryMetaData.ShippingCost != null)
                        {
                            shippingprice = summaryMetaData.ShippingCost;
                        }


                    }
                   


                }
                else if (type == "walletpaymentpending")
                {
                    OrderMaster om = _dbContext.OrderMasters.FirstOrDefault(u => u.OrderId == itemid);

                    if (om != null)
                    {
                        SummaryModel summaryMetaData = JsonConvert.DeserializeObject<SummaryModel>(om.SummaryMetaData);
                        ItemMetaData walletItemDetails = JsonConvert.DeserializeObject<ItemMetaData>(om.ItemMetaData);

                        sellerid = 0;



                        itemname = $"Refund for item {walletItemDetails.basicModel.Name}.";
                        description = $"Invoice number {om.InvoiceNumber}.";
                        itemimage = "~/wwwroot/images/wallet.png";
                        itemurl = "/user/billing";
                        sellingtype = "";
                        listingtype = "";
                        itemactualprice = Math.Abs(summaryMetaData.WalletAmount);
                        itemconversionprice = Math.Abs(summaryMetaData.WalletAmount);

                        actualcurrency = walletItemDetails.paymentModel.ActualCurrency;
                        itemcurrency = walletItemDetails.paymentModel.ConversionCurrency;

                        quantity = 1;


                        if (walletItemDetails.variationModel != null)
                        {
                            VariationMetaData = JsonConvert.SerializeObject(walletItemDetails.variationModel);
                        }

                        if (walletItemDetails.chargesModel != null)
                        {

                            ChargesMetaData = JsonConvert.SerializeObject(walletItemDetails.chargesModel);
                        }

                        //shipping meta data 

                        if (summaryMetaData.ShippingCost != null)
                        {
                            shippingprice = summaryMetaData.ShippingCost;
                        }


                    }



                }

                else if (type == "walletused")
                {
                   
                      
                        sellerid = 0;

                        string[] customValues = custom.Split(',');

                        itemname = $"Wallet used";
                        description = instruction;
                        itemimage = "~/wwwroot/images/walletused.png";
                        itemurl = "/user/billing";
                        sellingtype = "";
                        listingtype = "";

                        actualcurrency = customValues[0].Trim();
                        itemactualprice = decimal.Parse(customValues[1].Trim());



                        itemconversionprice =decimal.Parse(customValues[2].Trim());
                        itemcurrency = customValues[3].Trim();

                        quantity = 1;


                      





                }
                else if (type == "profileboost")
                {
                    sellerid = 0; //bcs this is seller by admin
                    string[] customValues = custom.Split('#');

                    itemname = customValues[0].Trim();
                    description = $"{customValues[0].Trim()} used on {DateTime.Now}";
                    noofcredit = -int.Parse(customValues[1].Trim());
                    itemactualprice = decimal.Parse(customValues[2].Trim());
                    itemimage = customValues[4].Trim();
                    sellingtype = "";
                    listingtype = "";
                    itemurl = customValues[3].Trim();





                    actualcurrency = _globalhelper.GetCurrentCurrency(0);
                    itemconversionprice = _globalhelper.GetCurrencyConversion(actualcurrency, userselectedcurrency, itemactualprice.ToString());
                    itemcurrency = userselectedcurrency;
                }

                else if (type == "itemboost")
                {
                    sellerid = 0; //bcs this is seller by admin
                    string[] customValues = custom.Split('#');

                    itemname = customValues[0].Trim();
                    description = $"{customValues[0].Trim()} used on {DateTime.Now}";
                    noofcredit = -int.Parse(customValues[1].Trim());
                    itemactualprice = decimal.Parse(customValues[2].Trim());
                    itemimage = customValues[4].Trim();
                    sellingtype = "";
                    listingtype = "";
                    itemurl = customValues[3].Trim();





                    actualcurrency = _globalhelper.GetCurrentCurrency(0);
                    itemconversionprice = _globalhelper.GetCurrencyConversion(actualcurrency, userselectedcurrency, itemactualprice.ToString());
                    itemcurrency = userselectedcurrency;
                }

                ItemBasicModel basicModel = new ItemBasicModel
                {
                    ID= itemid,
                    Name= itemname,
                    ItemImage = itemimage,
                    Quantity =quantity,

                    Description= description,
                    ItemURL= itemurl,


                    SellingType =sellingtype,
                    ListingType = listingtype,

                    InvoiceNumber=invoicenumber,

                    
                };

                ItemPaymentModel paymentModel = new ItemPaymentModel
                {
                    ConversionRate = conversionrate,

                    ConversionAmount = itemconversionprice,
                    ConversionCurrency = itemcurrency,


                    ActualAmount = itemactualprice,
                    ActualCurrency = actualcurrency,



                 

                };


                
                List<ItemVariationModel> variationList = new List<ItemVariationModel>();

                if (VariationMetaData != null)
                {

                    List<ItemVariationModel> attributeOptions = JsonConvert.DeserializeObject<List<ItemVariationModel>>(VariationMetaData);
                    //List<AttributeOptionViewModel> attributeOptions = JsonConvert.DeserializeObject<List<AttributeOptionViewModel>>(VariationMetaData);

                    if (attributeOptions != null && attributeOptions.Any())
                    {
                        foreach (var attributeOption in attributeOptions)
                        {
                            var variation = new ItemVariationModel
                            {
                                // Assuming 'OptionText' is mapped to 'VariationName'
                                VariationName = attributeOption.VariationName,

                                // Mapping from AttributeOptionViewModel to ItemPaymentModel
                                ConversionRate = conversionrate, // Set your conversion rate accordingly
                                ConversionAmount = attributeOption.ConversionAmount,
                                ConversionCurrency = itemcurrency, // Set your conversion currency accordingly
                                ActualAmount = attributeOption.ActualAmount,
                                ActualCurrency = actualcurrency // Set your actual currency accordingly
                            };

                            variationList.Add(variation);
                        }
                    }

                    variationprice = variationList.Sum(variation => variation.ConversionAmount);
                }
                


                ///this is addon for buyer
                List<ItemChargesModel> chargesList = chargelistcreator(itemcurrency, itemconversionprice, variationprice);

                //chargesList = JsonConvert.DeserializeObject<List<ItemChargesModel>>(chargesList);
                chargesprice = chargesList.Sum(u => u.ConversionAmount);

                //PlatformFee
                //Tax by admin
                //Vat by admin

                ///Category specific commission
                ///Item listing specific commission
                ///seller speicific commission


                //Sales Commission
                //Buyer Commission

                ///this is addon for buyer
                List<ItemSalesChargesModel> saleschargesList = saleschargelistcreator(itemcurrency, itemconversionprice, variationprice);

                //chargesList = JsonConvert.DeserializeObject<List<ItemChargesModel>>(chargesList);
                salechargesprice = saleschargesList.Sum(u => u.ConversionAmount);



                ItemCreditModel creditmodel = null;
                if (type == "credit" || type == "creditused" || type=="profileboost" || type=="itemboost")
                {
                    creditmodel = new ItemCreditModel
                    {
                        NoOfCredit = noofcredit,
                        IsExpiry = IsExpiry,
                        NoOfExpiryDays = NoOfExpiryDays,
                        ExpiryDate = ExpiryDate
                    };
                }

                ItemSubscriptionModel subscriptionmodel = null;
                if (type == "subscription")
                {
                     subscriptionmodel = new ItemSubscriptionModel
                    {
                        RecurringPeriodInDays = recurringperiodindays,
                        ExpiryDate =  (DateTime)ExpiryDate 
                     };

                }
                ItemMetaData itemmetadata = new ItemMetaData
                {
                    basicModel = basicModel,
                    paymentModel = paymentModel,
                    variationModel = variationList,
                    chargesModel = chargesList,
                    saleschargesModel=saleschargesList,
                    creditModel = creditmodel,
                    subscriptionModel = subscriptionmodel,
                    productDigitalMetaData = productsDitial
                };
                updatedjson = JsonConvert.SerializeObject(itemmetadata);


                ///loyalty points creation
                ///
                

            }


            string updatedsummaryjson = summaryModelMetaData(itemcurrency, itemconversionprice, quantity,
               variationprice, chargesprice, shippingprice, wallet, coupon, discount, salechargesprice, couponid, couponcode);



            



            // Serialize the updated metadata back to JSON
            var result = new { updatedjson, updatedsummaryjson, sellerid };

            // Return the serialized JSON
            return JsonConvert.SerializeObject(result);


            
        }
        
        
        #endregion

        public string summaryModelMetaData(string itemcurrency, decimal itemconversionprice, int quantity, decimal variationprice, decimal chargesprice, decimal shippingprice, decimal wallet, decimal coupon, decimal discount, decimal saleschargesprice, int? coupnid=0, string couponcode=null)
        {


            //var userselectedcurrency = _globalhelper.GetUserCurrency();
            var conversionrate = _globalhelper.ConversionRate(itemcurrency);
            SummaryModel summarymodel = new SummaryModel
            {


                Currency = itemcurrency,
                ItemTotal = itemconversionprice,


                ItemQtyTotal = itemconversionprice * quantity,


                VariationTotal = variationprice,
                VariationQtyTotal = variationprice * quantity,


                ChargesTotal = chargesprice,
                ChargesQtyTotal = chargesprice * quantity,


                SaleChargesTotal = saleschargesprice,
                SaleChargesQtyTotal = saleschargesprice * quantity,


                ShippingCost = shippingprice,
                ShippingQtyCost = shippingprice * quantity,


                WalletAmount = wallet,
                DiscountAmount = discount,

                CouponId = coupnid,
                CouponAmount = coupon,
                CouponCode = couponcode,

                ///ItemTotal + VariationTotal+ ChargesTotal+ ShippingCost  -wallet -discount-coupon
                Total = (itemconversionprice + variationprice),
                ///ItemTotal + VariationTotal+ ChargesTotal+ ShippingCost  -wallet -discount-coupon
                TotalQty = (itemconversionprice * quantity) + (variationprice * quantity),

                Commission = 0,

                ///this is the column used in all reports
                GrandTotal =
                (itemconversionprice * quantity) + (variationprice * quantity) + (chargesprice * quantity) + (shippingprice * quantity),
               

                ///Set base currency data for admin
                BaseCurrency = _globalhelper.BaseCurrency(),
                ConversionRate = conversionrate,


                BaseGrandTotal = ((itemconversionprice * quantity) + (variationprice * quantity) + (chargesprice * quantity) + (shippingprice * quantity)) / conversionrate,

                

            };


            string updatedsummaryjson = JsonConvert.SerializeObject(summarymodel);

            return updatedsummaryjson;
        }

        #region JsonCreator- ChargeList
        public List<ItemChargesModel> chargelistcreator(string itemcurrency, decimal itemconversionprice, decimal variationprice)
        {
            decimal conversionrate = 0;

            var basecurrency = _globalhelper.BaseCurrency();
            var userselectedcurrency = _globalhelper.GetUserCurrency();
            conversionrate = _globalhelper.ConversionRate(userselectedcurrency);

            decimal total = (itemconversionprice + variationprice);
            decimal actualamount = (itemconversionprice + variationprice) / conversionrate;

            decimal chargeamountActual = 0;
            decimal chargeamountConversion = 0;
            List<ItemChargesModel> chargelist = new List<ItemChargesModel>();

            //// Buyer Fees setting
            var commissionBuyerSettingsJson = _websettinghelper.GetWebsettingJson("CommissionBuyerSettings");

            if (!string.IsNullOrEmpty(commissionBuyerSettingsJson))
            {
                List<CommissionTaxBuyerSettingsModel> CommissionBuyerSettingsList = JsonConvert.DeserializeObject<List<CommissionTaxBuyerSettingsModel>>(commissionBuyerSettingsJson);

                // Assuming jsonPlatform is defined somewhere in your code
                // Replace jsonPlatform with the correct variable

                foreach (var jsonPlatform in CommissionBuyerSettingsList)
                {
                   
                    if (jsonPlatform.CommissionType == "Amount")
                    {
                        chargeamountActual = jsonPlatform.Amount;//this is actual 
                        chargeamountConversion = jsonPlatform.Amount * conversionrate;//this is after conversion
                    }
                    else if (jsonPlatform.CommissionType == "Percentage")
                    {
                        chargeamountActual = (actualamount * jsonPlatform.Amount) / 100;
                        chargeamountConversion = (total * jsonPlatform.Amount) / 100;
                    }

                    // Add commission settings to chargelist
                    chargelist.Add(new ItemChargesModel
                    {
                        // Include other properties from your ItemChargesModel
                        ChargesName = jsonPlatform.Label,
                        ConversionRate = 1,
                        ConversionCurrency = itemcurrency,
                        ActualCurrency = basecurrency,
                        ActualAmount = chargeamountActual,
                        ConversionAmount = chargeamountConversion ,
                    });
                }
            }

            //// Buyer List setting

            return chargelist;
        }

        #endregion

        #region JsonCreator- SaleChargesList
        public List<ItemSalesChargesModel> saleschargelistcreator(string itemcurrency, decimal itemconversionprice, decimal variationprice)
        {
            decimal conversionrate = 0;

            var basecurrency = _globalhelper.BaseCurrency();
            var userselectedcurrency = _globalhelper.GetUserCurrency();
            conversionrate = _globalhelper.ConversionRate(userselectedcurrency);

            decimal total = (itemconversionprice + variationprice);
            decimal actualamount = (itemconversionprice + variationprice) / conversionrate;

            decimal chargeamountActual = 0;
            decimal chargeamountConversion = 0;
            List<ItemSalesChargesModel> saleschargelist = new List<ItemSalesChargesModel>();

            //// Buyer Fees setting
            var commissionSellerSettingsJson = _websettinghelper.GetWebsettingJson("CommissionSellerSettings");

            if (!string.IsNullOrEmpty(commissionSellerSettingsJson))
            {
                List<CommissionTaxSellerSettingsModel> CommissionSellerSettingsList = JsonConvert.DeserializeObject<List<CommissionTaxSellerSettingsModel>>(commissionSellerSettingsJson);

                // Assuming jsonPlatform is defined somewhere in your code
                // Replace jsonPlatform with the correct variable

                foreach (var jsonPlatform in CommissionSellerSettingsList)
                {

                    if (jsonPlatform.CommissionType == "Amount")
                    {
                        chargeamountActual = jsonPlatform.Amount;//this is actual 
                        chargeamountConversion = jsonPlatform.Amount * conversionrate;//this is after conversion
                    }
                    else if (jsonPlatform.CommissionType == "Percentage")
                    {
                        chargeamountActual = (actualamount * jsonPlatform.Amount) / 100;
                        chargeamountConversion = (total * jsonPlatform.Amount) / 100;
                    }

                    // Add commission settings to chargelist
                    saleschargelist.Add(new ItemSalesChargesModel
                    {
                        // Include other properties from your ItemChargesModel
                        SalesChargeName = jsonPlatform.Label,
                        ConversionRate = 1,
                        ConversionCurrency = itemcurrency,
                        ActualCurrency = basecurrency,
                        ActualAmount = chargeamountActual,
                        ConversionAmount = chargeamountConversion,
                    });
                }
            }

            //// Buyer List setting

            return saleschargelist;
        }

        #endregion


        #region JsonDeserializedOrderITem


        //public ItemDetailMetaData ParseMetaDataItemDetailViewModel(string json)
        //{
        //    if (json == null)
        //    {
        //        return new ItemDetailMetaData(); // Return an empty list
        //    }

        //    ItemDetailMetaData parsedData = JsonConvert.DeserializeObject<ItemDetailMetaData>(json);

        //    // Return the parsed list
        //    return parsedData;

        //}
        public ItemMetaData ParseMetaDataItemDetailViewModel(string json)
        {
            if (json == null)
            {
                return new ItemMetaData(); // Return an empty list
            }

            ItemMetaData parsedData = JsonConvert.DeserializeObject<ItemMetaData>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion


        #region JsonCreator-ShippingMetadata
        public string Shippingmetadata(Guid CustomerAddressId)  ///just send id and create the shipping metada
        {

            #region CreateJsonForComments

            CustomerAddress cd = _dbContext.CustomerAddresses.FirstOrDefault(u => u.CustomerAddressGuid == CustomerAddressId);

            string shippingmetdata = string.Empty;






            // Create a new instance of CreditCommentViewModel and assign the values
            // Create a new instance of ShippingDetailMetaData and assign the values
            var metadata = new ShippingDetailMetaData
            {
                FullName = cd.FirstName + " " + cd.LastName,
                ShippingAddress = $"{(!string.IsNullOrEmpty(cd.Street) ? $"Street Number {cd.Street} " : "")}" +
                  $"{cd.Address} " +
                  $"{cd.City} " +
                  $"{cd.State} " +
                  $"{(!string.IsNullOrEmpty(cd.Zipcode) ? $"Zip Code {cd.Zipcode} " : "")}" +
                  $"{cd.Country}",
                ShippingEmail = cd.Email,
                ShippingPhone = cd.Phone,
                ShippingLatitude = cd.Latitude,
                ShippingLongitude = cd.Longitude,
                AddressType =cd.AddressType,
                NearestLandMark=cd.HouseNumber

            };

            // Serialize the object to JSON
            shippingmetdata = JsonConvert.SerializeObject(metadata);
            #endregion


            return shippingmetdata;
        }
        #endregion
        #region JsonDeserialized-Shipping


        public ShippingDetailMetaData ParseMetaDataShippingMetaData(string json)
        {
            if (json == null)
            {
                return new ShippingDetailMetaData(); // Return an empty list
            }

            ShippingDetailMetaData parsedData = JsonConvert.DeserializeObject<ShippingDetailMetaData>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion

        #region JsonDeserializedOrderSummary


        //public SummaryOrderMetaData ParseMetaDataOrderSummary(string json, string chargesmetadata)
        //{
        //    if (json == null)
        //    {
        //        return new SummaryOrderMetaData(); // Return an empty model
        //    }

        //    var jsonObject = JObject.Parse(json); // Parse the JSON string
        //    var jsonObjectCharges = string.IsNullOrEmpty(chargesmetadata) ? null : JObject.Parse(chargesmetadata); // Parse the JSON string or null


        //    int qty = jsonObject["Quantity"] != null ? (int)jsonObject["Quantity"] : 1;
        //    decimal itemTotal = jsonObject["ConversionAmount"] != null ? (decimal)jsonObject["ConversionAmount"]:0; /// A. Item Price * Qty
        //    decimal VariationTotal = 0; //B. variation price  * qty
        //    decimal ChargesTotal = 0; ///C. charges * qty
        //    decimal ShippingCost = 0; //D. shipping * qty
        //    decimal CouponTotal = 0; ///E. A+B -C
        //    decimal Wallet = 0; ///
        //    decimal Total = 0;//A+B+C+D-E

        //    if (jsonObject["variationDetailMetaData"] is JArray variations)
        //    {
        //        foreach (var variation in variations)
        //        {
        //            var conversionVariationAmount = (decimal)variation["ConversionVariationAmount"];
        //            VariationTotal += conversionVariationAmount;
        //        }
        //    }

        //    //VariationTotal = VariationTotal * qty; 


        //    if (jsonObject["ShippingFeesMetaData"] is JObject shippingFeesMetaData)
        //    {
        //        var shippingConversionAmount = (decimal)shippingFeesMetaData["ShippingConversionAmount"];

        //        ShippingCost = shippingConversionAmount; // Calculate ShippingTotal based on quantity
        //    }
        //    var parsedData = new SummaryOrderMetaData
        //    {
        //        Currency = (string)jsonObject["ConversionCurrency"],

        //        ItemTotal = itemTotal,     /// A. Item Price
        //        ItemQtyTotal=itemTotal*qty,
        //        VariationTotal = VariationTotal,//B. variation price
        //        VariationQtyTotal   = VariationTotal*qty,
        //         ChargesTotal = ChargesTotal,///C. charges * qty
        //         ChargesQtyTotal=ChargesTotal*qty,
        //        ShippingCost = ShippingCost, /////D. shipping * qty
        //        ShippingQtyCost =ShippingCost *qty,
        //        CouponTotal = CouponTotal,                    //////E. A+B -C
        //        Wallet= Wallet,
        //        TotalQty = (itemTotal * qty) + (VariationTotal * qty) + (ChargesTotal * qty) + (ShippingCost * qty) - CouponTotal-Wallet, 
        //        Total = itemTotal + VariationTotal+ ChargesTotal + ShippingCost  - CouponTotal - Wallet //A+B+C+D-E




        //    };

        //    return parsedData;
        //}

        public SummaryModel ParseMetaDataOrderSummary(string json)
        {
            if (json == null)
            {
                return new SummaryModel(); // Return an empty list
            }

            SummaryModel parsedData = JsonConvert.DeserializeObject<SummaryModel>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion



        #region JsonCreator-OrderStatusMetaData
        public string OrderStatusmetadata(string orderstatusname, DateTime orderstatusdate, string orderstatusnotes, string invoicenumber, string existingMetaData)
        {
            // Deserialize the existing metadata JSON string into a list of OrderStatusMetaData



            List<OrderStatusMetaData> existingMetadata = JsonConvert.DeserializeObject<List<OrderStatusMetaData>>(existingMetaData ?? "[]");


            //if (existingMetadata == null)
            //{
            //    existingMetadata = new List<OrderStatusMetaData>();
            //}

            int nextOrderStatusId = 1;

            if (existingMetadata.Count > 0)
            {
                // Get the highest existing OrderStatusID and increment it
                nextOrderStatusId = existingMetadata.Max(meta => meta.OrderStatusID) + 1;
            }

            // Create a new instance of OrderStatusMetaData
            var newMetadata = new OrderStatusMetaData
            {
                OrderStatusID = nextOrderStatusId,
                OrderStatusName = orderstatusname,
                OrderStatusDate = orderstatusdate,
                OrderStatusNotes = orderstatusnotes,
                InvoiceNumber = invoicenumber
            };

            // Add the new metadata to the existing list
            existingMetadata.Add(newMetadata);




            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }
        #endregion

        #region JsonCreator- OrderLoyaltypoins
        public string OrderLoyaltypointsCreation(string existingMetaData, string loyaltypoinstype, decimal orderamount)
        {
            LoyaltyPointSettingsModel loyalpoints = new LoyaltyPointSettingsModel();
            var _loyaltpointSettings = _websettinghelper.GetWebsettingJson("LoyaltyPointSettings");
            if (_loyaltpointSettings != null && !string.IsNullOrEmpty(_loyaltpointSettings))
            {
                var json = JsonConvert.DeserializeObject<LoyaltyPointSettingsModel>(_loyaltpointSettings);

                if (json != null)
                {
                    loyalpoints = new LoyaltyPointSettingsModel
                    {
                        PointsConversionRate = json.PointsConversionRate,
                        MinPointsRedeem = json.MinPointsRedeem,
                        PointsExpiry = json.PointsExpiry,
                        IsEnable = json.IsEnable
                    };
                }
            }

            decimal TotalLoyaltypoints = loyalpoints.PointsConversionRate * orderamount;

            List<LoyaltyPointsViewModel> existingMetadata;
            if (string.IsNullOrEmpty(existingMetaData))
            {
                existingMetadata = new List<LoyaltyPointsViewModel>();
            }
            else
            {
                existingMetadata = JsonConvert.DeserializeObject<List<LoyaltyPointsViewModel>>(existingMetaData);
            }

            var newMetadata = new LoyaltyPointsViewModel
            {
                Points = TotalLoyaltypoints,
                ExpirtyDate = DateTime.Now.AddDays(loyalpoints.PointsExpiry),
                PointsType = loyaltypoinstype
            };

            // Add the new metadata to the existing list
            existingMetadata.Add(newMetadata);

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }
        #endregion

        #region LoyaltyPoints- Deserialized
        public List<LoyaltyPointsViewModel> ParseMetaDataLoyaltypoints(string json)
        {
            if (json == null)
            {
                return new List<LoyaltyPointsViewModel>(); // Return an empty list
            }

            List<LoyaltyPointsViewModel> parsedData = JsonConvert.DeserializeObject<List<LoyaltyPointsViewModel>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion

        #region OrderStatusMetaData-deserialized
        public List<OrderStatusMetaData> ParseMetaDataOrderStatus(string json)
        {
            if (json == null)
            {
                return new List<OrderStatusMetaData>(); // Return an empty list
            }

            List<OrderStatusMetaData> parsedData = JsonConvert.DeserializeObject<List<OrderStatusMetaData>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion

        #region OrderStatus-Setup-MetaData-deserialized

        public List<OrderStatusSetupViewModel> OrderStatusSetup(string key)
        {
            var settingsData = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "OrderStatus");

            if (settingsData == null)
            {
                return new List<OrderStatusSetupViewModel>();
            }

            // Deserialize the JSON data into a Dictionary
            var settingsDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<OrderStatusSetupViewModel>>>(settingsData.WebsettingValue);

            if (settingsDictionary.ContainsKey(key))
            {
                // Retrieve the list associated with the provided key
                return settingsDictionary[key];
            }
            else
            {
                // If the key is not found, return an empty list
                return new List<OrderStatusSetupViewModel>();
            }
        }

        #endregion



        #region ChargesMetaData- Deserialized
        public static ChargesDetailMetaData ParseMetaDataChargesMetaData(string json)
        {
            if (json == null)
            {
                return new ChargesDetailMetaData();
            }

            ChargesDetailMetaData parsedData = JsonConvert.DeserializeObject<ChargesDetailMetaData>(json);
            return parsedData;
        }
        #endregion

    


        #region AllOrders
        public List<OrderViewModel> GetOrdersItem(string? datatype="", int? profileid=0, string? custom="")
        {
            //var dateformat = _globalhelper.Dateformat();
            var q = _dbContext.OrderMasters.Where(u=>u.IsDeleted==false).ToList();
            var itemJsonList = q.ToList().Select(item => new OrderViewModel
            {
                OrderId = item.OrderId,
                BuyerID = item.BuyerId,
                SellerID = item.SellerId,
                OrderStatus = item.OrderStatus,
                OrderProcessStatus = item.OrderProcessStatus,
                ItemType = item.ItemType,
                VendorNotes = item.VendorNotes,
                AdminNotes = item.AdminNotes,
                OrderDateDT = item.OrderDate,
                OrderDate = item.OrderDate.ToString(),
                UpdateDate = item.UpdateDate?.ToString(),
                ItemDetailMetaData = item.ItemMetaData != null ? ParseMetaDataItemDetailViewModel(item.ItemMetaData) : null,
                SellerViewModel = _userhelper.SellerList().FirstOrDefault(u => u.ProfileId == item.SellerId),
                BuyerViewModel = _userhelper.ClientList().FirstOrDefault(u => u.ProfileId == item.BuyerId),
                SummaryOrderMetaData = ParseMetaDataOrderSummary(item.SummaryMetaData),
                InvoiceNumber = item.InvoiceNumber,
                ShippingDetailMetaData = item.ShippingMetaData != null ? ParseMetaDataShippingMetaData(item.ShippingMetaData) : null,
                PaymentMetaData = item.PaymentMetaData != null ? ParseMetaDataPaymentMetaData(item.PaymentMetaData) : null,
                OrderStatusMetaData = item.OrderStatusMetaData != null ? ParseMetaDataOrderStatus(item.OrderStatusMetaData) : null,
                
                ReviewMetaData = item.ReviewMetaData != null ? ParseMetaDataReviewList(item.ReviewMetaData) : null,

                ChargesDetailMetaData= item.OrderChargesMetaData!=null ? ParseMetaDataChargesMetaData(item.OrderChargesMetaData) : null,
                TransactionType =item.TransactionType,

                IncomingOutgoing= DetermineIncomingOutgoing(item.TransactionType, (int)profileid, item.BuyerId, item.SellerId),
                PaymentStatus =item.PaymentStatus,
                LoyaltyPointMetaData= ParseMetaDataLoyaltypoints(item.LoyaltyPointsMetaData)



            });

            if(datatype=="cart")
            {
               
                itemJsonList = itemJsonList.Where(u => u.BuyerID == profileid && u.ItemType =="item" && u.OrderStatus=="cart");
            }

            if (datatype == "shipping")
            {

                itemJsonList = itemJsonList.Where(u => u.InvoiceNumber == custom && u.ItemType == "item" && u.OrderStatus == "cart");
            }
            else if(datatype =="wallet")
            {
                itemJsonList = itemJsonList.Where(u => u.BuyerID == profileid && u.ItemType == "wallet");
            }
            else if (datatype == "invoicenumber")
            {
                itemJsonList = itemJsonList.Where(u => u.InvoiceNumber == custom
                && u.OrderStatus == "confirm" && u.TransactionType == "purchased"



                );
            }

            else if (datatype == "transactionhistorybuyer")
            {
                itemJsonList = itemJsonList
     .Where(u => u.BuyerID == profileid &&
         ((u.TransactionType == "purchased" && u.OrderStatus == "confirm" && u.PaymentStatus == "paid") ||
         (u.TransactionType == "refund" && u.OrderStatus == "confirm" && u.PaymentStatus == "received"))
          //(u.TransactionType == "used" && u.OrderStatus == "confirm" && u.PaymentStatus == "used")


         );
    

            }

            else if (datatype == "transactionhistoryseller")
            {
                itemJsonList = itemJsonList
    .Where(u => (u.SellerID == profileid || u.BuyerID == profileid) &&
                ((u.TransactionType == "purchased" && u.OrderStatus == "confirm" && u.PaymentStatus == "paid") ||
                 (u.TransactionType == "refund" && u.OrderStatus == "confirm" && u.PaymentStatus == "received"))
   
         
         
         );


            }

            else if (datatype == "boost")
            {
                itemJsonList = itemJsonList
     .Where(u => 
         ((u.ItemType == "profileboost" || u.ItemType =="itemboost") &&
         u.TransactionType == "purchased" && u.OrderStatus == "confirm" && u.PaymentStatus == "paid") 


         );


            }

            return itemJsonList.ToList();


        }

        private static string DetermineIncomingOutgoing(string transactionType, int profileid, int buyerid, int sellerid)
        {

            if(profileid==buyerid)
            {
                if (transactionType == "purchased")
                {
                    return "outgoing";
                }
                else if (transactionType == "withdrawal")
                {
                    return "outgoing";
                }

                else if (transactionType == "used")
                {
                    return "outgoing";
                }

                else if (transactionType == "used")
                {
                    return "outgoing";
                }


                else if (transactionType == "refund")
                {
                    return "incoming";
                }
                else if (transactionType == "topup")
                {
                    return "incoming";
                }

            }
            else if(profileid ==sellerid)
            {
                if (transactionType == "purchased")
                {
                    return "incoming";
                }
                else if (transactionType == "withdrawal")
                {
                    return "outgoing";
                }

                else if (transactionType == "used")
                {
                    return "outgoing";
                }

                else if (transactionType == "used")
                {
                    return "outgoing";
                }


                else if (transactionType == "refund")
                {
                    return "outgoing";
                }
                else if (transactionType == "topup")
                {
                    return "incoming";
                }
            }

          




            return "outgoing";
        }

        public int orderstatuscount(string orderStatus, int profileid, string type)
        {

            var statuscount = 0;

            if (type == "seller")
            {
                statuscount = GetOrdersItem()
                    .Count(u => u.SellerID == profileid && u.OrderProcessStatus == orderStatus && u.OrderStatus == "confirm" && u.ItemType =="item");

            }
            else if (type == "buyer")
            {
                statuscount = GetOrdersItem()
                  .Count(u => u.BuyerID == profileid && u.OrderProcessStatus == orderStatus && u.OrderStatus == "confirm" && u.ItemType == "item");
            }

            return statuscount;
        }


        public decimal orderAmountcount(string orderStatus, int profileid, string type)
        {

            decimal totalAmount = 0;

            if (type == "seller")
            {
                totalAmount = GetOrdersItem()
    .Where(u => u.SellerID == profileid && u.OrderProcessStatus == orderStatus && u.OrderStatus == "confirm" && u.ItemType == "item")
    .Sum(u => u.SummaryOrderMetaData.GrandTotal);
            }
            else if (type == "buyer")
            {
               

                totalAmount = GetOrdersItem()
        .Where(u => u.BuyerID == profileid && u.OrderProcessStatus == orderStatus && u.OrderStatus == "confirm" && u.ItemType == "item")
        .Sum(u => u.SummaryOrderMetaData.GrandTotal);

            }

            return totalAmount;
        }

        #endregion



        #region ItemReview

        #region Review-JsonCreator
        public string Reviewmetadata(string reviewertype, int reviewerid, string reviewname, string reviewimage, decimal starrating, string description, string attachment, DateTime reviewdate, string existingMetaData)
        {
            // Initialize existingMetadata as an empty list if it's null
            List<ReviewMetaDataViewModel> existingMetadata = existingMetaData != null
                ? JsonConvert.DeserializeObject<List<ReviewMetaDataViewModel>>(existingMetaData)
                : new List<ReviewMetaDataViewModel>();

            // Create a new instance of ReviewMetaDataViewModel
            var newMetadata = new ReviewMetaDataViewModel
            {
                ReviewerID = reviewerid,
                ReviewerType = reviewertype,
                ReviewName = reviewname,
                ReviewerImage = reviewimage,
                StarRating = starrating,
                AverageRating = starrating,
                Description = description,
                Attachment = attachment,
                ReviewDate = reviewdate,
            };

            // Add the new metadata to the existing list
            existingMetadata.Add(newMetadata);

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }
        #endregion

        #region Review-Deserialized
        public List<ReviewMetaDataViewModel> ParseMetaDataReviewList(string json)
        {
            if (json == null)
            {
                return new List<ReviewMetaDataViewModel>(); // Return an empty list
            }

            List<ReviewMetaDataViewModel> parsedData = JsonConvert.DeserializeObject<List<ReviewMetaDataViewModel>>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion



        #region UpdateReview
        public string ReviewUpdate(int orderid, decimal rating, string description, string attachment, string reviewname, string reviewimage, int reviewerid)//buyer or seller
        {

            try
            {


                OrderMaster upSert = _dbContext.OrderMasters.FirstOrDefault(u => u.OrderId == orderid);

                if (upSert.ReviewMetaData != null)
                {


                    upSert.ReviewMetaData = Reviewmetadata("seller", reviewerid, reviewname, reviewimage, rating, description, attachment, DateTime.Now, upSert.ReviewMetaData);

                    _dbContext.SaveChanges();

                    _userhelper.UserOtherMetaDataUpdate(upSert.BuyerId, rating.ToString(), "BuyerRating");

                   
                   
                }
                else
                {


                    upSert.ReviewMetaData = Reviewmetadata("buyer", reviewerid, reviewname, reviewimage, rating, description, attachment, DateTime.Now, null);



                    _dbContext.SaveChanges();

                    _userhelper.UserOtherMetaDataUpdate(upSert.SellerId, rating.ToString(), "SellerRating");

                    ItemMetaData parsedData = JsonConvert.DeserializeObject<ItemMetaData>(upSert.ItemMetaData);
                    _productHelper.ItemOtherMetaDataUpdate(parsedData.basicModel.ID, rating.ToString(), "review");

                }





                ///Create Notification for user

                return "success";

            }
            catch (Exception)
            {

                return "fail";
            }


        }

        #endregion


        #region ReviewList-ITEM ID

        public List<ReviewMetaDataViewModel> ReviewListItemID(int itemId, string itemType)
        {
            var allOrders = GetOrdersItem().Where(u=>u.ItemType== itemType && u.OrderProcessStatus== "completed").OrderByDescending(u => u.OrderId);
            var reviewList = new List<ReviewMetaDataViewModel>();

            foreach (var order in allOrders)
            {
                if (order.ItemDetailMetaData != null && order.ItemDetailMetaData.basicModel.ID == itemId)
                {
                    if (order.ReviewMetaData != null)
                    {
                        reviewList.AddRange(order.ReviewMetaData);
                    }
                }
            }

            return reviewList;
        }

        #endregion

        #endregion


        #region ItemBoost- JsonCreator
        public string ItemBoostOrdermetadata(string itemtype, Guid itemid, string name, string image, string seourl, string invoicenumber, string paymenttype, int noofcredits, DateTime startdate, DateTime enddate, bool iscustomizedplan)

        {

            ItemBoostMetaDataViewModel existingMetadata = null;
            // Deserialize the JSON string into a list of VariationDetailMetaData objects
            // List<VariationDetailMetaData> variationmetadataList = JsonConvert.DeserializeObject<List<VariationDetailMetaData>>(VariationMetaData);


                var userselectedcurrency = _globalhelper.GetUserCurrency();
                var conversionrate = _globalhelper.ConversionRate(userselectedcurrency);
            string basecurrency = _globalhelper.BaseCurrency();
            // Get the item details here





            ///apply condition whether its item or profile
            ///
            decimal amount = 0;
            if (itemtype =="item")
            {
                var _listingboostSettings = _websettinghelper.GetWebsettingJson("ListingBoostSettings");

                var json = JsonConvert.DeserializeObject<ListingBoostSettingsModel>(_listingboostSettings);

                amount = json.Amount;
                if (iscustomizedplan == true)
                {
                    amount = json.CustomAmount;
                }

            }
            else if(itemtype=="profile")
            {
                var _listingboostSettings = _websettinghelper.GetWebsettingJson("ProfileBoostSettings");

                var json = JsonConvert.DeserializeObject<ProfileBoostSettingsModel>(_listingboostSettings);

                amount = json.Amount;
                if (iscustomizedplan == true)
                {
                    amount = json.CustomAmount;
                }
            }

            

         

           

            // Create a new instance of ItemDetailViewModel
            existingMetadata = new ItemBoostMetaDataViewModel
                {
                itemtype = itemtype,
                    ItemBoostGUID = itemid,
                    Name=name,
                    ImageUrl =image,
                    SEOURL=seourl,

                     PaymentType = paymenttype,
                     NoOfCredit =noofcredits,
                     StartDate = startdate,
                     EndDate = enddate,
                    IsCustomizedPlan= iscustomizedplan,

                ActualCurrency = basecurrency,
                ActualAmount = amount,
               


                ConversionCurrency = userselectedcurrency,

                ConversionAmount = Math.Round((decimal)amount * conversionrate, 2),



               
                   
                   

                    InvoiceNumber = invoicenumber,

                  

                };



            

            // Serialize the updated metadata back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);


            return updatedJson;
        }
        #endregion


        #region JsonDeserialized-ItemBoost


        public static ItemBoostMetaDataViewModel ParseMetaDataItemBoost(string json)
        {
            if (json == null)
            {
                return new ItemBoostMetaDataViewModel(); // Return an empty list
            }

            ItemBoostMetaDataViewModel parsedData = JsonConvert.DeserializeObject<ItemBoostMetaDataViewModel>(json);

            // Return the parsed list
            return parsedData;

        }
        #endregion


        #region Advertise-purchaseList
        public List<AdvertiseBoostViewModel> AdvertiseBoostList()
        {
            var purhcaselist = (from order in GetOrdersItem("boost")
                                join pb in _dbContext.ProductBoosts on order.InvoiceNumber equals pb.InvoiceNumber
                                where order.PaymentStatus =="paid"
                                      orderby order.OrderId descending

                                   
                                      select new AdvertiseBoostViewModel
                                      {



                                          ItemMetaData = order.ItemDetailMetaData,
                                           PaymentMetaData = order.PaymentMetaData,
                                          PurchaseDate = (DateTime)pb.InsertDate,

                                          BoostType = pb.BoostType,
                                          StartDate=(DateTime)pb.StartDate,
                                          EndDate = (DateTime)pb.EndDate,



                                          BuyerId =order.BuyerID,

                                          BoostMetaData = pb.BoostMetaData != null ? JsonConvert.DeserializeObject<BoostOtherMetaData>(pb.BoostMetaData) : null
                                      });

            return purhcaselist.ToList();
        }
        #endregion


        #region OrderOtherMetaData
        public static string OrderMetaDataCreator(string existingOrderOtherMetaData, decimal walletamountDeduction , string currency)
        {
            OrderOtherMetaData existingOtherMetaData;

            if (existingOrderOtherMetaData != null)
            {

                existingOtherMetaData = JsonConvert.DeserializeObject<OrderOtherMetaData>(existingOrderOtherMetaData);


                existingOtherMetaData.WalletUsedMetaData.WalletAmountUsed = walletamountDeduction;
                existingOtherMetaData.WalletUsedMetaData.WalletCurrency = currency;

            }
            else
            {

                existingOtherMetaData = new OrderOtherMetaData
                {
                    WalletUsedMetaData = new WalletUsedItemMetaData
                    {
                        WalletAmountUsed = walletamountDeduction,
                        WalletCurrency= currency
                    },

                    coupon = null
                };
            }




            string othermetadataJson = JsonConvert.SerializeObject(existingOtherMetaData);


            return othermetadataJson;
        }
        #endregion




        #region Order Helper V2

        #endregion


        #region CouponValidation
        public CouponValidationResult CouponValidation(string coupon, decimal totalAmount, int buyerid)
        {
                         

            ProductCoupon pc = _dbContext.ProductCoupons.FirstOrDefault(u => u.CouponName.ToLower() == coupon.ToLower() && u.IsPublish == true);

            var result = new CouponValidationResult();

            // Check if the coupon exists
            if (pc != null)
            {
                // Check Start Date Validation
                if (pc.StartDate > DateTime.Now)
                {
                    result.Message = "Coupon will start from " + GlobalHelper.DateFormat(pc.StartDate);
                    result.DiscountedAmount = 0;
                    result.IsSuccess = false;
                    return result;
                }

                // Check End Date Validation
                if (pc.EndDate < DateTime.Now)
                {
                    result.Message = "Coupon has expired on " + GlobalHelper.DateFormat(pc.EndDate);
                    result.DiscountedAmount = 0;
                    result.IsSuccess = false;
                    return result;
                }

                ///validat number of coupons allowed

                var uniqueInvoiceCount = GetOrdersItem()
                                              .Where(u => u.OrderStatus == "confirm" && u.SummaryOrderMetaData.CouponCode == coupon)
                                              .GroupBy(u => u.InvoiceNumber)
                                              .Count();

                if(pc.NoofCoupon<= uniqueInvoiceCount)
                {
                    result.Message = "All the coupons are used.";
                    result.DiscountedAmount = 0;
                    result.IsSuccess = false;
                    return result;
                }


                var uniqueInvoiceCountByUserID = GetOrdersItem()
                                             .Where(u => u.OrderStatus == "confirm" && u.SummaryOrderMetaData.CouponCode == coupon && u.BuyerID == buyerid)
                                             .GroupBy(u => u.InvoiceNumber)
                                             .Count();


                if (pc.PerPersonUsed <= uniqueInvoiceCountByUserID)
                {
                    result.Message = "Your limit for this coupon is finished.";
                    result.DiscountedAmount = 0;
                    result.IsSuccess = false;
                    return result;
                }


                // Calculate the discount based on the discount type
                if (pc.DiscountType == "Percentage")
                {
                    decimal percentageAmount = (totalAmount * pc.Discount) / 100;
                    result.DiscountedAmount =  percentageAmount;
                    result.IsSuccess = true;
                    result.CouponId = pc.ProductCouponId;
                }
                else if (pc.DiscountType == "Amount")
                {
                    result.CouponId = pc.ProductCouponId;
                    result.DiscountedAmount =  pc.Discount;
                    result.IsSuccess = true;
                }
               

                // If the coupon is valid
                result.Message = "Coupon Applied.";
            }
            else
            {
                result.Message = "Coupon code does not exist.";
                result.DiscountedAmount = 0;
                result.IsSuccess = false;
            }

            return result;
        }
        #endregion

    }
}
