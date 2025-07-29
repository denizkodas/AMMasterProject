using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using AMMasterProject.ViewModel;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using Razorpay.Api;
using Stripe;
using Stripe.Checkout;
using ZXing;
using static AMMasterProject.Helpers.PaymentGatewayHelper;



namespace AMMasterProject.Helpers
{
    public class PaymentGatewayHelper
    {
        private readonly IHttpClientFactory _clientFactory;

        #region Model


        private readonly MyDbContext _dbContext;

        private readonly WebsettingHelper _websettinghelper;
        private readonly OrderHelper _orderhelper;
        private readonly UserHelper _userHelper;
        private readonly HttpClient _client;

        protected string FullName;
        protected string Email;
        protected string Phone;

        #endregion

        #region DI


        public PaymentGatewayHelper(MyDbContext context, WebsettingHelper websettinghelper, OrderHelper orderhelper, UserHelper userHelper, HttpClient client, IHttpClientFactory clientFactory)
        {
            _dbContext = context;
            _client = client;
            _websettinghelper = websettinghelper;
            _orderhelper = orderhelper;
            _userHelper = userHelper;
            _clientFactory = clientFactory;
        }
        #endregion


        public void getOrderandUserData(string invoicenumber, string custom)
        {
            var orderModel = _orderhelper.GetOrdersItem("shipping", 0, invoicenumber).FirstOrDefault();
            if (orderModel != null)
            {
                this.FullName = orderModel.ShippingDetailMetaData.FullName;
                this.Email = orderModel.ShippingDetailMetaData.ShippingEmail;
                this.Phone = orderModel.ShippingDetailMetaData.ShippingPhone;
            }

            else
            {
                var userModel = _userHelper.UserGeneralByGUID(Guid.Parse(custom));

                this.FullName = userModel.Displayname;
                this.Email = userModel.Email;
                this.Phone = userModel.Contact;
            }

        }

            
        

        #region Paypal


        #region Process
        public string ProcessPayPal(string id, string name, string currency, string pricing, string membershiptype)
        {
            var createdPayment = CreatePayment(id, name, currency, pricing, membershiptype);
            var links = createdPayment.links.GetEnumerator();
            string paypalRedirectUrl = null;
            while (links.MoveNext())
            {
                Links lnk = links.Current;
                if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                {
                    paypalRedirectUrl = lnk.href;
                    break;
                }
            }

            return paypalRedirectUrl;
        }

        #endregion


        #region CreatePayment


        private PayPal.Api.Payment CreatePayment(string id, string name, string currency, string pricing, string membershiptype)
        {
            APIContext apiContext = GetAPIContext();

            string host = GlobalHelper.GetCurrentDomainName();
            string returnurl = GlobalHelper.GetReturnURL();
            string invoicenumber = id; //GlobalHelper.GetInvoiceNumber(id, membershiptype);

            //var itemList = new ItemList()
            //{
            //    items = new List<Item>()
            //    {
            //        new Item()
            //        {
            //            name = name,
            //            currency = currency,
            //            price = pricing,
            //            quantity = "1",
            //            sku = id
            //        }
            //    }
            //};

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            var redirectUrls = new RedirectUrls()
            {

                return_url = host + "/Payment/response?membershiptype=" + membershiptype + "&id=" + id + "&invoicenumber=" + invoicenumber + "&paymentmethod=paypal",
                cancel_url = host + "/Payment/failed?membershiptype=" + membershiptype + "&id=" + id + "&invoicenumber=" + invoicenumber + "&returnurl=" + returnurl


            };

            //var notification = new Notification()
            //{
            //    send_to_merchant = true,
            //    cc_emails = GlobalHelper.AdminEmails("order"), // Add the list of email addresses here
            //    subject = "Purchase " + membershiptype + " on " + host,
            //    note = "Invoice Number: " + invoicenumber + " Price: " + currency + pricing
            //};
            //var details = new Details()
            //{
            //    tax = "0",
            //    shipping = "0",
            //    subtotal = pricing
            //};

            var amount = new Amount()
            {
                currency = currency,
                total = pricing,
                //details = details
            };

            var transactionList = new List<Transaction>()
            {
                new Transaction()
                {
                    description = "Purchase " + membershiptype + " on " + host,
                    invoice_number = invoicenumber,
                    amount = amount,
                    //item_list = itemList
                }
            };

            var payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirectUrls,


            };

