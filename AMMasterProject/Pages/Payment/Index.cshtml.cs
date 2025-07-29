using Amazon;
using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PayPal.Api;
using Razorpay.Api;
using Serilog;
using Stripe;

namespace AMMasterProject.Pages.Payment
{

    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model


       

        public List<SelectListItem> paymentgatewaylist { get; set; }
        private readonly WebsettingHelper _websettinghelper;
        private readonly MembershipHelper _membershipHelper;
        private readonly PaymentGatewayHelper _paymentgatewayhelper;
        private readonly OrderHelper _orderhelper;
        public readonly GlobalHelper _globalhelper;
        public readonly UserHelper _userhelper;
        public string ID { get; set; }

        public int loginid { get; set; }
        public string MembershipType { get; set; }

        public bool isCurrencyAllowed { get; set; }


        public decimal AvailableWallet { get; set; }
        #endregion

        #region DI


        public IndexModel(WebsettingHelper websetting, MembershipHelper membershipHelper, PaymentGatewayHelper paymentGatewayHelper, OrderHelper orderhelper, GlobalHelper globalhelper, UserHelper userhelper)
        {
            _websettinghelper = websetting;
            _membershipHelper = membershipHelper;
            _paymentgatewayhelper = paymentGatewayHelper;
            _orderhelper = orderhelper;
            _globalhelper = globalhelper;   
            _userhelper = userhelper;
        }

        #endregion
        public void OnGet()
        {
            paymentgatewaylist = new List<SelectListItem>();
            if (RouteData.Values["ID"]== null || RouteData.Values["MembershipType"]==null)
            {
                // Redirect to error page or return an error response
               

                Response.Redirect("/Error?Title=Selection Fail&Message=Something went wrong&Body=Please try again later.");
            }

            ID = (string)RouteData.Values["ID"];
            MembershipType = (string)RouteData.Values["MembershipType"];


            ///cod  show only for item purchase , credit, subsription and boost cod is not an option but only bank transfer
            ///
            if (MembershipType == "item")
            {
                var _codSettings = _websettinghelper.GetWebsettingJson("CODSettings");
                if (_codSettings != null && !string.IsNullOrEmpty(_codSettings))
                {
                    var json = JsonConvert.DeserializeObject<CODSettingsModel>(_codSettings);

                    if (json != null)
                    {

                        paymentgatewaylist.Add(new SelectListItem
                        {
                            Value = "cod",
                            Text = "Cash On Delivery"
                        });

                    }
                }
            }

            var _transferSettings = _websettinghelper.GetWebsettingJson("BankTransferSettings");
            if (_transferSettings != null && !string.IsNullOrEmpty(_transferSettings))
            {
                var json = JsonConvert.DeserializeObject<BankTransferSettingsModel>(_transferSettings);

                if (json != null)
                {
                   


                    paymentgatewaylist.Add(new SelectListItem
                    {
                        Value = "banktransfer",
                        Text = "Bank Transfer"
                    });
                }
            }

            var _paypalSettings = _websettinghelper.GetWebsettingJson("PaypalSettings");
         
            if (_paypalSettings != null && !string.IsNullOrEmpty(_paypalSettings))
            {
                var json = JsonConvert.DeserializeObject<PaypalSettingsModel>(_paypalSettings);

                if (json != null && json.IsEnable)
                {
                    paymentgatewaylist.Add(new SelectListItem
                    {
                        Value = "paypal",
                        Text = "PayPal"
                    });
                }
            }


            var _stripeSettings = _websettinghelper.GetWebsettingJson("StripeSettings");

            if (_stripeSettings != null && !string.IsNullOrEmpty(_stripeSettings))
            {
                var json = JsonConvert.DeserializeObject<StripeSettingsModel>(_stripeSettings);

                if (json != null && json.IsEnable)
                {
                    paymentgatewaylist.Add(new SelectListItem
                    {
                        Value = "stripe",
                        Text = "Stripe",
                        
                    });
                }
            }


            var _razorpaySettings = _websettinghelper.GetWebsettingJson("RazorPaySettings");
            if (_razorpaySettings != null && !string.IsNullOrEmpty(_razorpaySettings))
            {
                var json = JsonConvert.DeserializeObject<RazorPaySettingsModel>(_razorpaySettings);

                if (json != null && json.IsEnable)
                {
                    paymentgatewaylist.Add(new SelectListItem
                    {
                        Value = "razorpay",
                        Text = "Razor Pay"
                    });
                }
            }


            var _payfastSettings = _websettinghelper.GetWebsettingJson("PayFastSettings");
            if (_payfastSettings != null && !string.IsNullOrEmpty(_payfastSettings))
            {
                var json = JsonConvert.DeserializeObject<PayFastSettingsModel>(_payfastSettings);

                if (json != null && json.IsEnable)
                {
                    paymentgatewaylist.Add(new SelectListItem
                    {
                        Value = "payfast",
                        Text = "PayFast"
                    });
                }
            }


            var _paystackSettings = _websettinghelper.GetWebsettingJson("PayStackSettings");

            if (_paystackSettings != null && !string.IsNullOrEmpty(_paystackSettings))
            {
                var json = JsonConvert.DeserializeObject<PayStackSettingsModel>(_paystackSettings);

                if (json != null && json.IsEnable)
                {
                    paymentgatewaylist.Add(new SelectListItem
                    {
                        Value = "paystack",
                        Text = "Paystack"
                    });
                }
            }
            
            
            var _dpoSettings = _websettinghelper.GetWebsettingJson("DPOSettings");
            if (_dpoSettings != null && !string.IsNullOrEmpty(_dpoSettings))
            {
                var json = JsonConvert.DeserializeObject<DPOSettingsModel>(_dpoSettings);

                if (json != null && json.IsEnable)
                {
                    paymentgatewaylist.Add(new SelectListItem
                    {
                        Value = "dpo",
                        Text = "DPO"
                    });
                }
            }




            var _sslcommerzSettings = _websettinghelper.GetWebsettingJson("SSLCommerzSettings");
            if (_sslcommerzSettings != null && !string.IsNullOrEmpty(_sslcommerzSettings))
            {
                var json = JsonConvert.DeserializeObject<SSLCommerzSettingsModel>(_sslcommerzSettings);

                if (json != null && json.IsEnable)
                {
                    paymentgatewaylist.Add(new SelectListItem
                    {
                        Value = "sslcommerz",
                        Text = "SslCommerz"
                    });
                }
            }


            var _mtnSettings = _websettinghelper.GetWebsettingJson("MTNSettings");
            if (_mtnSettings != null && !string.IsNullOrEmpty(_mtnSettings))
            {
                var json = JsonConvert.DeserializeObject<MTNSettingsModel>(_mtnSettings);

                if (json != null && json.IsEnable)
                {
                    paymentgatewaylist.Add(new SelectListItem
                    {
                        Value = "mtn",
                        Text = "MTN"
                    });
                }
            }

            ///available wallet





            if (User.Identity.IsAuthenticated)
            {

                loginid = int.Parse(User.FindFirst("UserID")?.Value);

                AvailableWallet = _userhelper.WalletAvailable(loginid);

                // continue with loginid variable
            }



        }

