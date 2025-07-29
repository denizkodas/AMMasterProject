using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PayPal.Api;
using Razorpay.Api;
using Stripe;
using System.ComponentModel.DataAnnotations;

namespace AMMasterProject.Controllers
{

    [Route("controller/[controller]/{action}")]
    [Controller]
    public class OrderController : Controller
    {
        #region DI


       
       
        private readonly ProductHelper _producthelper;
        private readonly UserHelper _userhelper;
        private readonly OrderHelper _orderhelper;
        private readonly MembershipHelper _membershipHelper;
        private readonly GlobalHelper _globalhelper;
        private readonly WebsettingHelper _websettinghelper;
        public ProductViewModel productmodel { get; set; }
        public OrderController(  ProductHelper producthelper, FileUploadHelper fileUploadHelper, OrderHelper orderhelper, MembershipHelper membershipHelper, GlobalHelper globalhelper, UserHelper userhelper, WebsettingHelper websettinghelper)
        {
            
           
            _producthelper = producthelper;
            _orderhelper = orderhelper;
            _membershipHelper = membershipHelper;
            _globalhelper = globalhelper;
            _userhelper = userhelper;
            _websettinghelper = websettinghelper;

        }


        #endregion

        public IActionResult Index()
        {
            return View();
        }

        #region OrderCreate


        //order id only send if its on edit mode
        //type credit, subscription or item
        //transactiontype = free purchase, used
        //orderStatus =Cart Confirm
        // //orderProcessStatus =Processing Completed  Cancelled
        public IActionResult CreateOrder(int Id, int quantity, string instruction, string type, string transactiontype, string orderStatus , string orderprocesstatus, string? VariationMetaData="", string? ChargesMetaData = "")
        {
            ///user variables
            int buyerid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");


            string invoicenumber = GlobalHelper.ReadCookie("cartInvoiceNumber");
            string ItemMetadata = "";
            string summarymetadata = "";
            string loyaltypointsmetadata = "";
            int sellerid = 0;
            if (invoicenumber == null)
            {
                ///before generating new invoice first check if there is already ITEM active in cart if yes
                ///so get that invoice number from order table
                ///

                //Depreciated on 18 jan 2024
                //invoicenumber = GlobalHelper.GetInvoiceNumber(Id.ToString(), type);

                invoicenumber = _orderhelper.invoicenumberactive(buyerid, Id.ToString(), type);

                GlobalHelper.SetCookie("cartInvoiceNumber", invoicenumber);
            }

            else
            {
                invoicenumber = GlobalHelper.ReadCookie("cartInvoiceNumber");
            }

            if (type == "credit")
            {
                //incase of credit or subscription do not use old invoice number

                invoicenumber = GlobalHelper.GetInvoiceNumber(Id.ToString(), type);

                GlobalHelper.SetCookie("cartInvoiceNumber", invoicenumber);


                string itemhelper = _orderhelper.ItemMetaData(type, "", "", Id, quantity, instruction, invoicenumber, VariationMetaData, ChargesMetaData);

                // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                ItemMetadata = itemMetadataResult.updatedjson;
                summarymetadata = itemMetadataResult.updatedsummaryjson;
                sellerid = int.Parse(itemMetadataResult.sellerid.ToString());
                //loyaltypointsmetadata = itemMetadataResult.loyaltypoints;
            }
            else if (type == "subscription")
            {
                //incase of credit or subscription do not use old invoice number

                invoicenumber = GlobalHelper.GetInvoiceNumber(Id.ToString(), type);

                GlobalHelper.SetCookie("cartInvoiceNumber", invoicenumber);

                string itemhelper = _orderhelper.ItemMetaData(type, "", "", Id, quantity, instruction, invoicenumber, VariationMetaData, ChargesMetaData);

                // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                ItemMetadata = itemMetadataResult.updatedjson;
                summarymetadata = itemMetadataResult.updatedsummaryjson;
                sellerid = int.Parse(itemMetadataResult.sellerid.ToString());
                //loyaltypointsmetadata = itemMetadataResult.loyaltypoints;
            }
            else if(type=="item")///first target this
            {
                //ItemMetadata = _orderhelper.ItemOrdermetadata(0, Id, quantity, instruction, invoicenumber, null, VariationMetaData);
                string itemhelper= _orderhelper.ItemMetaData(type, "", "", Id, quantity, instruction, invoicenumber, VariationMetaData, ChargesMetaData);

                // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson="", sellerid = 0});


                ItemMetadata = itemMetadataResult.updatedjson;
                summarymetadata = itemMetadataResult.updatedsummaryjson;
                sellerid = int.Parse(itemMetadataResult.sellerid.ToString());
                //loyaltypointsmetadata = itemMetadataResult.loyaltypoints;

            }
            else if(type=="itemboost")
            {

                //ItemMetadata = _orderhelper.ItemBoostOrdermetadata(Id, invoicenumber,"", 0, DateTime.Now, DateTime.Now);
            }
            else if (type == "profileboost")
            {
                //ItemMetadata = _orderhelper.ItemBoostOrdermetadata(Id, invoicenumber,"", 0, DateTime.Now, DateTime.Now);
            }

            // Create order





            string message = _orderhelper.OrderCreation(buyerid, sellerid, type, orderStatus, transactiontype, orderprocesstatus, invoicenumber, ItemMetadata, null, null, summarymetadata, _orderhelper.ChargesMetaData);


            ////Controller Part
            TempData["success"] = $"{type} added to cart";

            string url = $"/payment/selection/{invoicenumber}/{type}";

            if(type=="item")
            {
                url = "/shipping";
            }

            return Content(url);
        }
        #endregion

