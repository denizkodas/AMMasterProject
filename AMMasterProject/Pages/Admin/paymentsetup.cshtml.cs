using Amazon.S3.Model.Internal.MarshallTransformations;
using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Stripe;

namespace AMMasterProject.Pages.Admin
{
    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class paymentsetupModel : PageModel
    {

        #region Model
        private readonly WebsettingHelper _websettinghelper;

        public CODSettingsModel cod { get; set; }

        public BankTransferSettingsModel banktransfer { get; set; }
        public PaypalSettingsModel paypal { get; set; }

        public StripeSettingsModel stripe { get; set; }

        public RazorPaySettingsModel razorpay { get; set; }


        public PayFastSettingsModel payfast { get; set; }


        public PayStackSettingsModel paystack { get; set; }


        public DPOSettingsModel dpo { get; set; }


        public SSLCommerzSettingsModel sslcommerz { get; set; }

        public MTNSettingsModel mtn { get; set; }
        

        #endregion

        #region DI



        public paymentsetupModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            paypal = new PaypalSettingsModel();
            stripe = new StripeSettingsModel();
            razorpay = new RazorPaySettingsModel();
            payfast = new PayFastSettingsModel();
            paystack = new PayStackSettingsModel();
            dpo = new DPOSettingsModel();
            mtn = new MTNSettingsModel();
        }


        #endregion
        public void OnGet()
        {
            ///cod
            ///
            var _codSettings = _websettinghelper.GetWebsettingJson("CODSettings");
            if (_codSettings != null && !string.IsNullOrEmpty(_codSettings))
            {
                var json = JsonConvert.DeserializeObject<CODSettingsModel>(_codSettings);

                if (json != null)
                {
                    cod = new CODSettingsModel
                    {
                        Message=json.Message,
                       
                        IsEnable = json.IsEnable
                    };

                }
            }


            //transfer
            ///cod
            ///
            var _transferSettings = _websettinghelper.GetWebsettingJson("BankTransferSettings");
            if (_transferSettings != null && !string.IsNullOrEmpty(_transferSettings))
            {
                var json = JsonConvert.DeserializeObject<BankTransferSettingsModel>(_transferSettings);

                if (json != null)
                {
                    banktransfer = new BankTransferSettingsModel
                    {
                      
                        AccountDetails = json.AccountDetails,
                        IsEnable = json.IsEnable
                    };

                }
            }


            ////Paypal


            var _paypalSettings = _websettinghelper.GetWebsettingJson("PaypalSettings");
            if (_paypalSettings != null && !string.IsNullOrEmpty(_paypalSettings))
            {
                var json = JsonConvert.DeserializeObject<PaypalSettingsModel>(_paypalSettings);

                if (json != null)
                {
                    paypal = new PaypalSettingsModel
                    {
                        ClientId = json.ClientId,
                        ClientSecretKey = json.ClientSecretKey,
                        Environment = json.Environment,
                        IsEnable = json.IsEnable
                    };

                }
            }




            ////RazorPay
            ///


            var _stripeSettings = _websettinghelper.GetWebsettingJson("StripeSettings");
            if (_stripeSettings != null && !string.IsNullOrEmpty(_stripeSettings))
            {
                var json = JsonConvert.DeserializeObject<StripeSettingsModel>(_stripeSettings);

                if (json != null)
                {
                    stripe = new StripeSettingsModel
                    {
                        APIKey = json.APIKey,
                        IsEnable = json.IsEnable


                    };

                }
            }




            ////RazorPay
            ///

            var _razorpaySettings = _websettinghelper.GetWebsettingJson("RazorPaySettings");
            if (_razorpaySettings != null && !string.IsNullOrEmpty(_razorpaySettings))
            {
                var json = JsonConvert.DeserializeObject<RazorPaySettingsModel>(_razorpaySettings);

                if (json != null)
                {
                    razorpay = new RazorPaySettingsModel
                    {
                        KeyID = json.KeyID,
                        SecretID = json.SecretID,
                        StoreName = json.StoreName,
                        Environment=json.Environment,
                        IsEnable = json.IsEnable

                    };

                }
            }



            ////PayFast
            ///

            var _payfastSettings = _websettinghelper.GetWebsettingJson("PayFastSettings");
            if (_payfastSettings != null && !string.IsNullOrEmpty(_payfastSettings))
            {
                var json = JsonConvert.DeserializeObject<PayFastSettingsModel>(_payfastSettings);

                if (json != null)
                {
                    payfast = new PayFastSettingsModel
                    {
                        MerchantID = json.MerchantID,
                        MerchantKey = json.MerchantKey,
                        IsEnable = json.IsEnable


                    };

                }
            }


            ////PayStack
            ///

            var _paystackSettings = _websettinghelper.GetWebsettingJson("PayStackSettings");
            if (_paystackSettings != null && !string.IsNullOrEmpty(_paystackSettings))
            {
                var json = JsonConvert.DeserializeObject<PayStackSettingsModel>(_paystackSettings);

                if (json != null)
                {
                    paystack = new PayStackSettingsModel
                    {
                        PKLive = json.PKLive,
                        IsEnable = json.IsEnable


                    };

                }
            }





            ////DPO
            ///

            var _dpoSettings = _websettinghelper.GetWebsettingJson("DPOSettings");
            if (_dpoSettings != null && !string.IsNullOrEmpty(_dpoSettings))
            {
                var json = JsonConvert.DeserializeObject<DPOSettingsModel>(_dpoSettings);

                if (json != null)
                {
                    dpo = new DPOSettingsModel
                    {
                        CompanyToken = json.CompanyToken,
                        ServiceType = json.ServiceType,
                        IsEnable = json.IsEnable

                    };

                }
            }


            var _sslcommerzSettings = _websettinghelper.GetWebsettingJson("SSLCommerzSettings");
            if (_sslcommerzSettings != null && !string.IsNullOrEmpty(_sslcommerzSettings))
            {
                var json = JsonConvert.DeserializeObject<SSLCommerzSettingsModel>(_sslcommerzSettings);

                if (json != null)
                {
                    sslcommerz = new SSLCommerzSettingsModel
                    {
                        StoreID = json.StoreID,
                        StorePassword = json.StorePassword,
                        Environment =json.Environment,
                        IsEnable = json.IsEnable

                    };

                }
            }


            var _mtnSettings = _websettinghelper.GetWebsettingJson("MTNSettings");
            if (_mtnSettings != null && !string.IsNullOrEmpty(_mtnSettings))
            {
                var json = JsonConvert.DeserializeObject<MTNSettingsModel>(_mtnSettings);

                if (json != null)
                {
                   mtn = new MTNSettingsModel
                    {
                       APIURL= json.APIURL,
                       BaseKey = json.BaseKey,
                       OcpApimSubscriptionKey=json.OcpApimSubscriptionKey,
                        Environment = json.Environment,
                        IsEnable = json.IsEnable

                    };

                }
            }

        }
        public IActionResult OnPostCOD()
        {


            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(cod);

            string msg = _websettinghelper.UpdateWebsettingJson("CODSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }

            if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }

            else
            {
                TempData["success"] = msg;
            }


            return Redirect("/admin/paymentsetup#codsetup");
        }