            return payment.Create(apiContext);
        }
        #endregion


        #region PostPaymentExecution


        public Dictionary<string, object> ExecutePayment(string paymentId, string PayerID)
        {
            try
            {



                APIContext apiContext = GetAPIContext();

                var paymentExecution = new PaymentExecution()
                {
                    payer_id = PayerID
                };
                var payment = new PayPal.Api.Payment()
                {
                    id = paymentId
                };

                var executedPayment = payment.Execute(apiContext, paymentExecution);

                // Check the state of the executed payment
                if (executedPayment.state.ToLower() == "approved")
                {

                    var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "success" },
                        { "payerEmail", executedPayment.payer.payer_info.email },
                        { "amountPaid", executedPayment.transactions[0].amount.total },
                        { "Currency", executedPayment.transactions[0].amount.currency }

                    };

                    return result;


                }
                else
                {
                    var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "fail" },


                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "fail" },


                    };

                return result;
            }
        }

        #endregion

        #region PaypalConfigurations
        private Dictionary<string, string> GetConfig()
        {
            var config = new Dictionary<string, string>
            {


            { "mode", GetCredentials() },
            { "connectionTimeout", "360000" },
            { "requestRetries", "1" }
        };

            return config;
        }

        private string GetCredentials()
        {
            var paypalSettingsJson = _websettinghelper.GetWebsettingJson("PaypalSettings");
            if (!string.IsNullOrEmpty(paypalSettingsJson))
            {
                var paypalSettings = JsonConvert.DeserializeObject<PaypalSettingsModel>(paypalSettingsJson);
                if (paypalSettings != null)
                {
                    return paypalSettings.Environment;
                }
            }

            // Default value if settings are not available
            return "sandbox";
        }

        private string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(GetClientId(), GetClientSecret(), GetConfig()).GetAccessToken();
            return accessToken;
        }

        private string GetClientId()
        {
            var paypalSettingsJson = _websettinghelper.GetWebsettingJson("PaypalSettings");
            if (!string.IsNullOrEmpty(paypalSettingsJson))
            {
                var paypalSettings = JsonConvert.DeserializeObject<PaypalSettingsModel>(paypalSettingsJson);
                if (paypalSettings != null)
                {
                    return paypalSettings.ClientId;
                }
            }

            // Default value if settings are not available
            return "YOUR_DEFAULT_CLIENT_ID";
        }

        private string GetClientSecret()
        {
            var paypalSettingsJson = _websettinghelper.GetWebsettingJson("PaypalSettings");
            if (!string.IsNullOrEmpty(paypalSettingsJson))
            {
                var paypalSettings = JsonConvert.DeserializeObject<PaypalSettingsModel>(paypalSettingsJson);
                if (paypalSettings != null)
                {
                    return paypalSettings.ClientSecretKey;
                }
            }

            // Default value if settings are not available
            return "YOUR_DEFAULT_CLIENT_SECRET";
        }

        public APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken())
            {
                Config = GetConfig()
            };
            return apiContext;
        }
        #endregion

        #endregion


        #region Stripe

        public string ProcessStripe(string id, string name, string currency, string pricing, string membershiptype)
        {
            StripeConfiguration.ApiKey = GetStripeID();
            string host = GlobalHelper.GetCurrentDomainName();
            string returnurl = GlobalHelper.GetReturnURL();
            string invoicenumber = id;// GlobalHelper.GetInvoiceNumber(id, membershiptype);



            decimal converfeees = decimal.Parse(pricing.ToString());

            int fees = Convert.ToInt32(converfeees * 100);
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {

                     UnitAmount = fees , // $20.00
                    Currency = currency,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = name,
                        Description ="Purchase " + membershiptype + " on " + host  + " . invoicenumber: "+invoicenumber

                    },

                },
                Quantity = 1
            }
                },
                Mode = "payment",
                SuccessUrl = host + "/Payment/response?membershiptype=" + membershiptype + "&id=" + id + "&invoicenumber=" + invoicenumber + "&paymentmethod=stripe" + "&session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = host + "/Payment/failed?membershiptype=" + membershiptype + "&id=" + id + "&invoicenumber=" + invoicenumber + "&returnurl=" + returnurl


            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session.Url;
        }


        //validate customer
        private string GetOrCreateCustomer(string email)
        {

            StripeConfiguration.ApiKey = GetStripeID();
            var customerService = new CustomerService();

            // Check if customer exists
            var existingCustomerList = customerService.List(new CustomerListOptions { Email = email });
            var existingCustomer = existingCustomerList.FirstOrDefault();

            if (existingCustomer != null)
            {
                // Customer already exists, return the customer ID
                string customerId = existingCustomer.Id;
                return customerId;
            }
            else
            {
                // Customer does not exist, create a new customer
                var customerOptions = new CustomerCreateOptions
                {
                    Email = email,
                    // Add any additional customer details as needed
                };

                var newCustomer = customerService.Create(customerOptions);

                // Return the newly created customer ID
                string customerId = newCustomer.Id;
                return customerId;
            }
        }


        public Dictionary<string, object> ExecutePaymentStripe(string paymentid)
        {

            try
            {

                StripeConfiguration.ApiKey = GetStripeID();
                var sessionService = new SessionService();
                Session session = sessionService.Get(paymentid);



                // Check the state of the executed payment
                if (session != null)
                {

                    var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "success" },
                        { "payerEmail", session.CustomerDetails.Email },
                        { "amountPaid", (session.AmountTotal / 100.0).ToString() },
                        { "Currency", session.Currency.ToUpper() },
                        { "payerid", GetOrCreateCustomer(session.CustomerDetails.Email) },
                         { "paymentid", session.PaymentIntentId }
                    };

                    return result;


                }
                else
                {
                    var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "fail-Stripe" },


                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "fail-Stripe" },


                    };

                return result;
            }
        }

        private string GetStripeID()
        {
            string secretkey = "sk_test_C1Poayubia8dVZGqVp5xdJOT00f7EKOTV5";


            var stripeSettingsJson = _websettinghelper.GetWebsettingJson("StripeSettings");
            if (!string.IsNullOrEmpty(stripeSettingsJson))
            {
                var stripeSettings = JsonConvert.DeserializeObject<StripeSettingsModel>(stripeSettingsJson);
                if (stripeSettings != null)
                {
                    secretkey = stripeSettings.APIKey;
                }
            }


            return secretkey;
        }
        #endregion


        #region SSLCommerz

        #region SSLCommerzDI
        protected List<String> key_list;
        protected String generated_hash;
        protected string error;

        protected string Store_ID;
        protected string Store_Pass;
        protected bool Store_Test_Mode;

        protected string SSLCz_URL = "https://securepay.sslcommerz.com/"; ///default is live
        protected string Submit_URL = "gwprocess/v4/api.php";
        protected string Validation_URL = "validator/api/validationserverAPI.php";
        protected string Checking_URL = "validator/api/merchantTransIDvalidationAPI.php";

        #endregion

        public string ProcessSSLCommerz(string id, string name, string currency, string pricing, string membershiptype)
        {
           
          

            string host = GlobalHelper.GetCurrentDomainName();
            string returnurl = GlobalHelper.GetReturnURL();
            string invoicenumber = id;// GlobalHelper.GetInvoiceNumber(id, membershiptype);
            string referenceid = "";


            //string SuccessUrl = host + "/controller/appsetting/SSLCommerz";
            string SuccessUrl = host + "/controller/paymentgateway/SSLCommerz?membershiptype=" + membershiptype + "&id=" + id + "&invoicenumber=" + invoicenumber + "&paymentmethod=sslcommerz" + "&ssl_commerzID=" + invoicenumber;
            string CancelUrl = host + "/controller/paymentgateway/SSLCommerz?membershiptype=" + membershiptype + "&id=" + id + "&invoicenumber=" + invoicenumber + "&returnurl=" + returnurl;

            string Description = "Purchase " + membershiptype + " on " + host + " . invoicenumber: " + invoicenumber;




            // CREATING LIST OF POST DATA
            NameValueCollection PostData = new NameValueCollection();
            PostData.Add("total_amount", pricing);
            PostData.Add("currency", currency);
            PostData.Add("tran_id", id);


            PostData.Add("product_profile", "general");
            PostData.Add("product_category", name);
            PostData.Add("product_name", Description);
            //Madatory Dynamic field

            //pass order id and get shipping details

            PostData.Add("cus_name", "ABC XY");
            PostData.Add("cus_email", "abc.xyz@example.com");
            PostData.Add("cus_add1", "Address Line One");
            PostData.Add("cus_add2", "Address Line Two");
            PostData.Add("cus_city", "Dhaka");
            PostData.Add("cus_postcode", "1000");
            PostData.Add("cus_country", "Bangladesh");
            PostData.Add("cus_phone", "0171111111");

            PostData.Add("shipping_method", "NO");
            PostData.Add("num_of_item", "1");






            // Modify the success_url to include sslcommerz_payment_id as a parameter
            PostData.Add("success_url", SuccessUrl);
            PostData.Add("fail_url", CancelUrl); // "Fail.aspx" page needs to be created
            PostData.Add("cancel_url", CancelUrl); // "Cancel.aspx" page needs to be created
                                                   // PostData.Add("ipn_url", baseUrl + "IPN.aspx"); // If IPN is implemented, provide the url;



            string urlredirection = InitiateSSLCommerzTransaction(PostData);

            return urlredirection;

        }


        #region TestOrLiveMode
        public void GetSSLCommerzStoreID() //Use true for sandbox, false for live.
        {
            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            




            var sslcommerzSettingsJson = _websettinghelper.GetWebsettingJson("SSLCommerzSettings");
            if (!string.IsNullOrEmpty(sslcommerzSettingsJson))
            {
                var sslcommmerzSettings = JsonConvert.DeserializeObject<SSLCommerzSettingsModel>(sslcommerzSettingsJson);
                if (sslcommmerzSettings != null)
                {
                    this.Store_ID = sslcommmerzSettings.StoreID;
                    this.Store_Pass = sslcommmerzSettings.StorePassword;


                    if (sslcommmerzSettings.Environment == "sandbox")
                    {
                        this.SSLCz_URL = "https://sandbox.sslcommerz.com/";
                    }
                }
            }



        }

        #endregion


        #region CreatePayment
        public string InitiateSSLCommerzTransaction(NameValueCollection PostData, bool GetGateWayList = false)
        {
            //get the store id and store password and determine the sandbox or production
            GetSSLCommerzStoreID();
            PostData.Add("store_id", this.Store_ID);
            PostData.Add("store_passwd", this.Store_Pass);
            string response = this.SendPost(PostData);

            try
            {
                SSLCommerzInitResponse resp = JsonConvert.DeserializeObject<SSLCommerzInitResponse>(response);

                if (resp.status == "SUCCESS")
                {
                    if (GetGateWayList)
                    {
                        // We will work on it!
                    }
                    else
                    {
                        Uri uri = new Uri(resp.redirectGatewayURL);
                        string sslId = HttpUtility.ParseQueryString(uri.Query).Get("ssl_id");

                        // Store the sslId in the Session variable


                        return resp.GatewayPageURL.ToString();
                    }
                }
                else
                {
                    throw new Exception("Unable to get data from SSLCommerz. Please contact your manager!" + resp.status  + " " + resp.failedreason  );
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }

            return response;
        }
        #endregion

        #region ExecutePayment
        public Dictionary<string, object> SSLCommerzPaymentVerification(string MerchantTrxID)
        {
            bool hash_verified = true; //this.ipn_hash_verify(req);
            if (hash_verified)
            {
                GetSSLCommerzStoreID();
                string json = string.Empty;

                string EncodedValID = System.Web.HttpUtility.UrlEncode(MerchantTrxID);
                string EncodedStoreID = System.Web.HttpUtility.UrlEncode(this.Store_ID);
                string EncodedStorePassword = System.Web.HttpUtility.UrlEncode(this.Store_Pass);

                string validate_url = this.SSLCz_URL + this.Checking_URL + "?tran_id=" + EncodedValID + "&store_id=" + EncodedStoreID + "&store_passwd=" + EncodedStorePassword + "&v=1&format=json";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(validate_url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(resStream))
                {
                    json = reader.ReadToEnd();
                }
                if (json != "")
                {

                    SSLCommerzTransactionValidationResponse resp = JsonConvert.DeserializeObject<SSLCommerzTransactionValidationResponse>(json);


                 

                    if (resp.element[0].status == "VALID" || resp.element[0].status == "VALIDATED")
                    {


                        //if (MerchantTrxCurrency == "BDT")
                        //{
                        //    if (MerchantTrxID == resp.tran_id && (Math.Abs(Convert.ToDecimal(MerchantTrxAmount) - Convert.ToDecimal(resp.amount)) < 1) && MerchantTrxCurrency == "BDT")
                        //    {
                        //        return true;
                        //    }
                        //    else
                        //    {
                        //        this.error = "Amount not matching";
                        //        return false;
                        //    }
                        //}
                        //else
                        //{
                        //    if (MerchantTrxID == resp.tran_id && (Math.Abs(Convert.ToDecimal(MerchantTrxAmount) - Convert.ToDecimal(resp.currency_amount)) < 1) && MerchantTrxCurrency == resp.currency_type)
                        //    {
                        //        return true;
                        //    }
                        //    else
                        //    {
                        //        this.error = "Currency Amount not matching";
                        //        return false;
                        //    }

                        //}

                        var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "success" },
                        { "payerEmail",  "" },
                        { "amountPaid", resp.element[0].amount},
                        { "Currency",  resp.element[0].currency },
                        { "payerid",  resp.element[0].val_id},
                         { "paymentid",  resp.element[0].val_id }
                    };
                        return result;
                    }
                    else
                    {

                        var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "fail-SSLCommerze This transaction is either expired or fails" },


                    };

                        return result;

                    }
                }
                else
                {
                   
                 

                    var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "fail-SSLCommerze Unable to get Transaction JSON status" },


                    };

                    return result;


                }
            }
            else
            {
              

                var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "fail-SSLCommerze  Unable to verify hash" },


                    };

                return result;

            }
        }

        #endregion

        #region SendandCreatePost
        protected string SendPost(NameValueCollection PostData)
        {
            //Console.WriteLine(this.SSLCz_URL + this.Submit_URL);
            string response = Post(this.SSLCz_URL + this.Submit_URL, PostData);
            return response;
        }
        protected static string Post(string uri, NameValueCollection PostData)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                response = client.UploadValues(uri, PostData);
            }
            return System.Text.Encoding.UTF8.GetString(response);
        }
        #endregion

        #region SSLCommerzHashKey
        /// <summary>
        /// SSLCommerz IPN Hash Verify method
        /// </summary>
        /// <param name="req"></param>
        /// <param name="pass"></param>
        /// <returns>Boolean - True or False</returns>
        public Boolean ipn_hash_verify(HttpRequest req)
        {

            // Check For verify_sign and verify_key parameters
            if (req.Form["verify_sign"] != "" && req.Form["verify_key"] != "")
            {
                // Get the verify key
                String verify_key = req.Form["verify_key"];
                if (verify_key != "")
                {

                    // Split key string by comma to make a list array
                    key_list = verify_key.Split(',').ToList<String>();

                    // Initiate a key value pair list array
                    List<KeyValuePair<String, String>> data_array = new List<KeyValuePair<string, string>>();

                    // Store key and value of post in a list
                    foreach (String k in key_list)
                    {
                        data_array.Add(new KeyValuePair<string, string>(k, req.Form[k]));
                    }

                    // Store Hashed Password in list
                    String hashed_pass = this.MD5(this.Store_Pass);
                    data_array.Add(new KeyValuePair<string, string>("store_passwd", hashed_pass));

                    // Sort Array
                    data_array.Sort(
                        delegate (KeyValuePair<string, string> pair1,
                        KeyValuePair<string, string> pair2)
                        {
                            return pair1.Key.CompareTo(pair2.Key);
                        }
                    );


                    // Concat and make String from array
                    String hash_string = "";
                    foreach (var kv in data_array)
                    {
                        hash_string += kv.Key + "=" + kv.Value + "&";
                    }
                    // Trim & from end of this string
                    hash_string = hash_string.TrimEnd('&');

                    // Make hash by hash_string and store
                    generated_hash = this.MD5(hash_string);

                    // Check if generated hash and verify_sign match or not
                    if (generated_hash == req.Form["verify_sign"])
                    {
                        return true; // Matched
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Make PHP like MD5 Hashing
        /// </summary>
        /// <param name="s"></param>
        /// <returns>md5 Hashed String</returns>
        public String MD5(String s)
        {
            byte[] asciiBytes = ASCIIEncoding.ASCII.GetBytes(s);
            byte[] hashedBytes = System.Security.Cryptography.MD5CryptoServiceProvider.Create().ComputeHash(asciiBytes);
            string hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hashedString;
        }
        #endregion




        #endregion

        #region RazorPay

        protected string RazorPayKeyID;
        protected string RazorPaySecretID;
        protected string RazorPayStoreName;
        //protected bool Store_Test_Mode;
        public RazorPayOptionsModel ProcessRazorPay(string id, string name, string currency, string pricing, string membershiptype, string UserGUid="")
        {

            GetRazorPayStoreID();

            string razorpayKey = this.RazorPayKeyID;
            string razorpaySecret = this.RazorPaySecretID;

            string host = GlobalHelper.GetCurrentDomainName();
            string returnurl = GlobalHelper.GetReturnURL();
            string invoicenumber = id;// GlobalHelper.GetInvoiceNumber(id, membershiptype);

            try
            {
             
                string notes = $"Purchase {membershiptype} on  {host} invoicenumber: {invoicenumber}";
                RazorPayOrderModel order = new RazorPayOrderModel()
                {
                    OrderAmount =decimal.Parse(pricing),
                    Currency = currency,
                    Payment_Capture = 1,    // 0 - Manual capture, 1 - Auto capture
                    Notes = new Dictionary<string, string>()
                {
                    { "note 1", "Purchase" },

                    }
                };

                
                RazorpayClient client = new RazorpayClient(razorpayKey, razorpaySecret);
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", order.OrderAmountInSubUnits);
                options.Add("currency", order.Currency);
                options.Add("payment_capture", order.Payment_Capture);
                options.Add("notes", order.Notes);

                Razorpay.Api.Order orderResponse = client.Order.Create(options);
                var orderId = orderResponse.Attributes["id"].ToString();



                string SuccessUrl = host + "/controller/paymentgateway/Razorypaysuccess?membershiptype=" + membershiptype + "&id=" + id + "&invoicenumber=" + invoicenumber + "&paymentmethod=razorpay";
                string CancelUrl = host + "/controller/paymentgateway/Razorypaycancel?membershiptype=" + membershiptype + "&id=" + id + "&invoicenumber=" + invoicenumber + "&returnurl=" + returnurl;


                //var orderId = CreateTransfersViaOrder(order);

                //order moel



                ///call orderand userdata
                getOrderandUserData(invoicenumber, UserGUid);//profile guid

                RazorPayOptionsModel razorPayOptions = new RazorPayOptionsModel()
                {
                    Key = razorpayKey,
                    AmountInSubUnits = order.OrderAmountInSubUnits,
                    Currency = order.Currency,
                    Name = name,
                    Description = notes,
                    ImageLogUrl = "",
                    OrderId = orderId,
                    ProfileName = this.FullName,
                    ProfileContact = this.Phone,
                    ProfileEmail = this.Email,
                    SuccessURL = SuccessUrl,
                    CancelURL = CancelUrl,
                    Notes = new Dictionary<string, string>()
                {
                    { "note 1", "" }, { "note 2", "" }
                }
                };

                return  razorPayOptions;

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to get data from Razorpay. Please contact your manager!");
            }
        }


        public Dictionary<string, object> ExecuteRazorPay(string paymentid)//actually razorpayorderid not payment id
        {

            try
            {

                GetRazorPayStoreID();


                RazorpayClient client = new RazorpayClient(this.RazorPayKeyID, this.RazorPaySecretID);

                string orderId = paymentid;

                List<Razorpay.Api.Payment> payments = client.Order.Fetch(orderId).Payments();

                Razorpay.Api.Payment firstPayment = payments[0];

                var amountInPaise = Convert.ToInt32(firstPayment["amount"]);

                // Convert amount to decimal and divide by 100 to get decimal places
                decimal amountInRupees = amountInPaise / 100.0M;

                // Check the state of the executed payment
                if (payments.Count > 0)
                {
                   
                    var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "success" },
                        { "payerEmail", firstPayment["email"] },
                        { "amountPaid", amountInRupees },
                        { "Currency", firstPayment["currency"] },
                        { "payerid", orderId },
                         { "paymentid", firstPayment["id"] }


                    };

                    return result;


                }
                else
                {
                    var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "fail-razorpay" },


                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = new Dictionary<string, object>
                    {
                        { "executedstatus", "fail-razorpay" },


                    };

                return result;
            }
        }

        public void GetRazorPayStoreID() //Use true for sandbox, false for live.
        {
            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;






            var razorpaySettingsJson = _websettinghelper.GetWebsettingJson("RazorPaySettings");
            if (!string.IsNullOrEmpty(razorpaySettingsJson))
            {
                var razorpaySettings = JsonConvert.DeserializeObject<RazorPaySettingsModel>(razorpaySettingsJson);
                if (razorpaySettings != null)
                {
                    this.RazorPayKeyID = razorpaySettings.KeyID;
                    this.RazorPaySecretID = razorpaySettings.SecretID;
                    this.RazorPayStoreName = razorpaySettings.StoreName;




                }
            }



        }
        #endregion


        #region MTNMobile

        //public async Task<string> MTNAccessToken()
        //{
        //    var client = _clientFactory.CreateClient();
        //    var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://sandbox.momodeveloper.mtn.com/collection/token/");
        //    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("6cc2df85-ac39-4026-bb04-ea756050ad00:8de39d7424ed4476af56d70f3e9ac785"));
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        //    request.Headers.Add("Ocp-Apim-Subscription-Key", "db36413e69564a3aa1b90214df5bf038");

        //    var response = await client.SendAsync(request);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        try
        //        {
        //            var tokenData = JObject.Parse(content);
        //            return tokenData["access_token"]?.ToString();
        //        }
        //        catch (JsonException)
        //        {
        //            return null; // Handle JSON parsing errors
        //        }
        //    }
        //    return null; // Return null if the token retrieval failed
        //}

        protected string basekey;
        protected string apiurl;
        protected string subscriptionkey;
        protected string environment;

        public void GetMTNMobileStoreID() //Use true for sandbox, false for live.
        {
            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;






            var mtnSettingsJson = _websettinghelper.GetWebsettingJson("MTNSettings");
            if (!string.IsNullOrEmpty(mtnSettingsJson))
            {
                var mtnmobileSettings = JsonConvert.DeserializeObject<MTNSettingsModel>(mtnSettingsJson);
                if (mtnmobileSettings != null)
                {
                    this.basekey = mtnmobileSettings.BaseKey;
                    this.apiurl = mtnmobileSettings.APIURL;
                    this.subscriptionkey = mtnmobileSettings.OcpApimSubscriptionKey;
                    this.environment = mtnmobileSettings.Environment;



                }
            }



        }
        public string MTNAccessToken()
        {
            GetMTNMobileStoreID();
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, this.apiurl +"/collection/token/");
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(this.basekey));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                request.Headers.Add("Ocp-Apim-Subscription-Key", this.subscriptionkey);

                var response = client.SendAsync(request).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult(); // Block here
                    try
                    {
                        var tokenData = JObject.Parse(content);
                        return tokenData["access_token"]?.ToString();
                    }
                    catch (JsonException)
                    {
                        return null; // Handle JSON parsing errors
                    }
                }
                return null; // Return null if the token retrieval failed
            }
        }

        public object RequestToPay(string partyIdType, string partyId, string amount, string currency, string orderid)
        {
            GetMTNMobileStoreID();
            string accessToken = MTNAccessToken(); // Synchronously get the access token
            if (string.IsNullOrEmpty(accessToken))
            {
                return JsonConvert.SerializeObject(new { status = 500, message = "Failed to obtain access token" });
            }

            var client = _clientFactory.CreateClient();
            var referenceId = Guid.NewGuid();
             GlobalHelper.SetCookie("MTMReferenceID", referenceId.ToString());

            var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, this.apiurl +"/collection/v1_0/requesttopay")
            {
                Content = new StringContent(
                    $@"{{
                ""amount"": ""{amount}"",
                ""currency"": ""{currency}"",
                ""externalId"": ""{orderid}"",
                ""payer"": {{
                    ""partyIdType"": ""{partyIdType}"",
                    ""partyId"": ""{partyId}""
                }},
                ""payerMessage"": ""This is it"",
                ""payeeNote"": ""Thank you for your business""
            }}", Encoding.UTF8, "application/json")
            };

            // Properly add headers
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("X-Reference-Id", referenceId.ToString());
            request.Headers.Add("X-Target-Environment", this.environment);
            request.Headers.Add("Ocp-Apim-Subscription-Key", this.subscriptionkey);

            var response = client.SendAsync(request).GetAwaiter().GetResult(); // Synchronously send the HTTP request
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.SerializeObject(new { status = response.StatusCode, data = content });
            }
            else
            {
                var errorMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return JsonConvert.SerializeObject(new { status = response.StatusCode, message = errorMessage });
            }
        }


        public MTNTransactionValidationResult ValidateTransaction()
        {
            string accessToken = MTNAccessToken(); // Synchronously get the access token
            if (string.IsNullOrEmpty(accessToken))
            {
                return new MTNTransactionValidationResult
                {
                    StatusCode = 500,
                    Message = "Failed to obtain access token"
                };
            }

            string referenceidFromSession = GlobalHelper.ReadCookie("MTMReferenceID");
            if (string.IsNullOrEmpty(referenceidFromSession))
            {
                return new MTNTransactionValidationResult
                {
                    StatusCode = 404,
                    Message = "Reference ID not found in session"
                };
            }
            GetMTNMobileStoreID();
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, $"{this.apiurl}/collection/v1_0/requesttopay/{referenceidFromSession}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("X-Target-Environment", this.environment);
            request.Headers.Add("Ocp-Apim-Subscription-Key", this.subscriptionkey);

            var response = client.SendAsync(request).GetAwaiter().GetResult(); // Synchronously send the HTTP request
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                JObject jsonResponse = JObject.Parse(content);

                // Here, extracting the needed information and providing a result
                return new MTNTransactionValidationResult
                {
                    StatusCode = 200,
                    Data = content,
                    FinancialTransactionId = jsonResponse["financialTransactionId"]?.ToString(),
                    ExternalId = jsonResponse["externalId"]?.ToString(),
                    Amount = jsonResponse["amount"]?.ToString(),
                    Currency = jsonResponse["currency"]?.ToString(),
                    PayerPartyIdType = jsonResponse["payer"]["partyIdType"]?.ToString(),
                    PayerPartyId = jsonResponse["payer"]["partyId"]?.ToString(),
                    PayerMessage = jsonResponse["payerMessage"]?.ToString(),
                    PayeeNote = jsonResponse["payeeNote"]?.ToString(),
                    Status = jsonResponse["status"]?.ToString()
                };
            }
            else
            {
                return new MTNTransactionValidationResult
                {
                    StatusCode = (int)response.StatusCode,
                    Message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
                };
            }
        }
        public Dictionary<string, object> ExecuteMTNMobile(string paymentid)
        {
            try
            {
                var accessToken = MTNAccessToken(); // Get the token asynchronously
                if (string.IsNullOrEmpty(accessToken))
                {
                    return new Dictionary<string, object> { { "executedstatus", "fail-token" } }; // Handle missing access token
                }

                  

                var response = ValidateTransaction();

                if (response.StatusCode==200)
                {
                    

                 

                    return new Dictionary<string, object>
            {
                { "executedstatus", response.Status.ToLower() == "successful" ? "success" : "fail-mtnmobile" },
                { "payerEmail", response.PayerPartyId },
                { "amountPaid", response.Amount },
                { "Currency", response.Currency },
                { "payerid", response.PayerPartyId },
                { "paymentid", response.FinancialTransactionId }
            };
                }
                else
                {
                    return new Dictionary<string, object> { { "executedstatus", "fail-response" } }; // Handle unsuccessful HTTP response
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return new Dictionary<string, object> { { "executedstatus", "exception" } }; // Provide exception details
            }
        }



        #endregion



    }


}