        #region OrderBasketSummary
        public IActionResult OrderBasketSummaryView(string InvoiceNumber)
        {
            //ViewBag.MyString = myString;


            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable


            }

            List<OrderViewModel> orderViewModelsList = _orderhelper.GetOrdersItem().Where(u => u.OrderStatus == "cart" && u.InvoiceNumber ==InvoiceNumber && u.BuyerID ==loginid).OrderBy(u => u.SellerID).ToList();

            if (orderViewModelsList == null)
            {

                string errorUrl = "FAIL:";

                return Content(errorUrl);

            }
            else
            {

                return PartialView("/Pages/Payment/_Orderbasketsummary.cshtml", orderViewModelsList);
            }



        }
        #endregion


        #region ItemInvoice
        public IActionResult ItemInvoice(string InvoiceNumber, string usertype)
        {
            //ViewBag.MyString = myString;


            int loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
            //string usertypeLoginType = User.FindFirst("UserType")?.Value ?? "0";

            List<OrderViewModel> orderViewModelsList = _orderhelper.GetOrdersItem("invoicenumber", 0, InvoiceNumber).OrderBy(u => u.SellerID).ToList();

            //if client so filter the data client wise
            //if (usertype == "Client")
            //{
            //    orderViewModelsList = orderViewModelsList.Where(u => u.BuyerID == loginid).ToList();
            //}



            // continue with loginid variable
            if (usertype == "Vendor")
            {
                orderViewModelsList = orderViewModelsList.Where(u => u.SellerID == loginid).ToList();
            }
           

            ///override vendor and client logic if usertype is admin



            if (orderViewModelsList.Count ==0)
            {

                string errorUrl = "Invoice number not exist";

                return Content(errorUrl);

            }
            else
            {

                return PartialView("/Pages/orders/_invoice.cshtml", orderViewModelsList);
            }



        }
        #endregion

        #region QuantityUpdate
        public IActionResult QtyUpdate(int orderid, int Quantity, string instruction)
        {

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

           string message=_orderhelper.CartUpdate(orderid, Quantity, instruction, loginid);

            return Content(message);
        }

        #endregion