        public IActionResult OnPost(string selectedPaymentGateway, string selectedPaymentCurrency, string selectedPaymentAmount)
        {
            try
            {

                Guid  ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value);
                // This is important on the response page. Check if it exists, then only allow execution; otherwise, do not allow.
                TempData["PaymentResponse"] = "Exist";

                if (RouteData.Values["ID"] != null)
                {
                    ID = (string)RouteData.Values["ID"];  //invoice number
                }

                if (RouteData.Values["MembershipType"] != null)
                {
                    MembershipType = (string)RouteData.Values["MembershipType"];
                }

                ///in case of 0 value after discount or wallet so just update the order
                ///
                if((decimal.Parse(selectedPaymentAmount) ==0))
                {
                    //loginid = int.Parse(User.FindFirst("UserID")?.Value);
                    //string orderstatus = _orderhelper.OrderPaymentUpdate("wallet", "paid", "wallet", string.Empty, selectedPaymentAmount, selectedPaymentCurrency, "", ID, loginid, "processing");

                    string url = $"/Payment/response?membershiptype={MembershipType}&id={ID}&invoicenumber={ID}&paymentmethod=wallet&wallet_id={Guid.NewGuid()}&currency={selectedPaymentCurrency}&amount={selectedPaymentAmount}";




                    return Redirect(url);

                }
                if (selectedPaymentGateway == "paypal")
                {
                   
                    string requestUrl = _paymentgatewayhelper.ProcessPayPal(ID, "Purchased", selectedPaymentCurrency, selectedPaymentAmount, MembershipType);

                    if (!string.IsNullOrEmpty(requestUrl))
                    {
                        if (!string.IsNullOrEmpty(requestUrl))
                        {
                            return Redirect(requestUrl);
                        }
                    }
                }

                else if(selectedPaymentGateway == "stripe")
                {
                    string requestUrl = _paymentgatewayhelper.ProcessStripe(ID, "Purchased", selectedPaymentCurrency, selectedPaymentAmount, MembershipType);

                    if (!string.IsNullOrEmpty(requestUrl))
                    {
                        return Redirect(requestUrl);
                    }
                }

                else if (selectedPaymentGateway == "razorpay")
                {
                    RazorPayOptionsModel razorPayOptions = _paymentgatewayhelper.ProcessRazorPay(ID, "Purchased", selectedPaymentCurrency, selectedPaymentAmount, MembershipType, ProfileGUID.ToString());

                    if (razorPayOptions!=null)
                    {
                        //return Redirect(requestUrl);

                        return Partial("/Pages/Payment/_RazorPay.cshtml", razorPayOptions);
                    }
                }

                else if (selectedPaymentGateway == "sslcommerz")
                {
                    string requestUrl = _paymentgatewayhelper.ProcessSSLCommerz(ID, "Purchased", selectedPaymentCurrency, selectedPaymentAmount, MembershipType);

                    if (!string.IsNullOrEmpty(requestUrl))
                    {
                        
                        return Redirect(requestUrl);
                    }
                }

                else if (selectedPaymentGateway == "mtn")
                {
                    ///this should be call parital view so buyer can type phone number
                    //return Partial("/Pages/Payment/mtnpaymentgateway/_mtnbuyerphone.cshtml");
                    //return Redirect("~/payment/mtn");

                   
                }

                else if (selectedPaymentGateway == "cod")
                {

                    string url = $"/Payment/response?membershiptype={MembershipType}&id={ID}&invoicenumber={ID}&paymentmethod=cod&cod_id={Guid.NewGuid()}&currency={selectedPaymentCurrency}&amount={selectedPaymentAmount}";

                    


                    return Redirect(url);
                }


                
                // If the selected payment gateway is not "paypal" or the request URL is null or empty, return the current page.
                return Page();
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Error on payment gateway");
                // Redirect to an error page with appropriate error message.
                return Redirect("/Error?Title=Payment Error&Message=Payment cannot be processed.&Body="+ ex.Message);
            }
        }
    
    
       
    
    }
}
