using AMMasterProject.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using Razorpay.Api;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AMMasterProject.Controllers
{
    [Route("controller/[controller]/{action}")]
    [Controller]
    public class PaymentGatewayController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IHttpClientFactory _clientFactory;
        //}
        private readonly PaymentGatewayHelper _paymentgatewayhelper;
        public PaymentGatewayController(IHttpClientFactory clientFactory, PaymentGatewayHelper paymentGatewayHelper)
        {
            _clientFactory = clientFactory;
            _paymentgatewayhelper = paymentGatewayHelper;
        }
        #region RazorPay

        public IActionResult Razorypaysuccess()
        {
            if (!System.String.IsNullOrEmpty(Request.Form["razorpay_payment_id"]))
            {
                string url = "";
               // Razorpay will send payment details in the request body
               var razorpayPaymentId = Request.Form["razorpay_payment_id"];
                var razorpayOrderId = Request.Form["razorpay_order_id"];
                var razorpaySignature = Request.Form["razorpay_signature"];


                var validSignature = CompareSignatures(razorpayOrderId, razorpayPaymentId, razorpaySignature);
                if (validSignature)
                {
                    // Construct the success URL with parameters
                     url = $"/Payment/response?membershiptype={Request.Query["membershiptype"]}&id={Request.Query["id"]}&invoicenumber={Request.Query["invoicenumber"]}&paymentmethod=razorpay&razorpayorderid={razorpayOrderId}";

                }
                else
                {
                    url = $"/Payment/failed?membershiptype={Request.Query["membershiptype"]}&id={Request.Query["id"]}&invoicenumber={Request.Query["invoicenumber"]}&returnurl={Request.Query["returnurl"]}";

                }


                // Redirect the user to the success URL
                return Redirect(url);
            }
            else
            {
                // Redirect to the error page with a message
                //string errorMessage = "razorpay_payment_id not found in form return post invoice id = " + Request.Query["id"];
                string CancelUrl = $"/Payment/failed?membershiptype={Request.Query["membershiptype"]}&id={Request.Query["id"]}&invoicenumber={Request.Query["invoicenumber"]}&returnurl={Request.Query["returnurl"]}";

                return Redirect(CancelUrl);
            }
        }

        public IActionResult Razorypaycancel()
        {
           
                // Redirect to the error page with a message
                //string errorMessage = "razorpay_payment_id not found in form return post invoice id = " + Request.Query["id"];
                string CancelUrl = $"/Payment/failed?membershiptype={Request.Query["membershiptype"]}&id={Request.Query["id"]}&invoicenumber={Request.Query["invoicenumber"]}&returnurl={Request.Query["returnurl"]}";

                return Redirect(CancelUrl);
            
        }

        private bool CompareSignatures(string orderId, string paymentId, string razorPaySignature)
        {
            string razorpaySecret = "I9w5szkHgPOdNIMBY9CjB2dc";
            var text = orderId + "|" + paymentId;
            var secret = razorpaySecret;
            var generatedSignature = CalculateSHA256(text, secret);
            if (generatedSignature == razorPaySignature)
                return true;
            else
                return false;
        }

        private string CalculateSHA256(string text, string secret)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2BeHashed = enc.GetBytes(text),
            baSalt = enc.GetBytes(secret);
            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }


        #endregion

        #region SSLCommerz
        [HttpPost]
        public IActionResult SSLCommerz()
        {

            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x00000C00;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Razorpay will send payment details in the request body
            if (!System.String.IsNullOrEmpty(Request.Form["status"]) && Request.Form["status"] == "VALID")


            {
                string TrxID = Request.Form["tran_id"];
                // AMOUNT and Currency FROM DB FOR THIS TRANSACTION
                //string amount = "100.00";
                //string currency = "BDT";


                string SuccessUrl = "~/Payment/response?membershiptype=" + Request.Query["membershiptype"].ToString() + "&id=" + Request.Query["id"].ToString() + "&invoicenumber=" + Request.Query["invoicenumber"].ToString() + "&paymentmethod=sslcommerz" + "&ssl_commerzID=" + Request.Query["invoicenumber"].ToString();

                return Redirect(SuccessUrl);
                //SSLCommerz sslcz = new SSLCommerz("tezar65ab5dce86802", "tezar65ab5dce86802@ssl", true);
                //Response.Write("Validation Response: " + sslcz.OrderValidate(TrxID, amount, currency, Request));
            }
            else
            {
                // Redirect to the error page with a message
                string CancelUrl = $"~/Payment/failed?membershiptype={Request.Query["membershiptype"]}&id={Request.Query["id"]}&invoicenumber={Request.Query["invoicenumber"]}&returnurl={Request.Query["returnurl"]}";

                return Redirect(CancelUrl);
            }
        }
        #endregion


        #region MTN
        public IActionResult MTNBuyerPhoneView()
        {
          

            //ViewBag.MyString = myString;
            return PartialView("/Pages/payment/mtnpaymentgateway/_mtnbuyerphone.cshtml");
        }
        public IActionResult RequestToPay(string partyIdType, string partyId, string amount, string currency, string orderid)
        {
            // Call the helper method
            var jsonResponse = _paymentgatewayhelper.RequestToPay(partyIdType, partyId, amount, currency, orderid);

            // Deserialize the JSON response from the helper
            dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse.ToString());

            // Extract status and data/message based on the original helper method's response
            int status = responseObject.status;
            var data = responseObject.data != null ? responseObject.data : null;
            var message = responseObject.message != null ? responseObject.message : "";

            // Return a new JSON object with deserialized data
            if (data != null)
            {
                // If there is data, return it with status
                return Json(new { status = status, data = data });
            }
            else
            {
                // If there is no data, probably an error message exists
                return Json(new { status = status, message = message });
            }
        }
        public IActionResult mtntranscationvalidation(string membershiptype)
        {
            // Call the helper method
            var response = _paymentgatewayhelper.ValidateTransaction();

            if(response.Status.ToString().ToLower() =="successful")
            {
                string referenceid = GlobalHelper.ReadCookie("MTMReferenceID");
                string url = "/Payment/response?membershiptype=" + membershiptype + "&id=" + response.ExternalId.ToString() + "&invoicenumber=" + response.ExternalId.ToString() + "&paymentmethod=mtnmobile" + "&mtnmobileID=" + referenceid;
                return Json(new { status = "successful", returnurl=url });
            }
            //var response = _paymentgatewayhelper.RequestToPay(partyIdType, partyId, amount, currency, orderid);
            //ViewBag.MyString = myString;
            return Json(new { status = "fail" });
        }

        /// <summary>
        /// Refactor completed
        /// </summary>
        /// <returns></returns>
        //public async Task<string> MTNAccessToken()
        //{
        //    using (var client = _clientFactory.CreateClient())
        //    {
        //        var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://sandbox.momodeveloper.mtn.com/collection/token/");
        //        var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("6cc2df85-ac39-4026-bb04-ea756050ad00:8de39d7424ed4476af56d70f3e9ac785"));
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        //        request.Headers.Add("Ocp-Apim-Subscription-Key", "db36413e69564a3aa1b90214df5bf038");

        //        using (var response = await client.SendAsync(request))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var content = await response.Content.ReadAsStringAsync();
        //                try
        //                {
        //                    var tokenData = JObject.Parse(content);
        //                    return tokenData["access_token"]?.ToString();
        //                }
        //                catch (JsonException)
        //                {
        //                    return null; // Handle JSON parsing errors
        //                }
        //            }
        //            return null; // Return null if the token retrieval failed
        //        }
        //    }
        //}

        //    [HttpPost]
        //    public IActionResult RequestToPay(string partyIdType, string partyId, string amount, string currency, string orderid)
        //    {
        //        // Synchronously get the access token
        //        var accessTokenTask =_payment; // This is originally an async method
        //        var accessToken = accessTokenTask.GetAwaiter().GetResult(); // Using GetAwaiter().GetResult() to block and wait for result

        //        if (string.IsNullOrEmpty(accessToken))
        //        {
        //            return Json(new { status = StatusCodes.Status500InternalServerError, message = "Failed to obtain access token" });
        //        }

        //        var client = _clientFactory.CreateClient();
        //        var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, "https://sandbox.momodeveloper.mtn.com/collection/v1_0/requesttopay");
        //        var referenceid = Guid.NewGuid(); // Use orderid as the reference ID
        //        HttpContext.Session.SetString("MTMReferenceID", referenceid.ToString());
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //        request.Headers.Add("X-Reference-Id", referenceid.ToString());
        //        request.Headers.Add("X-Target-Environment", "sandbox");
        //        request.Headers.Add("Ocp-Apim-Subscription-Key", "db36413e69564a3aa1b90214df5bf038");

        //        // Create JSON payload with dynamic parameters
        //        string jsonBody = $@"{{
        //    ""amount"": ""{amount}"",
        //    ""currency"": ""{currency}"",
        //    ""externalId"": ""{orderid}"",
        //    ""payer"": {{
        //        ""partyIdType"": ""{partyIdType}"",
        //        ""partyId"": ""{partyId}""
        //    }},
        //    ""payerMessage"": ""This is it"",
        //    ""payeeNote"": ""Thank you for your business""
        //}}";
        //        request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //        // Synchronously send the HTTP request
        //        var responseTask = client.SendAsync(request);
        //        var response = responseTask.GetAwaiter().GetResult(); // Blocking here as well

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var contentTask = response.Content.ReadAsStringAsync();
        //            var content = contentTask.GetAwaiter().GetResult(); // Blocking to read content
        //            return Json(new { status = response.StatusCode, data = content });
        //        }
        //        else
        //        {
        //            var errorTask = response.Content.ReadAsStringAsync();
        //            var errorMessage = errorTask.GetAwaiter().GetResult(); // Blocking to read error message
        //            return Json(new { status = response.StatusCode, message = errorMessage });
        //        }
        //    }


        //    public async Task<IActionResult> mtntranscationvalidation(string membershiptype)
        //    {
        //        var accessToken = await MTNAccessToken(); // Get the token asynchronously
        //        if (string.IsNullOrEmpty(accessToken))
        //        {
        //            return Json(new { status = StatusCodes.Status500InternalServerError, message = "Failed to obtain access token" });
        //        }

        //        // Retrieve the reference ID from session
        //        string referenceidFromSession = HttpContext.Session.GetString("MTMReferenceID");
        //        if (string.IsNullOrEmpty(referenceidFromSession))
        //        {
        //            return Json(new { status = StatusCodes.Status404NotFound, message = "Reference ID not found in session" });
        //        }

        //        var client = _clientFactory.CreateClient();
        //        var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, $"https://sandbox.momodeveloper.mtn.com/collection/v1_0/requesttopay/{referenceidFromSession}");

        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //        request.Headers.Add("X-Target-Environment", "sandbox");
        //        request.Headers.Add("Ocp-Apim-Subscription-Key", "db36413e69564a3aa1b90214df5bf038");

        //        var response = await client.SendAsync(request);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Assuming responseContent has already been read from the response
        //            var responseContent = await response.Content.ReadAsStringAsync();
        //            var jsonResponse = JObject.Parse(responseContent); // Parse the JSON content into a JObject

        //            // Extracting individual values from the JSON object
        //            var financialTransactionId = jsonResponse["financialTransactionId"].ToString();
        //            var externalId = jsonResponse["externalId"].ToString();
        //            var amount = jsonResponse["amount"].ToString();
        //            var currency = jsonResponse["currency"].ToString();

        //            // Extracting payer information
        //            var payerPartyIdType = jsonResponse["payer"]["partyIdType"].ToString();
        //            var payerPartyId = jsonResponse["payer"]["partyId"].ToString();

        //            // Extracting messages and status
        //            var payerMessage = jsonResponse["payerMessage"].ToString();
        //            var payeeNote = jsonResponse["payeeNote"].ToString();
        //            var status = jsonResponse["status"].ToString();

        //            if (status.ToLower() =="successful")
        //            {
        //                //string host = GlobalHelper.GetCurrentDomainName();
        //                //string returnurl = GlobalHelper.GetReturnURL();
        //                //string invoicenumber = "";// GlobalHelper.GetInvoiceNumber(id, membershiptype);
        //                //string referenceid = referenceidFromSession;
        //                //string paidamount = "";
        //                //string paidcurrency = "";



        //                string url = "/Payment/response?membershiptype="+membershiptype + "&id=" + externalId.ToString() + "&invoicenumber=" + externalId.ToString() + "&paymentmethod=mtnmobile" + "&mtnmobileID=" + referenceidFromSession.ToString();


        //                //return Json(new { status = response.StatusCode, data = responseContent });

        //                //string SuccessUrl = host + "/controller/appsetting/SSLCommerz";
        //                //string url = $"/Payment/response?membershiptype={membershiptype}&id={invoicenumber}&invoicenumber={invoicenumber}&paymentmethod=mtnmobile&mtnmobile_id={referenceid}&currency={paidcurrency}&amount={paidamount}";


        //                return Json(new { status = status.ToString().ToLower(), returnurl = url });

        //                //return Redirect(url);


        //            }
        //            // Now return the status along with other data
        //            return Json(new { status = status, data = responseContent });
        //        }
        //        else
        //        {
        //            return Json(new { status = response.StatusCode, message = await response.Content.ReadAsStringAsync() });
        //        }
        //    }

        #endregion
    }
}