        public IActionResult OnPostBankTransfer()
        {


            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(banktransfer);

            string msg = _websettinghelper.UpdateWebsettingJson("BankTransferSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }

            if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }

            else
            {
                TempData["success"] = msg;
            }


            return Redirect("/admin/paymentsetup#transfersetup");
        }

        public IActionResult OnPostPaypal()
        {

       
            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(paypal);

            string msg = _websettinghelper.UpdateWebsettingJson("PaypalSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }

            if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }

            else
            {
                TempData["success"] = msg;
            }


            return Redirect("/admin/paymentsetup#paypalsetup");
        }


        public IActionResult OnPostStripe()
        {

           

            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(stripe);

            string msg = _websettinghelper.UpdateWebsettingJson("StripeSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }

            if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }

            else
            {
                TempData["success"] = msg;
            }



            return Redirect("/admin/paymentsetup#stripe");
        }

        public IActionResult OnPostRazorPay()
        {

          

            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(razorpay);

            string msg = _websettinghelper.UpdateWebsettingJson("RazorPaySettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
               
            }

            if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
                
            }

            else
            {
                TempData["success"] = "Updated Successfully";
            }


            return Redirect("/admin/paymentsetup#razorpay");
        }



        public IActionResult OnPostPayFast()
        {



            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(payfast);

            string msg = _websettinghelper.UpdateWebsettingJson("PayFastSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }

            if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }

            else
            {
                TempData["success"] = msg;
            }

            return Redirect("/admin/paymentsetup#payfast");
        }


        public IActionResult OnPostPayStack()
        {

          

            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(paystack);

            string msg = _websettinghelper.UpdateWebsettingJson("PayStackSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }

            if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }

            else
            {
                TempData["success"] = msg;
            }

            return Redirect("/admin/paymentsetup#paystack");
        }

        public IActionResult OnPostDPO()
        {

         

            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(dpo);

            string msg = _websettinghelper.UpdateWebsettingJson("DPOSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }

            if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }

            else
            {
                TempData["success"] = msg;
            }



            return Redirect("/admin/paymentsetup#dpo");
        }

        public IActionResult OnPostSSLCommerz()
        {



            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(sslcommerz);

            string msg = _websettinghelper.UpdateWebsettingJson("SSLCommerzSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }

            if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }

            else
            {
                TempData["success"] = msg;
            }



            return Redirect("/admin/paymentsetup#sslcommerz");
        }

        public IActionResult OnPostMTN()
        {



            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(mtn);

            string msg = _websettinghelper.UpdateWebsettingJson("MTNSettings", jsonData);

            if (msg == "insert")
            {
                TempData["success"] = "Inserted successfully";
            }

            if (msg == "update")
            {
                TempData["success"] = "Updated successfully";
            }

            else
            {
                TempData["success"] = msg;
            }



            return Redirect("/admin/paymentsetup#mtn");
        }
    }
}
