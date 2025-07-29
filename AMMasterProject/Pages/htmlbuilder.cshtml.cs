using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Razorpay.Api;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AMMasterProject.Pages
{
    public class htmlbuilderModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

      

        public string OrderID { get; set; }
        public string currentdomain { get; set; }
        public htmlbuilderModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            
        }

       
        //public async Task<IActionResult> OnGetAsync()
        //{
        //    OrderID = await GenerateOrderIdAsync();
        //    currentdomain = GlobalHelper.GetCurrentDomainName();

        //    // Pass OrderID to the partial view
        //    ViewData["OrderID"] = OrderID;

        //    CreateOrder();

        //    return Page();


        //    //return Partial("/Pages/Payment/_RazorPay.cshtml", ViewData);
        //}

        //public IActionResult OnGet()
        //{
        //    string razorpayKey = "rzp_test_YuSihFErCSA3wH";
        //    string razorpaySecret = "I9w5szkHgPOdNIMBY9CjB2dc";

        //    try
        //    {


        //        RazorPayOrderModel order = new RazorPayOrderModel()
        //        {
        //            OrderAmount = 1200,
        //            Currency = "INR",
        //            Payment_Capture = 1,    // 0 - Manual capture, 1 - Auto capture
        //            Notes = new Dictionary<string, string>()
        //        {
        //            { "note 1", "first note while creating order" }, { "note 2", "you can add max 15 notes" },
        //            { "note for account 1", "this is a linked note for account 1" }, { "note 2 for second transfer", "it's another note for 2nd account" }
        //        }
        //        };


        //        RazorpayClient client = new RazorpayClient(razorpayKey, razorpaySecret);
        //        Dictionary<string, object> options = new Dictionary<string, object>();
        //        options.Add("amount", order.OrderAmountInSubUnits);
        //        options.Add("currency", order.Currency);
        //        options.Add("payment_capture", order.Payment_Capture);
        //        options.Add("notes", order.Notes);

        //        Order orderResponse = client.Order.Create(options);
        //        var orderId = orderResponse.Attributes["id"].ToString();





        //        //var orderId = CreateTransfersViaOrder(order);

        //        RazorPayOptionsModel razorPayOptions = new RazorPayOptionsModel()
        //        {
        //            Key = razorpayKey,
        //            AmountInSubUnits = order.OrderAmountInSubUnits,
        //            Currency = order.Currency,
        //            Name = "Skynet",
        //            Description = "for Dotnet",
        //            ImageLogUrl = "",
        //            OrderId = orderId,
        //            ProfileName = "Gaura",
        //            ProfileContact = "9123456780",
        //            ProfileEmail = "abc@gmail.com",
        //            SuccessURL = $"{GlobalHelper.GetCurrentDomainName()}/controller/PaymentGateway/Razorypaysuccess",
        //            CancelURL = $"{GlobalHelper.GetCurrentDomainName()}/controller/PaymentGateway/Razorypaycancel",
        //            Notes = new Dictionary<string, string>()
        //        {
        //            { "note 1", "" }, { "note 2", "" }
        //        }
        //        };

        //        return Partial("/Pages/Payment/_RazorPay.cshtml", razorPayOptions);

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

  

      

        private async Task<string> GenerateOrderIdAsync()
        {



            // Your Razorpay API key and secret
            string razorpayKey = "rzp_test_YuSihFErCSA3wH";
            string razorpaySecret = "I9w5szkHgPOdNIMBY9CjB2dc";

            // Create an instance of HttpClient
            var httpClient = _httpClientFactory.CreateClient();

            // Set up the Razorpay API endpoint
            string razorpayApiUrl = "https://api.razorpay.com/v1/orders";

            // Construct the request payload
            var orderRequest = new Dictionary<string, object>
            {
                { "amount", 5000 },
                { "currency", "INR" },
                { "receipt", "receipt#1" },
                // Add any other required parameters
            };

            // Convert the request payload to JSON
            string jsonPayload = JsonSerializer.Serialize(orderRequest);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Set up basic authentication with your Razorpay key and secret
            var base64Auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{razorpayKey}:{razorpaySecret}"));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Auth);

            // Make the POST request to Razorpay API
            var response = await httpClient.PostAsync(razorpayApiUrl, content);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Parse the response JSON to get the order ID
                var responseContent = await response.Content.ReadAsStringAsync();
                var orderDetails = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                var orderId = orderDetails["id"].ToString();

                // Pass the order ID to the JavaScript function
                OrderID = orderId;

                return OrderID;
            }
            else
            {
                // Handle the case where the order creation failed
                // You might want to log the error or take appropriate action
                return OrderID;
            }
        }

        //private async Task SubmitRazorpayFormAsync()
        //{
        //    // Construct the form data
        //    var formData = new Dictionary<string, string>
        //{
        //    { "key_id", "rzp_test_YuSihFErCSA3wH" },
        //    { "amount", "1001" },
        //    { "order_id", OrderID },
        //    { "name", "AM Technology" },
        //    { "description", "A Wild Sheep Chase" },
        //     { "callback_url", "https://localhost:7009/controller/appsetting/Razorypay" },
        //       { "cancel_url", "https://localhost:7009/controller/appsetting/Razorypay" },
        //    // Add other form fields
        //};

        //    // Create an instance of HttpClient
        //    using (var httpClient = _httpClientFactory.CreateClient())
        //    {
        //        // Set the content type
        //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        //        // Make the POST request to Razorpay API
        //        var response = await httpClient.PostAsync("https://api.razorpay.com/v1/checkout/embedded", new FormUrlEncodedContent(formData));

        //        // Check if the request was successful
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            // Handle the case where the form submission failed
        //            // You might want to log the error or take appropriate action
        //            // ...
        //        }
        //        else
        //        {
        //            Response.Redirect("https://api.razorpay.com/v1/checkout/embedded");
        //        }
        //    }
        //}
     
        //public void OnGet()
        //{
        //}

        
    }
}
