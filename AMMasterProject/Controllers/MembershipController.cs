using Amazon;
using Amazon.Runtime.Internal.Transform;
using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PayPal.Api;
using Stripe;

namespace AMMasterProject.Controllers
{

    [Route("controller/[controller]/{action}")]
    [Controller]
    public class MembershipController : Controller
    {

        #region DI

        private readonly MyDbContext _dbContext;
        private readonly MembershipHelper _membershipHelper;
        private readonly WebsettingHelper _websettinghelper;
        private readonly OrderHelper _orderHelper;
        private readonly ProductHelper _producthelper;
        private readonly GlobalHelper _globalHelper;
        public MembershipController(MyDbContext context, MembershipHelper membershipHelper, WebsettingHelper websettinghelper, OrderHelper orderHelper, ProductHelper producthelper, GlobalHelper globalHelper)
        {
            _dbContext = context;
            _membershipHelper = membershipHelper;
            _websettinghelper = websettinghelper;
            _orderHelper = orderHelper;
            _producthelper = producthelper;
            _globalHelper = globalHelper;


        }
        #endregion

        #region PartialViews


        public IActionResult CreditPackageView()
        {
            //ViewBag.MyString = myString;

            var model = _membershipHelper.CreditPackageList();
            return PartialView("/Pages/Credits/_credit-plans.cshtml", model);
        }


        public IActionResult SubscriptionPackageView()
        {
            //ViewBag.MyString = myString;

            var model = _membershipHelper.SubscriptionPackageList();
            return PartialView("/Pages/Subscriptions/_subscription-plans.cshtml", model);
        }






        public IActionResult CreditAvailableCountPartialView()
        {
            //ViewBag.MyString = myString;

            int profileid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");

            if (profileid != 0)
            {
                CreditCounterViewModel model = new CreditCounterViewModel();

                var _creditsystemSettings = _websettinghelper.GetWebsettingJson("CreditSystemSettings");

                if (_creditsystemSettings != null && !string.IsNullOrEmpty(_creditsystemSettings))
                {
                    var json = JsonConvert.DeserializeObject<CreditCounterViewModel>(_creditsystemSettings);

                    if (json != null)
                    {
                        model = new CreditCounterViewModel
                        {
                            IsEnable = json.IsEnable,
                            CreditAvailable = _membershipHelper.CreditAvailable(profileid)
                        };
                    }
                }




                return PartialView("/Pages/Credits/CreditCount.cshtml", model);
            }
            return Content("");
        }








        #endregion


        // change this  Depreciate method and shifted to order controller
        //public IActionResult MembershipBasketSummaryView(string ID, string membershiptype)
        //{
        //    //ViewBag.MyString = myString;

        //    var model = _membershipHelper.MembershipBasketSummary(ID, membershiptype);

        //    if (model == null)
        //    {

        //        string errorUrl = "FAIL:";

        //        return Content(errorUrl);

        //    }
        //    else
        //    {

        //        return PartialView("/Pages/Payment/_membershipbasketsummary.cshtml", model);
        //    }



        //}

        #region CreditUsage


        [HttpGet]
        public IActionResult CreditUsage(string cipherText, string creditType)
        {
            try
            {

                string message = "";
                if (!User.Identity.IsAuthenticated)
                {
                    message = "login";
                }
                //First Checked the creditType  Address, Phone, Email REQUIRED
                int requiredcredit = _membershipHelper.CreditDeductionRequired(creditType);
                //2. Check if user has the credit available
                int profileid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
               
                int usercredit = _membershipHelper.CreditAvailable(profileid);
                //3. If user has the credit so return the decryption else


                 if (usercredit >= requiredcredit)
                {
                    message = EncryptionHelper.dycryption(cipherText);
                    ///place order for credit deduction
                    ///

                   

                   
                    string type = "credit";

                    string invoicenumber = GlobalHelper.GetInvoiceNumber(profileid.ToString(), type);

                  
                    string custom = $"Credit used to view the {creditType} {message.Replace(",","")} {GlobalHelper.GetReturnURL()},{requiredcredit},0,{GlobalHelper.GetReturnURL()} ";

                    string itemhelper = _orderHelper.ItemMetaData("creditused", "", "", 0, 1, "", invoicenumber, "", "", custom);

                    // Deserialize the JSON string to an anonymous type with properties updatedjson and success
                    var itemMetadataResult = JsonConvert.DeserializeAnonymousType(itemhelper, new { updatedjson = "", updatedsummaryjson = "", sellerid = 0 });


                    string ItemMetadata = itemMetadataResult.updatedjson;
                    string summarymetadata = itemMetadataResult.updatedsummaryjson;
                    int sellerid = 0;

                    string paymentmetadata = _orderHelper.paymentmetadata("NA", "paid", $"Credit used {message}", string.Empty, DateTime.Now, $"Credit used to view the {message} {GlobalHelper.GetReturnURL()}", ItemMetadata, "0", _globalHelper.BaseCurrency(), "", invoicenumber);// payment metadata





                    string orderstatus = _orderHelper.OrderCreation(profileid, sellerid, type, "confirm", "used", "completed", invoicenumber, ItemMetadata, paymentmetadata, "paid", summarymetadata);




                    //string itemmetadata = _membershipHelper.CreditUsageMetaData("Credit used to view the " + creditType + " " + message, requiredcredit, GlobalHelper.GetReturnURL());
                    //string invoicenumber = GlobalHelper.GetInvoiceNumber(profileid.ToString(), "credit");
                    //string orderstatus = _orderHelper.OrderCreation(profileid, 0, "credit", "confirm", "used", "completed", invoicenumber, itemmetadata, "", "");


                }
                else
                {
                    //show popop you do not have sufficient credit
                    message = "insufficient";
                }



                return new JsonResult(message);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting CreditUsage" + ex.Message);
            }
        }


































        #endregion

        #region CreditDeduction
        [HttpGet]
        public IActionResult CreditDeduction(string creditType)
        {
            try
            {

           
                //First Checked the creditType  Address, Phone, Email REQUIRED
                int requiredcredit = _membershipHelper.CreditDeductionRequired(creditType);
               


                return new JsonResult(requiredcredit);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting CreditDeduction" + ex.Message);
            }
        }
        #endregion



        #region AvailableCredits
        [HttpGet]
        public IActionResult AvailbleUserCredit()
        {
            try
            {


                int profileid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                int usercredit = _membershipHelper.CreditAvailable(profileid);



                return new JsonResult(usercredit);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Return an error response
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting AvailbleUserCredit" + ex.Message);
            }
        }
        #endregion

    }
}