        #region OrderUpdate

        //Add to Cart order update
        //QTY,


        public IActionResult ItemOrderUpdate(int orderid, string? orderStatus, string? orderprocessstatus, string? ordernotes, string? paymentstatus, string? itemmetadata, string? paymentmetdata)
        {

            string message = _orderhelper.OrderUpdate(orderid, orderStatus, orderprocessstatus, ordernotes, paymentstatus, itemmetadata, paymentmetdata);

            return Content("success");
        }

        #endregion

        #region ShowCart
        public IActionResult ShowCart()
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable

               
            }
            List<OrderViewModel> orderViewModelsList = _orderhelper.GetOrdersItem("cart", loginid).OrderBy(u => u.SellerID).ToList();
           
            if(orderViewModelsList.Count > 0)
            {

                string Invoicenumber = orderViewModelsList.First().InvoiceNumber;

                GlobalHelper.SetCookie("cartInvoiceNumber", Invoicenumber);
              
            }
            
            
            return PartialView("/Pages/orders/_cartview.cshtml", orderViewModelsList);


        }
        #endregion

        #region CartCounter
        public int CartCounter()
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

            var q = _orderhelper.GetOrdersItem()
     .Where(u => u.OrderStatus == "cart" && u.BuyerID == loginid && u.ItemType == "item")
     .Sum(u => u.ItemDetailMetaData.basicModel.Quantity);
            return q;
        }
        #endregion

        #region CartDelete

        [HttpPost]
        public IActionResult DeleteCartItem(int OrderId)
        {

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

           string message= _orderhelper.CartDelete(OrderId, loginid);
           

            return Content(message);
        }

        #endregion

        #region OrderStatusUpdates

        [HttpPost]
        public IActionResult OrderStatusUpdate(int OrderId, string orderprocesstatus, string ordernotes)
        {

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

            string message = _orderhelper.OrderUpdate(OrderId,string.Empty, orderprocesstatus, ordernotes, string.Empty, string.Empty, string.Empty);
          

            return Content(message);
        }

        #endregion



        #region ShippingUpdate

        public IActionResult ShippingUpdate(Guid CustomerAddressGuid)
        {

            string shippingMetaData = _orderhelper.Shippingmetadata(CustomerAddressGuid);

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }
            var redirecturl = _orderhelper.ShippingUpdateCart(loginid, shippingMetaData);





            return Content(redirecturl);


        }
        #endregion


        #region OrderStatus-Setup
        public IActionResult OrderStatusSetup(string key)
        {
            List<OrderStatusSetupViewModel> ModelsList = _orderhelper.OrderStatusSetup(key).ToList();

            if (ModelsList == null)
            {
                string errorUrl = "FAIL";
                return Content(errorUrl);
            }
            else
            {
                return Json(ModelsList); // Serialize ModelsList to JSON and return it
            }
        }
        #endregion



        #region Review
        #region ReviewUpsert

        public IActionResult ReviewUpdate(int orderid, decimal rating, string description, string attachment)
        {

         

            int loginid = 0;
            string reviewname = "";
            string reviewimage = "";
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                reviewname = User.FindFirst("FirstName")?.Value ?? "0"  + " " + User.FindFirst("LastName")?.Value ?? "0";
                reviewimage = User.FindFirst("Image")?.Value ?? "";
                // continue with loginid variable
            }

            string reviewUpsert = _orderhelper.ReviewUpdate(orderid, rating, description, attachment, reviewname, reviewimage, loginid);



            return Content("success");


        }

        #endregion



        #region ReviewList- ItemID
        public IActionResult ReviewListItemID(int ItemID, string itemtype, int startIndex)
        {
            List<ReviewMetaDataViewModel> ViewModelsList = _orderhelper.ReviewListItemID(ItemID, itemtype).ToList();

            if (ViewModelsList == null)
            {
                string errorUrl = "FAIL:";
                return Content(errorUrl);
            }
            else
            {
                // Determine the number of records to fetch
                int takeCount = 10;

                // Filter and take the next set of records based on startIndex and takeCount
                var paginatedList = ViewModelsList.Skip(startIndex).Take(takeCount).ToList();

                return PartialView("/Pages/review/_listreview.cshtml", paginatedList);
            }
        }
        #endregion
        #endregion



        #region Advertise-Order
        #region ProductBoost-CreditUsage
        [HttpGet]
        public IActionResult AdvertiseCreditUsage(string advertisetype, string itemtype, int noofcreditrequired,  Guid itemid, DateTime startdate, DateTime enddate, bool iscustomized)
        {
            try
            {

                string message = "";
                string dateformat = _globalhelper.Dateformat("datetime");

                string itemname = "";
                string image = "";
                string itemurl = "";
                int itemID = 0;
                ProductViewModel product = null;
                UserGeneralView user = null;
                if (itemtype == "item")
                {
                    

                    product = _producthelper.productmasterdataV2(0, "guididwise", 1, 1, 0, itemid.ToString()).FirstOrDefault();

                    itemname = product.ProductName;
                    image = product.ProductImage;
                    itemurl = GlobalHelper.GetCurrentDomainName() + "/item/" + product.ProductSeourl;
                    itemID = product.ProductId;
                }
                else if(itemtype == "profile")
                {
                    

                    user = _userhelper.UserGeneralByGUID(Guid.Parse(itemid.ToString()));


                    itemname = user.sellerviewmodel.BusinessName;
                    image = user.Image;
                    itemurl = GlobalHelper.GetCurrentDomainName()  + user.sellerviewmodel.BusinessUrlpath;
                    itemID = user.ProfileId;
                }

                if (!User.Identity.IsAuthenticated)
                {
                    message = "login";
                }
                //First Checked the creditType  Address, Phone, Email REQUIRED
                int requiredcredit = noofcreditrequired;
                //2. Check if user has the credit available
                int profileid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                int usercredit = _membershipHelper.CreditAvailable(profileid);
                //3. If user has the credit so return the decryption else


                if (usercredit >= requiredcredit)
                {

                    #region CreditUsage
                    string type = itemtype+advertisetype;

                    string invoicenumber = GlobalHelper.GetInvoiceNumber(itemID.ToString(), "C");


                    string custom = $"Credit used to boost the {itemtype} {GlobalHelper.GetReturnURL()}#" +
                        $"{requiredcredit}#" +
                        $"0#" +
                        $"{itemurl}#" +
                        $"{startdate.ToString(dateformat)} and {enddate.ToString(dateformat)} ";

                    string itemhelper = _orderhelper.ItemMetaData("creditused", "", "", 0, 1, "", invoicenumber, "", "", custom);

                    // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                    var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                    string ItemMetadata = itemMetadataResult.updatedjson;
                    string summarymetadata = itemMetadataResult.updatedsummaryjson;
                    int sellerid = 0;

                    string paymentmetadata = _orderhelper.paymentmetadata("NA", "paid", $"Credit used {message}", string.Empty, DateTime.Now, $"Credit used to view the {message} {GlobalHelper.GetReturnURL()}", ItemMetadata, "0", _globalhelper.BaseCurrency(), "", invoicenumber);// payment metadata





                    string orderstatus = _orderhelper.OrderCreation(profileid, sellerid, "credit", "confirm", "used", "completed", invoicenumber, ItemMetadata, paymentmetadata, "paid", summarymetadata);



                    #endregion



                    #region BoostOrderAsSales

                  
                    invoicenumber = GlobalHelper.GetInvoiceNumber(profileid.ToString(), type);


                     custom = $"{type} {advertisetype} {itemname} between {startdate.ToString(dateformat)} and {enddate.ToString(dateformat)}" +
                        $"#{requiredcredit}" +
                        $"#0" +
                        $"#{itemurl}" +
                        $"#{image} "+
                        $"#{iscustomized}" +
                        $"#payment";
                    

                     itemhelper = _orderhelper.ItemMetaData(type, "", "", 0, 1, "", invoicenumber, "", "", custom);

                    // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                     itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                     ItemMetadata = itemMetadataResult.updatedjson;
                     summarymetadata = itemMetadataResult.updatedsummaryjson;
                     sellerid = 0;

                     paymentmetadata = _orderhelper.paymentmetadata("credit", "paid", $"Credit used {message}", string.Empty, DateTime.Now, $"Credit used to view the {message} {GlobalHelper.GetReturnURL()}", ItemMetadata, "0", _globalhelper.BaseCurrency(), "", invoicenumber);// payment metadata





                     orderstatus = _orderhelper.OrderCreation(profileid, sellerid, type, "confirm", "purchased", "completed", invoicenumber, ItemMetadata, paymentmetadata, "paid", summarymetadata);

                    #endregion

                   
                    //1. created this order to for credit deduction
                    //string itemmetadata = _membershipHelper.CreditUsageMetaData("Credit used to boost the " + itemtype + " " + message, requiredcredit, itemurl);
                    //string invoicenumber = GlobalHelper.GetInvoiceNumber(profileid.ToString(), advertisetype);
                    //string orderstatus = _orderhelper.OrderCreation(profileid, 0, "credit", "confirm", "used", "completed", invoicenumber, itemmetadata, "", "");


                    //2. then second step to create purchase of advertise boost to count as sale
                    //itemmetadata = _orderhelper.ItemBoostOrdermetadata(itemtype,itemid, itemname, image,itemurl, invoicenumber, "credit", requiredcredit, startdate, enddate, iscustomized);
                    //orderstatus = _orderhelper.OrderCreation(profileid, 0, advertisetype, "confirm", "purchased", "completed", invoicenumber, itemmetadata, "", "");

                    ///insert in productbost
                    ///
                    _producthelper.productboost(itemid, itemtype, startdate, enddate, invoicenumber, true);

                    

                    TempData["success"] = $"{itemname} is successfully promoted.";

                    message = "/advertise/allads";



                    return Content(message);
                    //update payment meta data as wel/

                    //orderstatus = _orderHelper.OrderPaymentUpdate("Credit Usage", "paid", "", string.Empty, "0", "", "", invoicenumber, profileid, "completed");
                }
                else
                {
                    //show popop you do not have sufficient credit
                    message = "insufficient";
                }



                return Content(message);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting AdvertiseCreditUsage" + ex.Message);
            }
        }

        #endregion


        #region ProductBoost-Payment
        [HttpGet]
        public IActionResult AdvertisePayment(string advertisetype, string itemtype, Guid itemid, DateTime startdate, DateTime enddate, bool iscustomized, decimal amount)
        {
            try
            {

                string message = "";
                string dateformat = _globalhelper.Dateformat("datetime");

                string itemname = "";
                string image = "";
                string itemurl = "";
                int itemID = 0;

                ProductViewModel product = null;
                UserGeneralView user = null;
                if (itemtype == "item")
                {


                    product = _producthelper.productmasterdataV2(0, "guididwise", 1, 1, 0, itemid.ToString()).FirstOrDefault();

                    itemname = product.ProductName;
                    image = product.ProductImage;
                    itemurl = GlobalHelper.GetCurrentDomainName() + "/item/" + product.ProductSeourl;
                    itemID = product.ProductId;
                }
                else if (itemtype == "profile")
                {


                    user = _userhelper.UserGeneralByGUID(Guid.Parse(itemid.ToString()));


                    itemname = user.sellerviewmodel.BusinessName;
                    image = user.Image;
                    itemurl = GlobalHelper.GetCurrentDomainName() + user.sellerviewmodel.BusinessUrlpath;
                    itemID = user.ProfileId;
                }

                if (!User.Identity.IsAuthenticated)
                {
                    message = "login";
                }
            
                
                //2.
                int profileid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");


                #region BoostOrderAsSales


                string type = itemtype + advertisetype;
                string invoicenumber = GlobalHelper.GetInvoiceNumber(itemID.ToString(), type);

                GlobalHelper.SetCookie("cartInvoiceNumber", invoicenumber);

                string custom = $"{itemtype} {advertisetype} {itemname} between {startdate.ToString(dateformat)} and {enddate.ToString(dateformat)}" +
                $"#0" +
                $"#{amount}" +
                $"#{itemurl}" +
                $"#{image} " +
                $"#{iscustomized}"+
                $"#payment";
                string itemhelper = _orderhelper.ItemMetaData(type, "", "", itemID, 1, "", invoicenumber, "", "", custom);

                // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


               var  ItemMetadata = itemMetadataResult.updatedjson;
               var summarymetadata = itemMetadataResult.updatedsummaryjson;
               int sellerid = int.Parse(itemMetadataResult.sellerid.ToString());






                string orderstatus = _orderhelper.OrderCreation(profileid, sellerid, type, "cart", "purchased", "completed", invoicenumber, ItemMetadata, null, null, summarymetadata, _orderhelper.ChargesMetaData);
                
                ///it will only show once payment is paid
                _producthelper.productboost(itemid, itemtype, startdate, enddate, invoicenumber, true);



                #endregion


                TempData["success"] = $"{itemname} is selected to promote";

                message = $"/payment/selection/{invoicenumber}/boost";

                

                return Content(message);


                
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting AdvertiseCreditUsage" + ex.Message);
            }
        }

        #endregion


        #endregion


        #region ItemBoost-Admin
        [HttpGet]
        public IActionResult AdvertiseAdmin(string advertisetype, string itemtype, Guid itemid, int noofdays, bool iscustomized)
        {
            try
            {

                string message = "";
                string dateformat = _globalhelper.Dateformat("datetime");

                string itemname = "";
                string image = "";
                string itemurl = "";
                int itemID = 0;
                ProductViewModel product = null;
                UserGeneralView user = null;

                DateTime startdate = DateTime.Now;
                DateTime enddate = startdate.AddDays(noofdays);
                if (itemtype == "item")
                {


                    product = _producthelper.productmasterdataV2(0, "guididwise", 1, 1, 0, itemid.ToString()).FirstOrDefault();

                    itemname = product.ProductName;
                    image = product.ProductImage;
                    itemurl = GlobalHelper.GetCurrentDomainName() + "/item/" + product.ProductSeourl;
                    itemID = product.ProductId;
                }
                else if (itemtype == "profile")
                {


                    user = _userhelper.UserGeneralByGUID(Guid.Parse(itemid.ToString()));


                    itemname = user.sellerviewmodel.BusinessName;
                    image = user.Image;
                    itemurl = GlobalHelper.GetCurrentDomainName() + user.sellerviewmodel.BusinessUrlpath;
                    itemID = user.ProfileId;
                }

                if (!User.Identity.IsAuthenticated)
                {
                    message = "login";
                }

                int profileid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");


                #region CreditUsage
                string type = itemtype + advertisetype;

                string invoicenumber = GlobalHelper.GetInvoiceNumber(itemID.ToString(), "C");


                string custom = $"Credit used to boost the {itemtype} {GlobalHelper.GetReturnURL()}#" +
                    $"0#" +
                    $"0#" +
                    $"{itemurl}#" +
                    $"{startdate.ToString(dateformat)} and {enddate.ToString(dateformat)} ";

                string itemhelper = _orderhelper.ItemMetaData("creditused", "", "", 0, 1, "", invoicenumber, "", "", custom);

                // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                string ItemMetadata = itemMetadataResult.updatedjson;
                string summarymetadata = itemMetadataResult.updatedsummaryjson;
                int sellerid = 0;

                string paymentmetadata = _orderhelper.paymentmetadata("NA", "paid", $"Credit used {message}", string.Empty, DateTime.Now, $"Credit used to view the {message} {GlobalHelper.GetReturnURL()}", ItemMetadata, "0", _globalhelper.BaseCurrency(), "", invoicenumber);// payment metadata





                string orderstatus = _orderhelper.OrderCreation(profileid, sellerid, "credit", "confirm", "used", "completed", invoicenumber, ItemMetadata, paymentmetadata, "paid", summarymetadata);



                #endregion



                #region BoostOrderAsSales


                invoicenumber = GlobalHelper.GetInvoiceNumber(profileid.ToString(), type);


                custom = $"{type} {advertisetype} {itemname} between {startdate.ToString(dateformat)} and {enddate.ToString(dateformat)}" +
                   $"#0" +
                   $"#0" +
                   $"#{itemurl}" +
                   $"#{image} " +
                   $"#{iscustomized}" +
                   $"#payment";


                itemhelper = _orderhelper.ItemMetaData(type, "", "", 0, 1, "", invoicenumber, "", "", custom);

                // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                ItemMetadata = itemMetadataResult.updatedjson;
                summarymetadata = itemMetadataResult.updatedsummaryjson;
                sellerid = 0;

                paymentmetadata = _orderhelper.paymentmetadata("credit", "paid", $"Credit used {message}", string.Empty, DateTime.Now, $"Credit used to view the {message} {GlobalHelper.GetReturnURL()}", ItemMetadata, "0", _globalhelper.BaseCurrency(), "", invoicenumber);// payment metadata





                orderstatus = _orderhelper.OrderCreation(profileid, sellerid, type, "confirm", "purchased", "completed", invoicenumber, ItemMetadata, paymentmetadata, "paid", summarymetadata);

                #endregion

                _producthelper.productboost(itemid, itemtype, startdate, enddate, invoicenumber, true);
                //message = itemname + " between " + startdate.ToString(dateformat) + " and " + enddate.ToString(dateformat);

                //    string itemmetadata = _membershipHelper.CreditUsageMetaData("Credit used to boost the " + itemtype + " " + message, 0, itemurl);
                //    string invoicenumber = GlobalHelper.GetInvoiceNumber(profileid.ToString(), advertisetype);
                //    string orderstatus = _orderhelper.OrderCreation(profileid, 0, "credit", "confirm", "used", "completed", invoicenumber, itemmetadata, "", "");


                //    //2. then second step to create purchase of advertise boost to count as sale
                //    itemmetadata = _orderhelper.ItemBoostOrdermetadata(itemtype, itemid, itemname, image, itemurl, invoicenumber, "credit", 0, startdate, enddate, iscustomized);
                //    orderstatus = _orderhelper.OrderCreation(profileid, 0, advertisetype, "confirm", "purchased", "completed", invoicenumber, itemmetadata, "", "");


                //    _producthelper.productboost(itemid, itemtype, startdate, enddate, invoicenumber, true);




                TempData["success"] = $"{itemname} is successfully promoted.";

                message = "/advertise/allads";



                return Content(message);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting AdvertiseCreditUsage" + ex.Message);
            }
        }

        #endregion


        #region WalletUsed
        //[HttpPost]
        //public IActionResult WalletUsed(string invoicenumber, int buyerid, string type)//type used, removed
        //{
        //    try
        //    {



        //        string message = _orderhelper.WalletCreate(invoicenumber, buyerid, type);

        //        return Json(new { message });



        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        // Return an error response
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting WalletUsed" + ex.Message);
        //    }
        //}


        
        public IActionResult WalletAvailable()
        {
            try
            {

                if (User.Identity.IsAuthenticated)
                {

                   int loginid = int.Parse(User.FindFirst("UserID")?.Value);

                    string availablewallet = _userhelper.WalletAvailable(loginid).ToString();

                    return Json(new { availablewallet });
                    //AvailableWallet = _orderhelper.WalletAvailable(loginid);

                    // continue with loginid variable
                }

                else
                {
                    return Json(new { availablewallet = 0 });
                }

               



            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting WalletUsed" + ex.Message);
            }
        }



        #endregion

        #region CouponValidation
        [HttpPost]
        public IActionResult CouponValidation(string couponcode, decimal totalamount, string invoicenumber)
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }
            var validationResult = _orderhelper.CouponValidation(couponcode, totalamount, loginid);
            if(validationResult.IsSuccess==true)
            {
               

                string message = _orderhelper.CouponUpdate(invoicenumber, validationResult.CouponId, couponcode,"", loginid,  validationResult.DiscountedAmount);
            }

            return Json(new
            {
                IsSuccess = validationResult.IsSuccess,
                Message = validationResult.Message,
                DiscountedAmount = validationResult.DiscountedAmount,
                PayableAmount = totalamount- validationResult.DiscountedAmount


               
            });
        }
        #endregion


        #region CouponAlreadyExist
        public IActionResult CouponAlreadyExistForOrder(string InvoiceNumber)
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
            }

            var orders = _orderhelper.GetOrdersItem()
                                     .Where(u => u.OrderStatus == "cart" && u.InvoiceNumber == InvoiceNumber && u.BuyerID == loginid)
                                     .OrderBy(u => u.SellerID);

            //var totalCouponAmount = orders.Sum(u => u.SummaryOrderMetaData.CouponAmount);
            //var totalAmount= orders.Sum(u => u.SummaryOrderMetaData.GrandTotal);
            var couponCode = orders.FirstOrDefault()?.SummaryOrderMetaData.CouponCode;

            if (!orders.Any())
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Order does not exist"
                });
            }
            else if (couponCode!="")
            {
                
                var validationResult = new { Message = "Coupon Applied" }; // Replace with actual validation result

                return Json(new
                {
                    IsSuccess = true,
                    //Message = validationResult.Message,
                    //DiscountedAmount = totalCouponAmount,
                    //PayableAmount = totalAmount - totalCouponAmount

                    //TotalCouponID = totalCouponID,
                    CouponCode = couponCode

                });
            }

            return Json(new
            {
                IsSuccess = "Fail",
                Message = "No coupons applied"
            });
        }
        #endregion

        #region AvailableLoyaltypoints
        public IActionResult AvailableLoyaltypoints()
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
            }


            LoyaltyPointSettingsModel loyalpoints = new LoyaltyPointSettingsModel();
            var _loyaltpointSettings = _websettinghelper.GetWebsettingJson("LoyaltyPointSettings");
            if (_loyaltpointSettings != null && !string.IsNullOrEmpty(_loyaltpointSettings))
            {
                var json = JsonConvert.DeserializeObject<LoyaltyPointSettingsModel>(_loyaltpointSettings);

                if (json != null)
                {
                    loyalpoints = new LoyaltyPointSettingsModel
                    {
                       
                        MinPointsRedeem = json.MinPointsRedeem,
                        PointsExpiry = json.PointsExpiry,
                        IsEnable = json.IsEnable
                    };
                }
            }


            var orders = _orderhelper.GetOrdersItem()
                              .Where(u => u.BuyerID == loginid && u.OrderProcessStatus=="completed")
                              .SelectMany(u => u.LoyaltyPointMetaData)
                              .Where(lp =>  lp.ExpirtyDate >= DateTime.Now)
                              .Sum(lp => lp.Points);

            var AvailablePoints = new LoyaltyPointsAvailableViewModel
            {
                AvailablePoints = orders,
                MinRedeemPoints= loyalpoints.MinPointsRedeem,
                IsEnabled=loyalpoints.IsEnable
            };


            return PartialView("/Pages/orders/_availableloyaltypoints.cshtml", AvailablePoints);

        }
        #endregion
    }
}
