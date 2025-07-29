using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using AMMasterProject.Helpers;
using System.Net;
using RestSharp;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject.Controllers
{
    /// <summary>
    /// /any app setting method use here, like google map ap
    /// </summary>

    [Route("controller/[controller]/{action}")]
    [Controller]
    public class AppSettingController : Controller
    {
        private readonly MyDbContext _dbcontext;
        private readonly WebsettingHelper _websettinghelper;
        private readonly GlobalHelper _globalHelper;
        private readonly HttpClient _httpClient;
        public FireBaseSettingsModel firebase { get; set; }

        public LanguageSetupSettingsModel languagesetup { get; set; }


        public AppSettingController(WebsettingHelper websettinghelper, GlobalHelper globalHelper, MyDbContext dbcontext, IHttpClientFactory httpClientFactory)
        {
            _websettinghelper = websettinghelper;
            firebase = new FireBaseSettingsModel();
            _globalHelper = globalHelper;
            _dbcontext = dbcontext;
            _httpClient = httpClientFactory.CreateClient();
        }




        [HttpGet("{*url}")]
        public async Task<IActionResult> CountyCityState(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://www.amtechnology.info/{url}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            catch (HttpRequestException)
            {
                return StatusCode(500, "Error fetching data from the external server.");
            }
        }







        #region FireBase

        [HttpGet]
        public IActionResult firebasesettings()
        {
            var _firebaseSettings = _websettinghelper.GetWebsettingJson("FireBaseSettings");
            if (_firebaseSettings != null && !string.IsNullOrEmpty(_firebaseSettings))
            {
                var json = JsonConvert.DeserializeObject<FireBaseSettingsModel>(_firebaseSettings);

                if (json != null)
                {
                    firebase = new FireBaseSettingsModel
                    {
                        apiKey = json.apiKey,
                        authDomain = json.authDomain,
                        projectId = json.projectId,
                        storageBucket = json.storageBucket,
                        messagingSenderId = json.messagingSenderId,
                        appId = json.appId,
                        measurementId = json.measurementId,
                    };

                }
            }


            return Json(firebase);
        }
        #endregion


        #region GoogleMapAPI

        [HttpGet]
        public IActionResult getgooglemapi()
        {

            string googlemapapikey = _globalHelper.GetGoogleMapAPi();


            return Json(googlemapapikey);
        }
        #endregion

        #region Languagescript

        [HttpGet]
        public IActionResult languagesettings()
        {


            return Json(_globalHelper.languagesettings());
        }
        #endregion

        #region CountryList
        [HttpGet]
        public IActionResult countrylist()
        {
            var model = _globalHelper.GetCountryList().OrderBy(u => u.Name).Where(u => u.IsCountryPublish == true);

            return Json(model);
        }


        [HttpPost]
        public IActionResult UpdateCountry(int countryid)
        {
            CountryCode update = _dbcontext.Countries.FirstOrDefault(u => u.CountryID == countryid);

            if (update != null)
            {
                update.IsPublish = !update.IsPublish;

                _dbcontext.Update(update);
                _dbcontext.SaveChanges();

                return Json("success");
            }


            return NotFound();
        }
        #endregion


        #region CurrencyList
        [HttpGet]
        public IActionResult currencylist()
        {
            var model = _globalHelper.GetCurrencyList().Where(u => u.IsCurrencyPublish == true).ToList();

            return Json(model);
        }


        [HttpGet]
        public IActionResult currencybaseselection()
        {
            var model = _globalHelper.GetBaseCurrency();

            return Json(model);
        }

        [HttpPost]
        public IActionResult UpdateCurrency(int currencyid, decimal conversionrate)
        {
            CountryCode update = _dbcontext.Countries.FirstOrDefault(u => u.CountryID == currencyid);

            if (update != null)
            {
                update.IsCurrencyPublish = !update.IsCurrencyPublish;
                update.ConversionRate = conversionrate;
                update.ConversionUpdatedDate = DateTime.Now;

                _dbcontext.Update(update);
                _dbcontext.SaveChanges();

                return Json("success");
            }


            return NotFound();
        }

        #endregion


        #region LanguageList
        [HttpGet]
        public IActionResult languagelist()
        {
            var model = _globalHelper.GetLanguageList().Where(u => u.IsPublish == true).ToList();

            return Json(model);
        }


        [HttpGet]
        public IActionResult languagebaseselection()
        {
            var model = _globalHelper.GetBaseLanguage();

            return Json(model);
        }

        [HttpPost]
        public IActionResult UpdateLanguage(string languagecode)
        {
            var languageSetting = _dbcontext.Websettings.FirstOrDefault(u => u.WebsettingKey == "Language");
            var languageList = JsonConvert.DeserializeObject<List<LanguageViewModel>>(languageSetting.WebsettingValue);

            // Find the language in the list based on the provided language code
            var language = languageList.FirstOrDefault(l => l.Code == languagecode);

            if (language != null)
            {
                // Update the IsPublish property
                language.IsPublish = !language.IsPublish;

                // Serialize the updated language list
                var updatedLanguageSetting = JsonConvert.SerializeObject(languageList);

                // Update the database with the updated language list
                languageSetting.WebsettingValue = updatedLanguageSetting;
                _dbcontext.SaveChanges();

                return Json("success");
            }


            return NotFound();
        }
        #endregion


        #region JsonFileconversion
        //[HttpGet]
        //public IActionResult jsonconversion()
        //{
        //    string jsonFilePath = "D:\\Projects\\AMMasterProject\\curencycountrydata.json";
        //    string json = System.IO.File.ReadAllText(jsonFilePath);

        //    // Deserialize the JSON array into a JArray
        //    var jsonArray = JArray.Parse(json);

        //    // Create a new JArray for the desired JSON structure
        //    var desiredJsonArray = new JArray();
        //    foreach (var jsonObject in jsonArray)
        //    {
        //        int id = (int)jsonObject["ID"];
        //        string name = (string)jsonObject["Name"];
        //        bool isCountryPublish = (bool)jsonObject["IsCountryPublish"];
        //        int mobileCode = (int)jsonObject["MobileCode"];
        //        string countryCode3Digit = (string)jsonObject["CountryCode3Digit"];
        //        string countryCode2Digit = (string)jsonObject["CountryCode2Digit"];
        //        string flagPath = (string)jsonObject["FlagPath"];
        //        string currencyName = (string)jsonObject["Currency Name"];
        //        string currencyCode = (string)jsonObject["Currency Code"];
        //        string currencySymbol = (string)jsonObject["Currency Symbol"];
        //        bool isCurrencyPublish = (bool)jsonObject["IsCurrencyPublish"];

        //        // Create the desired JSON structure for each object
        //        var jsonData = new JObject(
        //            new JProperty("Country",
        //                new JObject(
        //                    new JProperty("ID", id),
        //                    new JProperty("Name", name),
        //                    new JProperty("IsCountryPublish", isCountryPublish),
        //                    new JProperty("MobileCode", mobileCode),
        //                    new JProperty("CountryCode3Digit", countryCode3Digit),
        //                    new JProperty("CountryCode2Digit", countryCode2Digit),
        //                    new JProperty("FlagPath", flagPath)
        //                )
        //            ),
        //            new JProperty("Currency",
        //                new JObject(
        //                    new JProperty("CurrencyName", currencyName),
        //                    new JProperty("CurrencyCode", currencyCode),
        //                    new JProperty("CurrencySymbol", currencySymbol),
        //                    new JProperty("IsCurrencyPublish", isCurrencyPublish)
        //                )
        //            )
        //        );

        //        desiredJsonArray.Add(jsonData);
        //    }

        //    // Write the JSON data to a file
        //    //string exportFilePath = "D:\\Projects\\AMMasterProject\\desiredJsonData.json";
        //    //System.IO.File.WriteAllText(exportFilePath, desiredJsonArray.ToString(Formatting.Indented));
        //    _websettinghelper.UpdateWebsettingJson("RegionalSettings", desiredJsonArray.ToString(Formatting.Indented));
        //    return Ok("");
        //}


        #endregion


        #region CountryCode2Digit- apiip.net Call
        public IActionResult CountryCode2Digit()
        {
            try
            {
                string ipAddress = GlobalHelper.IPAddress();
                string apikey = "b3fea954-8e07-49f0-a05a-b12c83336f8b";

                using (WebClient client = new WebClient())
                {
                    string url = "http://apiip.net/api/check?ip=" + ipAddress + "&accessKey=" + apikey;
                    string json = client.DownloadString(url);

                    return Content(json, "application/json"); // Return the complete JSON response
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log the error
                return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while fetching country information");
            }
        }
        #endregion


        #region Currency-GetCurrentRate
        [HttpPost]
        public async Task<IActionResult> currencycurrentrate(int countryid)
        {
            string apikey = "";
            var _basecurrencySettings = _websettinghelper.GetWebsettingJson("DefaultCurrencySettings");

            var _currencyapiSettings = _websettinghelper.GetWebsettingJson("CurrencyAPILayerSettings");

            if (_basecurrencySettings != null && !string.IsNullOrEmpty(_basecurrencySettings))
            {
                var json = JsonConvert.DeserializeObject<DefaultCurrencySettingsModel>(_basecurrencySettings);

                var CurrencyAPILayerjson = JsonConvert.DeserializeObject<CurrencyAPILayerSettingsModel>(_currencyapiSettings);

                if (CurrencyAPILayerjson.IsPublish == true)
                {
                    apikey = CurrencyAPILayerjson.APIKey;
                }

                if (apikey == "")
                {
                    TempData["success"] = "Currency conversion api is not updated.";
                    return Json("Currency conversion api is not updated.");
                }



                if (json != null)
                {

                    string basecurrency = json.BaseCurrency;  // Set your source currency here
                    decimal amount = 1;         // base amount


                    /* string toCurrency = "USD"; */   // get this from database, Set your target currency here

                    var currencylist = _dbcontext.Countries.Where(u => u.IsCurrencyPublish == true && u.CountryID == countryid).ToList();

                    foreach (var item in currencylist)
                    {
                        decimal conversionResult = await GlobalHelper.GetConversionResult(apikey, item.CurrencyCode, basecurrency, 1);


                        CountryCode cc = _dbcontext.Countries.FirstOrDefault(u => u.CountryID == item.CountryID);

                        if (cc != null)
                        {
                            cc.ConversionRate = conversionResult;
                            cc.ConversionUpdatedDate = DateTime.Now;

                            _dbcontext.Update(cc);
                            _dbcontext.SaveChanges();
                        }

                    }


                }
            }

            else
            {
                return Json("fail");
                TempData["success"] = "Please set base currency in admin > configuration.";
            }

            TempData["success"] = "Update successfully.";
            return Json("success");  // Return null or appropriate response if an error occurs
        }
        #endregion


        #region GetWordsFromnumber
        public IActionResult WordsFromNumber(decimal number)
        {
            string numberintwords = GlobalHelper.ConvertPriceToWords(number);

            return Json(numberintwords);
        }
        #endregion

        #region DateFormat
        public IActionResult DateFormat(string type) //date or time
        {
            string format = _globalHelper.Dateformat(type);


            return Json(new { format });
        }
        #endregion


        #region WebSettingUpdate
        [HttpPost]
        public IActionResult CommissionBuyerDeleteByRowID(string id)
        {
            try
            {
                var jsonExistingMetaData = _websettinghelper.GetWebsettingJson("CommissionBuyerSettings");
                // Deserialize the existing metadata JSON string into a list of CommissionTaxBuyerSettingsModel
                List<CommissionTaxBuyerSettingsModel> existingMetadata = JsonConvert.DeserializeObject<List<CommissionTaxBuyerSettingsModel>>(jsonExistingMetaData ?? "[]");

                // Find and remove the record with the specified ID
                CommissionTaxBuyerSettingsModel recordToRemove = existingMetadata.FirstOrDefault(x => x.ID == id);
                if (recordToRemove != null)
                {
                    existingMetadata.Remove(recordToRemove);
                }

                // Serialize the updated list back to JSON
                string updatedJson = JsonConvert.SerializeObject(existingMetadata);

                _websettinghelper.UpdateWebsettingJsonList("CommissionBuyerSettings", updatedJson);

                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("commissionbuyerdelete: " + ex.Message);
            }
        }

       
        [HttpPost]
        public IActionResult CommissionBuyerUpdateByRowID(string id, string label, string commissiontype, decimal amount)
        {
            try
            {
                var jsonExistingMetaData = _websettinghelper.GetWebsettingJson("CommissionBuyerSettings");
                // Deserialize the existing metadata JSON string into a list of CommissionTaxBuyerSettingsModel
                List<CommissionTaxBuyerSettingsModel> existingMetadata = JsonConvert.DeserializeObject<List<CommissionTaxBuyerSettingsModel>>(jsonExistingMetaData ?? "[]");

                // Find and remove the record with the specified ID
                CommissionTaxBuyerSettingsModel filter = existingMetadata.FirstOrDefault(x => x.ID == id);
                if (filter != null)
                {
                    filter.Label = label;
                    filter.CommissionType = commissiontype;
                    filter.Amount = amount;
                    filter.UpdatedDate = DateTime.Now;
                }

                // Serialize the updated list back to JSON
                string updatedJson = JsonConvert.SerializeObject(existingMetadata);

                _websettinghelper.UpdateWebsettingJsonList("CommissionBuyerSettings", updatedJson);

                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("commissionbuyerUpdate: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult CommissionSellerDeleteByRowID(string id)
        {
            try
            {
                var jsonExistingMetaData = _websettinghelper.GetWebsettingJson("CommissionSellerSettings");
                // Deserialize the existing metadata JSON string into a list of CommissionTaxBuyerSettingsModel
                List<CommissionTaxSellerSettingsModel> existingMetadata = JsonConvert.DeserializeObject<List<CommissionTaxSellerSettingsModel>>(jsonExistingMetaData ?? "[]");

                // Find and remove the record with the specified ID
                CommissionTaxSellerSettingsModel recordToRemove = existingMetadata.FirstOrDefault(x => x.ID == id);
                if (recordToRemove != null)
                {
                    existingMetadata.Remove(recordToRemove);
                }

                // Serialize the updated list back to JSON
                string updatedJson = JsonConvert.SerializeObject(existingMetadata);

                _websettinghelper.UpdateWebsettingJsonList("CommissionSellerSettings", updatedJson);

                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("commissionSellerdelete: " + ex.Message);
            }
        }


        [HttpPost]
        public IActionResult CommissionSellerUpdateByRowID(string id, string label, string commissiontype, decimal amount)
        {
            try
            {
                var jsonExistingMetaData = _websettinghelper.GetWebsettingJson("CommissionSellerSettings");
                // Deserialize the existing metadata JSON string into a list of CommissionTaxBuyerSettingsModel
                List<CommissionTaxBuyerSettingsModel> existingMetadata = JsonConvert.DeserializeObject<List<CommissionTaxBuyerSettingsModel>>(jsonExistingMetaData ?? "[]");

                // Find and remove the record with the specified ID
                CommissionTaxBuyerSettingsModel filter = existingMetadata.FirstOrDefault(x => x.ID == id);
                if (filter != null)
                {
                    filter.Label = label;
                    filter.CommissionType = commissiontype;
                    filter.Amount = amount;
                    filter.UpdatedDate = DateTime.Now;
                }

                // Serialize the updated list back to JSON
                string updatedJson = JsonConvert.SerializeObject(existingMetadata);

                _websettinghelper.UpdateWebsettingJsonList("CommissionSellerSettings", updatedJson);

                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("commissionSellerUpdate: " + ex.Message);
            }
        }

        #endregion
    }




}
