using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace AMMasterProject.Pages.Payment.mtnpaymentgateway
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async void OnGet()
        {
            PerformTransaction();
        }

        #region MTN


        public async Task<IActionResult> PerformTransaction()
        {
            // First, obtain the token
            var tokenResponse = await GetToken();
            if (tokenResponse is OkObjectResult okResult)
            {
                // Extract the access token from the response
                var token = JObject.Parse(okResult.Value.ToString())["access_token"].ToString();
                // Now use this token to make the RequestToPay call
                return await RequestToPay(token);
            }
            return tokenResponse; // Return error if token retrieval failed
        }

        private async Task<IActionResult> GetToken()
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://sandbox.momodeveloper.mtn.com/collection/token/");

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes("494ea3a4-208b-4967-add9-eb32cca99196:fb01eada01564679befe6c900abdc320"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            request.Headers.Add("Ocp-Apim-Subscription-Key", "db36413e69564a3aa1b90214df5bf038");

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Content(content.ToString());
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        private async Task<IActionResult> RequestToPay(string accessToken)
        {
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://sandbox.momodeveloper.mtn.com/collection/v1_0/requesttopay");
            var referenceid = Guid.NewGuid().ToString();
            // Set the authorization and other headers
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //request.Headers.Add("X-Callback-Url", "https://ezycommerce.amtechnology.info/controller/payment/mtncallback");
            request.Headers.Add("X-Reference-Id", referenceid);
            request.Headers.Add("X-Target-Environment", "sandbox");
            request.Headers.Add("Ocp-Apim-Subscription-Key", "db36413e69564a3aa1b90214df5bf038");


            // Prepare the JSON payload
            string jsonBody = @"{
        ""amount"": ""100"",
        ""currency"": ""EUR"",
        ""externalId"": ""987654"",
        ""payer"": {
            ""partyIdType"": ""MSISDN"",
            ""partyId"": ""0774922487""
        },
        ""payerMessage"": ""This is it"",
        ""payeeNote"": ""This is it""
    }";

            // Set the content of the request including the Content-Type header
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Send the request
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Content(content.ToString());
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        #endregion
    }
}
