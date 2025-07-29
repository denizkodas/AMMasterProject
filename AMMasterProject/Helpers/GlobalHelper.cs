using Newtonsoft.Json;
using AMMasterProject.Models;
using AMMasterProject.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.NetworkInformation;
using Azure.Core;
using Azure;

using System.Text.RegularExpressions;
using GoogleMaps.LocationServices;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Security.Authentication;
using AMMasterProject.Controllers;




using ZXing;
using ZXing.Windows.Compatibility;
using System.Drawing.Imaging;
using TimeZoneConverter;
using System.Collections.ObjectModel;
using Google.Apis.Http;
using ThirdParty.Json.LitJson;


//using PayPal.Api;
//using PayPal.Api;

namespace AMMasterProject.Helpers
{
    public class GlobalHelper
    {
        #region Model
        private readonly WebsettingHelper _websettinghelper;

        private readonly MyDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IWebHostEnvironment _hostingEnvironment;
        #endregion


        #region DI
        public GlobalHelper(WebsettingHelper websettinghelper, MyDbContext dbContext, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _websettinghelper = websettinghelper;
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        #endregion


        #region SocialMediaSettingIcon_BasedonId
        public  string SocialMediaSettingIcon(int socialmediaId)
        {
            var web = _websettinghelper.GetWebsettingJson("SocialMediaSettings");

            if (web != null)
            {
                // Deserialize the JSON string into a list of SocialMediaSettingViewModel objects
                List<SocialMediaSettingViewModel> socialMediaList = JsonConvert.DeserializeObject<List<SocialMediaSettingViewModel>>(web);

                // Find the social media setting with the matching ID
                var socialMediaSetting = socialMediaList.FirstOrDefault(s => s.ID == socialmediaId);

                // Return the Icon if found, otherwise return a default or null
                if (socialMediaSetting != null)
                {
                    return socialMediaSetting.Icon;
                }
            }

            return null; // or return a default icon link if needed
        }
        #endregion

        #region VideoExtractor
        public static string ExtractVideoId(string url)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|\S*?[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})");
            var match = regex.Match(url);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
        #endregion

        #region CurrentDomainName


        public static string GetCurrentDomainName()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            //var request = httpContext?.Request;

            //if (request != null)
            //{
            //    var host = $"{request.Scheme}://{request.Host.Value}";
            //    return host;
            //}

            if (httpContext != null)
            {
                var request = httpContext.Request;

                // Check the X-Forwarded-Proto header to determine the original scheme
                var scheme = request.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? request.Scheme;

                var host = request.Host.Value;
                var domain = $"{scheme}://{host}";

                return domain;
            }

            return string.Empty;
        }

        public static string GetCurrentUrl()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            var request = httpContext?.Request;

            if (request != null)
            {
                var url = $"{request.Scheme}://{request.Host.Value}{request.PathBase.Value}{request.Path.Value}{request.QueryString.Value}";
                return url;
            }

            return string.Empty;
        }


        public static void SetReturnURL()
        {
            string url = GetCurrentUrl();

            if (url != null)
            {
                var httpContext = new HttpContextAccessor().HttpContext;
                httpContext.Session.SetString("returnurl", url);
            }


        }

        public static void SetReturnURLInSession(string url)
        {


            if (url != null)
            {
                var httpContext = new HttpContextAccessor().HttpContext;
                string currentydomain = GetCurrentDomainName();
                httpContext.Session.SetString("returnurl", currentydomain + url);
            }


        }

        public static string GetReturnURL()
        {


            var httpContext = new HttpContextAccessor().HttpContext;
            string returnUrl = httpContext.Session.GetString("returnurl");

            return returnUrl ?? string.Empty;
        }



        public static string GetReturnURLCookie(string cookiename)
        {
            HttpContext httpContext = new HttpContextAccessor().HttpContext;
            string url = httpContext.Request.Cookies[cookiename];

            if (url == null)
            {
                url = string.Empty;
            }

            return url;
        }




        #endregion

        #region Cookie
        public static void SetCookie(string cookiename, string value)
        {
            //HttpContext httpContext = new HttpContextAccessor().HttpContext;
            //httpContext.Response.Cookies.Append(cookiename, value);

            var httpContextAccessor = new HttpContextAccessor();
            HttpContext httpContext = httpContextAccessor.HttpContext;

            var cookieOptions = new CookieOptions
            {
                // Set a distant future date to make the cookie persistent
                Expires = DateTime.Now.AddYears(10),
                HttpOnly = true, // Ensures the cookie is only accessible through HTTP requests, not client-side scripts
            };

            httpContext.Response.Cookies.Append(cookiename, value, cookieOptions);

        }

        public static string ReadCookie(string cookiename)
        {
            HttpContext httpContext = new HttpContextAccessor().HttpContext;
            string cookie = httpContext.Request.Cookies[cookiename];
            return cookie;
        }
        public static void RemoveCookie(string cookiename)
        {
            HttpContext httpContext = new HttpContextAccessor().HttpContext;


            httpContext.Response.Cookies.Delete(cookiename);
        }

        #endregion



        #region InvoiceNumberGenerator

        //only for cross reference generator
        public static string GetInvoiceNumber(string id, string membershiptype)
        {

            char firstLetter = membershiptype.ToUpper().Substring(0, 1)[0];
            string orderid = id.Substring(0, Math.Min(id.Length, 4)).ToUpper();
            string uniqueId = Guid.NewGuid().ToString().ToUpper().Substring(0, 15).Replace("-", ""); // Generate a unique identifier (UUID)
            string currentDay = DateTimeOffset.Now.ToString("dd");

            string invoiceNumber = $"{firstLetter}{orderid}{currentDay}{uniqueId}";

            return invoiceNumber;
        }
        #endregion


        #region CurrencyConversion

        public bool IsMultiCurrencyEnabled()
        {

            bool isMultiCurrencyEnabled = false;
            var _defaultcurrencySettings = _websettinghelper.GetWebsettingJson("DefaultCurrencySettings");

            if (_defaultcurrencySettings != null && !string.IsNullOrEmpty(_defaultcurrencySettings))
            {

                var json = JsonConvert.DeserializeObject<DefaultCurrencySettingsModel>(_defaultcurrencySettings);

                if (json != null)
                {
                    isMultiCurrencyEnabled = json.IsMultiCurrency;

                }


            }

            return isMultiCurrencyEnabled;
        }
        public decimal GetCurrencyConversion(string actualcurrency, string conversioncurrency, string actualamount)
        {


            //var vactualcurrency = _dbContext.Countries.FirstOrDefault(u => u.CurrencySymbol + " " + u.CurrencyCode == actualcurrency);
            var vuserselectedcurrency = _dbContext.Countries.FirstOrDefault(u => u.CurrencyCode == conversioncurrency);


            decimal conversion = decimal.Parse(actualamount) * decimal.Parse(vuserselectedcurrency.ConversionRate.ToString());


            return conversion;
        }


        //Conversion Rate
        public decimal ConversionRate(string conversioncurrency)
        {
            if (conversioncurrency == null)
            {
                return 1;
            }


            //if mutli currency is disable so do not do conversion

            bool IsmultiCurrencyEnabled = IsMultiCurrencyEnabled();

            if (IsmultiCurrencyEnabled == false)
            {
                return 1;
            }

            //get admin setup base currency
            string basecurrency = BaseCurrency();

            if (basecurrency == conversioncurrency)
            {
                return 1;
            }


            //var vactualcurrency = _dbContext.Countries.FirstOrDefault(u => u.CurrencySymbol + " " + u.CurrencyCode == actualcurrency);
            var q = _dbContext.Countries.FirstOrDefault(u => u.CurrencyCode == conversioncurrency);


            decimal conversionrate = decimal.Parse(q.ConversionRate.ToString());

            return conversionrate;
        }
        #endregion

        #region GetUserCurrency

        public string GetUserCurrency()
        {
            string currencyfromcookie = ReadCookie("currencycode");

            //if(currencyfromcookie=="")
            //{
            //    currencyfromcookie = "USD";
            //}

            ///if is multi currency is disabled so do not read userd based currency
            ///
            bool IsmultiCurrencyEnabled = IsMultiCurrencyEnabled();

            if (IsmultiCurrencyEnabled == false)
            {
                currencyfromcookie = BaseCurrency();
            }

            return currencyfromcookie;
        }

        #region GetCurrency
        public string GetCurrentCurrency(int currencyid)
        {
            string currencyname = "$ USD";


            var currencyList = _dbContext.Countries.FirstOrDefault(u => u.CountryID == currencyid);

            if (currencyList != null)
            {
                currencyname = currencyList.CurrencySymbol + " " + currencyList.CurrencyCode;
            }

            else
            {
                currencyname = BaseCurrency();
            }

            return currencyname;

        }

        #endregion


        #endregion


        #region DateConversionToHourDayYear
        public static string ConvertToTimeAgo(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return string.Empty; // Handle null case as per your requirements
            }

            DateTime insertDate = dateTime.Value;
            TimeSpan timeSince = DateTime.UtcNow.Subtract(insertDate);

            if (timeSince.TotalSeconds < 0)
            {
                // Reverse the logic for future expiration
                timeSince = -timeSince;

                if (timeSince.TotalSeconds < 60)
                {
                    return "Just now";
                }
                else if (timeSince.TotalMinutes < 60)
                {
                    int minutes = (int)timeSince.TotalMinutes;
                    return $"Expires in {minutes} minute{(minutes == 1 ? "" : "s")}";
                }
                else if (timeSince.TotalHours < 24)
                {
                    int hours = (int)timeSince.TotalHours;
                    return $"Expires in {hours} hour{(hours == 1 ? "" : "s")}";
                }
                else if (timeSince.TotalDays < 7)
                {
                    int days = (int)timeSince.TotalDays;
                    return $"Expires in {days} day{(days == 1 ? "" : "s")}";
                }
                else if (timeSince.TotalDays < 30.436875) // Approximate number of days in a month
                {
                    int weeks = (int)(timeSince.TotalDays / 7);
                    return $"Expires in {weeks} week{(weeks == 1 ? "" : "s")}";
                }
                else
                {
                    int months = (int)(timeSince.TotalDays / 30.436875); // Approximate number of days in a month
                    return $"Expires in {months} month{(months == 1 ? "" : "s")}";
                }
            }
            else
            {
                // Logic for past time duration as before
                if (timeSince.TotalSeconds < 60)
                {
                    return "Just now";
                }
                else if (timeSince.TotalMinutes < 60)
                {
                    int minutes = (int)timeSince.TotalMinutes;
                    return $"{minutes} minute{(minutes == 1 ? "" : "s")} ago";
                }
                else if (timeSince.TotalHours < 24)
                {
                    int hours = (int)timeSince.TotalHours;
                    return $"{hours} hour{(hours == 1 ? "" : "s")} ago";
                }
                else if (timeSince.TotalDays < 7)
                {
                    int days = (int)timeSince.TotalDays;
                    return $"{days} day{(days == 1 ? "" : "s")} ago";
                }
                else if (timeSince.TotalDays < 30.436875) // Approximate number of days in a month
                {
                    int weeks = (int)(timeSince.TotalDays / 7);
                    return $"{weeks} week{(weeks == 1 ? "" : "s")} ago";
                }
                else
                {
                    int months = (int)(timeSince.TotalDays / 30.436875); // Approximate number of days in a month
                    return $"{months} month{(months == 1 ? "" : "s")} ago";
                }
            }
        }
        #endregion

        #region DateFormat


        public string Dateformat(string? datetime = "date") ///only date or full date with time
        {
            string df = "MMM dd, yyyy HH:mm:ss";

            // Example: "MMM dd, yyyy HH:mm:ss"
            //var df24Hour = "MMM dd, yyyy HH:mm:ss";

            // Example: "MMM dd, yyyy hh:mm:ss a"
            //var dfAMPM = "MMM dd, yyyy hh:mm:ss a";

            var _dateformatSettings = _websettinghelper.GetWebsettingJson("DateFormatSettings");

            if (_dateformatSettings != null && !string.IsNullOrEmpty(_dateformatSettings))
            {

                var json = JsonConvert.DeserializeObject<DateFormatSettingsModel>(_dateformatSettings);



                if (json != null)
                {

                    if (datetime == "date")
                    {
                        df = json.DateFormat;
                    }
                    else if (datetime == "time")
                    {
                        df = json.TimeFormat;
                    }

                }
            }
            return df;
        }

        public static string DateFormat(DateTime? dateTime)
        {


            string df = "MMM dd, yyyy";



            if (dateTime == null)
            {
                return string.Empty; // Handle null case as per your requirements
            }

            DateTime dateformat = dateTime.Value;
            string formattedDate = dateformat.ToString(df);

            return formattedDate;
        }

        #endregion

        #region GetYearandReturnNumberofYears
        public static int GetNumberOfYearsInBusiness(int? foundingYear)
        {
            // Get the current year
            int currentYear = DateTime.Now.Year;

            // Check if foundingYear is null
            if (foundingYear.HasValue)
            {
                // Calculate the number of years in business
                int yearsInBusiness = currentYear - foundingYear.Value;
                return yearsInBusiness;
            }
            else
            {
                // Handle the case where foundingYear is null
                // Default to the current year if foundingYear is null
                return 0; // You can change this default value as needed
            }
        }

        #endregion


        #region GenerateRandomCode
        public static string RandomNumber()
        {
            Random random = new Random();
            int validationCode = random.Next(10000, 99999);

            return validationCode.ToString();
        }

        #endregion

        #region Images
        public bool ValidateImageSize(string filePath, int desiredWidth, int desiredHeight)
        {
            using (var image = System.Drawing.Image.FromFile(filePath))
            {
                int actualWidth = image.Width;
                int actualHeight = image.Height;

                if (actualWidth != desiredWidth || actualHeight != desiredHeight)
                {
                    // Image size doesn't match the desired dimensions
                    return false;
                }
            }

            // Image size is valid
            return true;
        }
        #endregion



        #region AdminEmails


        public static List<string> AdminEmails(string type)
        {
            List<string> emails = new List<string>();

            if (type == "order")
            {
                emails.Add("globaliwebsite@gmail.com");
                emails.Add("amtechnology.info");
            }
            // Add more conditions for different types and corresponding email addresses if needed

            return emails;
        }
        #endregion

        #region ValidateEmailOrPhone
        public static string GetEmailOrPhone(string input)
        {
            if (IsEmail(input))
            {
                return "Email";
            }
            else if (IsPhoneNumber(input))
            {
                return "Phone";
            }
            else
            {
                return "Unknown";
            }
        }

        public static bool IsEmail(string input)
        {
            // Use a regular expression pattern to validate email
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(input, pattern);
        }

        public static bool IsPhoneNumber(string input)
        {
            // Use a regular expression pattern to validate phone number
            string pattern = @"^\d{3}\d{3}\d{4}$";
            return Regex.IsMatch(input, pattern);
        }
        #endregion


        #region CompanySetup

        private string setup()
        {
            return "succes";
        }
        //public static string CompanySetup()
        //{
        //    GlobalHelper gh = new GlobalHelper();
        //    gh.setup();
        //    return "";

        //}
        #endregion

        #region SEOURL
        public static string SEOURL(string value)
        {

            if (value != null)
            {
                return Regex.Replace(value.ToLower(), "[^a-z0-9]+", "-");
            }
            else
            {
                return "";
            }

        }
        #endregion


        #region ExtractKeyword
        public static string ExtractKeywords(string text)
        {
            // Define common or insignificant words to exclude from keywords
            string[] excludedWords = { "a", "an", "the", "and", "or", "but", "on", "in", "with", "for" };

            // Split the text into words
            string[] words = text.Split(new[] { ' ', ',', '.', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);

            // Filter out excluded words
            var keywords = words.Where(w => !excludedWords.Contains(w.ToLower())).Distinct();

            // Join the keywords into a comma-separated string
            return string.Join(", ", keywords);
        }
        #endregion


        #region GoogleLocations
        public class GeocodeResult
        {
            public string Country { get; set; }

            public string Country2DigitCode { get; set; }

            public string CountryFlagUrlPath { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Zipcode { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public string TimeZone { get; set; }
        }


       

        public GeocodeResult GetGeocodeDetails(string address)
        {
            GeocodeResult result = new GeocodeResult();

            var apiKey = GetGoogleMapAPi(); // Replace with your actual API key

            var locationService = new GoogleLocationService(apiKey);

            var point = locationService.GetLatLongFromAddress(address);
            result.Latitude = point.Latitude;
            result.Longitude = point.Longitude;

            var geocodingService = new GoogleLocationService(apiKey);
            var addressData = geocodingService.GetAddressFromLatLang(result.Latitude, result.Longitude);
            CountryViewModel countrymodel= GetCountryList().FirstOrDefault(u=>u.Name.Trim().ToLower() == addressData.Country.Trim().ToLower());
            // Get time zone information using the Google Time Zone API
          


            result.Country = addressData.Country;
            result.State = addressData.State;
            result.City = addressData.City;
            result.Zipcode = addressData.Zip;
            result.Country2DigitCode = countrymodel.CountryAlpha2;
            result.CountryFlagUrlPath = countrymodel.Flag.Replace("~","");
            result.TimeZone = GetTimeZoneByCoordinatesAsync(result.Latitude, result.Longitude);
            return result;
        }

        

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2, string distancetype)
        {
            const double EarthRadiusKm = 6371; // Earth's radius in kilometers
            const double EarthRadiusMiles = 3959; // Earth's radius in miles

            // Convert latitude and longitude from degrees to radians
            double lat1Rad = Math.PI * lat1 / 180;
            double lon1Rad = Math.PI * lon1 / 180;
            double lat2Rad = Math.PI * lat2 / 180;
            double lon2Rad = Math.PI * lon2 / 180;

            // Haversine formula
            double dlon = lon2Rad - lon1Rad;
            double dlat = lat2Rad - lat1Rad;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(dlon / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance;

            if (distancetype == "KM")
            {
                distance = EarthRadiusKm * c; // Return distance in kilometers
            }
            else if (distancetype == "M")
            {
                distance = EarthRadiusMiles * c; // Return distance in miles
            }
            else
            {
                throw new ArgumentException("Invalid distancetype. Use 'KM' for kilometers or 'M' for miles.");
            }

            return distance;
        }

        //depreciated google helper
        //public string GetCountryOnIP()
        //{
        //    GeocodeResult result = new GeocodeResult();

        //    var apiKey = GetGoogleMapAPi(); // Replace with your actual API key

        //    try
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            try
        //            {
        //                // Send a request to the Google Geolocation API
        //                var response = httpClient.GetStringAsync($"https://www.googleapis.com/geolocation/v1/geolocate?key={apiKey}").Result;

        //                // Parse the JSON response
        //                var jsonResponse = JObject.Parse(response);

        //                // Extract the country code (you may need to adapt this based on the API's response structure)
        //                var countryCode = jsonResponse["location"]["countryCode"].ToString();

        //                result.Country = countryCode;
        //            }
        //            catch (Exception ex)
        //            {
        //                // Handle any exceptions here
        //                //Console.WriteLine($"Error: {ex.Message}");
        //                result.Country = "Unknown"; // Return a default value or handle the error as needed.
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result.Country = "Unknown";
        //    }

        //    return result.Country.ToString();
        //}

        //ipinfo
        public Dictionary<string, string> GetLocationOnIP(string ipAddress)
        {
            Dictionary<string, string> locationData = new Dictionary<string, string>();

            try
            {
                string _token = "b36d9f54504fe5";
                using (var httpClient = new HttpClient())
                {
                    // Send a request to the ipinfo.io API with the token
                    var response = httpClient.GetStringAsync($"https://ipinfo.io/{ipAddress}?token={_token}").Result;

                    // Parse the JSON response
                    var jsonResponse = JObject.Parse(response);

                    // Extract the country, city, and timezone
                    var country = jsonResponse["country"]?.ToString() ?? "Unknown";
                    var city = jsonResponse["city"]?.ToString() ?? "Unknown";
                    var timezone = jsonResponse["timezone"]?.ToString() ?? "Unknown";

                    // Add to dictionary
                    locationData["Country"] = country;
                    locationData["City"] = city;
                    locationData["Timezone"] = timezone;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                locationData["Country"] = "Unknown";
                locationData["City"] = "Unknown";
                locationData["Timezone"] = "Unknown";
            }

            return locationData;
        }
    
    #endregion


    #region AppSettingMethods

    #region Language

    public LanguageSetupSettingsModel languagesettings()
        {
            var languagesetup = new LanguageSetupSettingsModel();
            var _languagesetupSettings = _websettinghelper.GetWebsettingJson("LanguageSetupSettings");

            if (_languagesetupSettings != null && !string.IsNullOrEmpty(_languagesetupSettings))
            {
                languagesetup = JsonConvert.DeserializeObject<LanguageSetupSettingsModel>(_languagesetupSettings);
            }

            return languagesetup;
        }

        #endregion




        #endregion


        #region CustomScripts
        public List<ScriptCustom> GetScriptCustom()
        {
            var scriptCustomList = new List<ScriptCustom>();
            //string googleMapApiKey = GetGoogleMapAPi();

            //string googleMapApiScript = $"<script type=\"text/javascript\" src=\"https://maps.googleapis.com/maps/api/js?libraries=places&key={googleMapApiKey}\" async defer></script>";

            var scriptManagerSettings = _websettinghelper.GetWebsettingJson("ScriptManagerSettings");

            if (!string.IsNullOrEmpty(scriptManagerSettings))
            {
                List<ScriptManagerSettingsViewModel> listParse = JsonConvert.DeserializeObject<List<ScriptManagerSettingsViewModel>>(scriptManagerSettings);
                var listScriptManager = listParse.Where(u => u.IsPublish).ToList();

                foreach (var item in listScriptManager)
                {
                    scriptCustomList.Add(new ScriptCustom { Script = item.Script, ScriptName=item.Name });
                    
                }

                // Add the Google Maps API script outside the loop
                //scriptCustomList.Add(new ScriptCustom { Script = googleMapApiScript , ScriptName = "GoogleAPIKey" });
                
            }

            return scriptCustomList;
        }

        #endregion


        #region Country

        public List<CountryViewModel> GetCountryList()
        {
            var countryList = _dbContext.Countries.ToList();

            var countryViewModelList = countryList.Select(country => new CountryViewModel
            {
                ID = country.CountryID,
                Name = country.Name,
                Flag = country.Flagpath,
                CountryAlpha2 = country.CountryCode2Digit.Trim(),
                CountryMobileCode = country.MobileCode.Trim(),
                CurrencyCode = country.CurrencyCode.Trim(),
                ConversionRate = country.ConversionRate,
                CurrencyName = country.CurrencyName,
                CurrencySymbol = country.CurrencySymbol,
                IsCurrencyPublish = country.IsCurrencyPublish,
                IsCountryPublish = country.IsPublish
                // Map other properties as needed
            }).OrderBy(u => u.CountryAlpha2).ToList();

            return countryViewModelList;
        }

        #endregion

        #region Currency

        public List<CountryViewModel> GetCurrencyList()
        {
            var currencyList = _dbContext.Countries.ToList();

            var currencyViewModelList = currencyList.Select(currency => new CountryViewModel()
            {
                ID = currency.CountryID,
                Name = currency.Name,
                CurrencyName = currency.CurrencyName.Trim(),
                CurrencyCode = currency.CurrencyCode.Trim(),
                CurrencySymbol = currency.CurrencySymbol.Trim(),
                IsCurrencyPublish = currency.IsCurrencyPublish,
                ConversionRate = currency.ConversionRate,
                ConversionDateupdate = currency.ConversionUpdatedDate

                // Map other properties as needed
            }).OrderBy(u => u.Name).ToList();

            return currencyViewModelList;
        }


        //based on admin base currency setup it returns all column from the currency table
        public CountryViewModel GetBaseCurrency()
        {
            string defaultCurrency = "USD";
            var currencySetupSettings = _websettinghelper.GetWebsettingJson("DefaultCurrencySettings");

            if (!string.IsNullOrEmpty(currencySetupSettings))
            {
                var json = JsonConvert.DeserializeObject<DefaultCurrencySettingsModel>(currencySetupSettings);

                if (json != null)
                {
                    defaultCurrency = json.BaseCurrency ?? "USD";
                }
            }

            var model = GetCurrencyList().FirstOrDefault(u => u.CurrencyCode == defaultCurrency);
            return model;
        }
        #endregion

        #region Language

        public List<LanguageViewModel> GetLanguageList()
        {
            var languageSetting = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "Language");

            if (languageSetting == null)
            {
                return new List<LanguageViewModel>();
            }

            var languageList = JsonConvert.DeserializeObject<List<LanguageViewModel>>(languageSetting.WebsettingValue);

            return languageList;
        }


        public LanguageViewModel GetBaseLanguage()
        {
            string defaultLanguage = "en";
            var languageSetupSettings = _websettinghelper.GetWebsettingJson("LanguageSetupSettings");

            if (!string.IsNullOrEmpty(languageSetupSettings))
            {
                var json = JsonConvert.DeserializeObject<LanguageSetupSettingsModel>(languageSetupSettings);

                if (json != null)
                {
                    defaultLanguage = json.DefaultLanguage ?? "en";
                }
            }

            var model = GetLanguageList().FirstOrDefault(u => u.Code == defaultLanguage);
            return model;
        }

        #region GetGoogleMapAPI
        public string GetGoogleMapAPi()
        {
            string apikey = "";
            var googlemapapiSetupSettings = _websettinghelper.GetWebsettingJson("GoogleMapsApiSettings");

            if (!string.IsNullOrEmpty(googlemapapiSetupSettings))
            {
                var json = JsonConvert.DeserializeObject<GoogleMapsApiSettingsModel>(googlemapapiSetupSettings);

                if (json != null)
                {
                    apikey = json.APIKey;
                }
            }


            return apikey;
        }
        #endregion



        #endregion


        #region IPAddress
        public static string IPAddress()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            var request = httpContext?.Request;
            string ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = httpContext.Request.Headers["X-Forwarded-For"];
            }
            else
            {
                ipAddress = "39.39.126.129";
            }

            return ipAddress;

        }

        #endregion


        #region CurrencyConversionAPICall
        public static async Task<decimal> GetConversionResult(string apikey, string toCurrency, string baseCurrency, decimal amount)
        {

            string url = $"https://api.apilayer.com/exchangerates_data/convert?to={toCurrency}&from={baseCurrency}&amount={amount}";
            // Set TLS 1.2 as the default security protocol for the HttpClient
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("apikey", apikey);

                HttpResponseMessage response = await client.GetAsync(url);
                string content = await response.Content.ReadAsStringAsync();

                // Process the response content
                JObject json = JObject.Parse(content);
                decimal result = json["result"] != null ? (decimal)json["result"] : 1;

                return result;  // Return the conversion result or 1 if not available
            }
        }
        #endregion


        #region VideoURLGenerator
        public static string VideoURLGenerator(string provider, string source)
        {
            if (provider == "youtube")
            {
                return "https://www.youtube.com/embed/" + source;
            }
            else if (provider == "vimeo")
            {
                return "https://player.vimeo.com/video/" + source;
            }
            else if (provider == "html5")
            {
                return source; // Assuming the source field already contains the complete video URL
            }
            return string.Empty;
        }

        #endregion


        #region CommaSeperatedLastValue
        public static string CommaSeperationLastValue(string array)
        {
            string returnvalue = "";
            if (array == "")
            {
                return returnvalue;
            }


            string[] values = array.Split(',');
            if (values.Length > 0)
            {
                string lastValue = values[values.Length - 1];
                if (int.TryParse(lastValue, out int result))
                {
                    returnvalue = result.ToString();
                }
            }

            return returnvalue;
        }
        #endregion



        #region LabelSetting

        public string LabelSet(string labelname)
        {
            var _labelSettings = _websettinghelper.GetWebsettingJson("LabelSettings");

            if (_labelSettings != null && !string.IsNullOrEmpty(_labelSettings))
            {
                var json = JsonConvert.DeserializeObject<LabelSettingsModel>(_labelSettings);

                if (json != null)
                {
                    // Use a where clause to filter the properties based on the labelname
                    var labelProperty = typeof(LabelSettingsModel)
                        .GetProperties()
                        .FirstOrDefault(p => p.Name.Equals(labelname, StringComparison.OrdinalIgnoreCase));

                    if (labelProperty != null)
                    {
                        // Retrieve the value of the matching property
                        string value = labelProperty.GetValue(json)?.ToString();
                        return value ?? string.Empty;
                    }
                }
            }

            return string.Empty; // Return empty string if no label settings are available or labelname is not found
        }


        #endregion


        #region ValiDateCurrencyAllowed
        public bool currencyalloweValidation(string paymentmethod)
        {
            bool message = false;
            if (paymentmethod == "cod" || paymentmethod == "banktransfer")
            {
                message = true;

                return message;
            }


            string currencycodeselected = GetUserCurrency();

            // Define a static list of payment methods and their allowed currencies
            Dictionary<string, List<string>> paymentMethodsAllowedCurrencies = new Dictionary<string, List<string>>()
            {
                { "paypal", new List<string> { "AUD", "BRL", "CAD", "CNY", "CZK", "DKK", "EUR", "HKD", "HUF", "ILS", "JPY", "MYR", "MXN", "TWD", "NZD", "NOK", "PHP", "PLN", "GBP", "SGD", "SEK", "CHF", "THB", "USD" } },
                { "stripe", new List<string> { "USD", "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BIF", "BMD", "BND", "BOB", "BRL", "BSD", "BWP", "BYN", "BZD", "CAD", "CDF", "CHF", "CLP", "CNY", "COP", "CRC", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD", "EGP", "ETB", "EUR", "FJD", "FKP", "GBP", "GEL", "GIP", "GMD", "GNF", "GTQ", "GYD", "HKD", "HNL", "HTG", "HUF", "IDR", "ILS", "INR", "ISK", "JMD", "JPY", "KES", "KGS", "KHR", "KMF", "KRW", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MUR", "MVR", "MWK", "MXN", "MYR", "MZN", "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "RWF", "SAR", "SBD", "SCR", "SEK", "SGD", "SHP", "SLE", "SOS", "SRD", "STD", "SZL", "THB", "TJS", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "UYU", "UZS", "VND", "VUV", "WST", "XAF", "XCD", "XOF", "XPF", "YER", "ZAR", "ZMW" } },
                { "razorpay", new List<string> { "USD", "EUR", "GBP", "SGD", "AED", "AUD", "CAD", "CNY", "SEK", "NZD", "MXN", "HKD", "NOK", "RUB", "ALL", "AMD", "ARS", "AWG", "BBD", "BDT", "BMD", "BND", "BOB", "BSD", "BWP", "BYN", "BZD", "CHF", "COP", "CRC", "CVE", "CZK", "DKK", "DOP", "DZD", "EGP", "ETB", "FJD", "GEL", "GIP", "GMD", "GNF", "GTQ", "GYD", "HNL", "HUF", "IDR", "ILS", "INR", "ISK", "JMD", "JPY", "KES", "KGS", "KHR", "KMF", "KRW", "KYD", "KZT", "LAK", "LBP", "LKR", "LRD", "LSL", "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRO", "MUR", "MVR", "MWK", "MYR", "MZN", "NAD", "NGN", "NIO", "NPR", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RWF", "SAR", "SCR", "SEK", "SGD", "SLL", "SOS", "SZL", "THB", "TOP", "TRY", "TTD", "TWD", "TZS", "UAH", "UGX", "UYU", "UZS", "VND", "VUV", "WST", "XAF", "XCD", "XOF", "XPF", "YER", "ZAR", "GHS" } },
                { "payfast", new List<string> { "ZAR"} },
                { "paystack", new List<string> { "GHS", "ZAR", "NGN", "KES", "USD" } },
                { "dpo", new List<string> { "USD", "GBP", "EUR" } },
                { "sslcommerz", new List<string> { "BDT", "EUR", "GBP", "AUD", "USD", "CAD" } },
                 { "mtn", new List<string> { "EUR",  "USD", "UGX" } },
                 // Add
            };

            if (paymentMethodsAllowedCurrencies.ContainsKey(paymentmethod.ToLower()))
            {
                List<string> allowedCurrencies = paymentMethodsAllowedCurrencies[paymentmethod.ToLower()];
                if (allowedCurrencies.Contains(currencycodeselected))
                {
                    message = true;
                }
            }

            return message;
        }
        #endregion


        #region BaseCurrency

        //this fetch what admin has setup the base currency
        public string BaseCurrency()
        {
            string basecurrency = "USD";
            var _basecurrencySettings = _websettinghelper.GetWebsettingJson("DefaultCurrencySettings");
            if (_basecurrencySettings != null && !string.IsNullOrEmpty(_basecurrencySettings))
            {
                var json = JsonConvert.DeserializeObject<DefaultCurrencySettingsModel>(_basecurrencySettings);

                if (json != null)
                {

                    basecurrency = json.BaseCurrency;
                }
            }

            return basecurrency;
        }
        #endregion


        public string LevelofUser(int profileid, string type)//seller or buyer
        {
            string levelname = "";
            if (type == "buyer")
            {
                levelname = "Explorer";
            }
            else if (type == "seller")
            {
                levelname = "Aspiring Seller";
            }
            return levelname;


            //            Buyer Levels:

            //Explorer: Just starting the property search journey.
            //Dream Seeker: Actively looking for that perfect property.
            //Informed Buyer: Gathering information and researching options.
            //Serious Shopper: Engaged in property viewings and evaluations.
            //Ready Investor: Prepared to make a purchase decision.
            //Property Prodigy: Knowledgeable and strategic in property selection.
            //Seller Levels:

            //Aspiring Seller: Considering the idea of selling a property.
            //Prep and Plan: Getting the property ready for market.
            //Listing Pro: Property listed and actively seeking buyers.
            //Negotiation Ace: Skillful in negotiating offers.
            //Pending Closure: In the final stages of completing a sale.
            //Market Maven: Experienced and successful in property sales
        }


        #region ConvertNumbertoWords
        //public static string ConvertPriceToWords(decimal price)
        //{
        //    if (price == 0)
        //        return "Zero Dollars";

        //    string[] ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        //    string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        //    string[] teens = { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        //    string[] thousands = { "", "Thousand", "Million", "Billion" };

        //    int i = 0;
        //    string words = "";

        //    while (price > 0)
        //    {
        //        int threeDigits = (int)(price % 1000);
        //        price /= 1000;

        //        int onesDigit = threeDigits % 10;
        //        int tensDigit = (threeDigits / 10) % 10;
        //        int hundredsDigit = threeDigits / 100;

        //        string threeDigitWords = "";

        //        if (hundredsDigit > 0)
        //        {
        //            threeDigitWords += ones[hundredsDigit] + " Hundred ";
        //        }

        //        if (tensDigit > 0)
        //        {
        //            if (tensDigit == 1 && onesDigit > 0)
        //            {
        //                threeDigitWords += teens[onesDigit] + " ";
        //                onesDigit = 0;
        //            }
        //            else
        //            {
        //                threeDigitWords += tens[tensDigit] + " ";
        //            }
        //        }

        //        if (onesDigit > 0)
        //        {
        //            threeDigitWords += ones[onesDigit] + " ";
        //        }

        //        if (threeDigits > 0)
        //        {
        //            threeDigitWords += thousands[i] + " ";
        //        }

        //        words = threeDigitWords + words;
        //        i++;
        //    }

        //    return words.Trim() + " Dollars";
        //}

        public static string ConvertPriceToWords(decimal price)
        {
            if (price == 0)
                return "Zero";

            string[] ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            string[] teens = { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            string[] thousands = { "", "Thousand", "Million", "Billion" };

            int i = 0;
            string words = "";

            int dollars = (int)Math.Floor(price);
            int cents = (int)((price - dollars) * 100);

            while (dollars > 0)
            {
                int threeDigits = dollars % 1000;
                dollars /= 1000;

                int onesDigit = threeDigits % 10;
                int tensDigit = (threeDigits / 10) % 10;
                int hundredsDigit = threeDigits / 100;

                string threeDigitWords = "";

                if (hundredsDigit > 0)
                {
                    threeDigitWords += ones[hundredsDigit] + " Hundred ";
                }

                if (tensDigit > 0)
                {
                    if (tensDigit == 1 && onesDigit > 0)
                    {
                        threeDigitWords += teens[onesDigit] + " ";
                        onesDigit = 0;
                    }
                    else
                    {
                        threeDigitWords += tens[tensDigit] + " ";
                    }
                }

                if (onesDigit > 0)
                {
                    threeDigitWords += ones[onesDigit] + " ";
                }

                if (threeDigits > 0)
                {
                    threeDigitWords += thousands[i] + " ";
                }

                words = threeDigitWords + words;
                i++;
            }

            string dollarsPart = words.Trim();

            if (cents > 0)
            {
                string centsPart = $"and {ConvertPriceToWords(cents)} Cents";
                return dollarsPart + " " + centsPart;
            }

            return dollarsPart;
        }
        #endregion


        #region ReadTEmplateFile
        public static string ReadTemplateFile(string filename)
        {
            string htmlTemplate = File.ReadAllText(filename); // Load the HTML template from a file


            return htmlTemplate;
        }
        #endregion


        #region Mins-Days-Month-Year
        public static string FormatLastSeen(DateTime lastSeenTime)
        {

            if (lastSeenTime == null)
            {
                lastSeenTime = DateTime.Now;
            }

            TimeSpan timeDifference = DateTime.Now - lastSeenTime;

            if (timeDifference.TotalMinutes < 60)
            {
                // Less than an hour ago
                int minutesAgo = (int)timeDifference.TotalMinutes;
                return $"{minutesAgo} minute{(minutesAgo != 1 ? "s" : "")} ago";
            }
            else if (timeDifference.TotalHours < 24)
            {
                // Less than a day ago
                int hoursAgo = (int)timeDifference.TotalHours;
                return $"{hoursAgo} hour{(hoursAgo != 1 ? "s" : "")} ago";
            }
            else if (timeDifference.TotalDays < 30)
            {
                // Less than a month ago
                int daysAgo = (int)timeDifference.TotalDays;
                return $"{daysAgo} day{(daysAgo != 1 ? "s" : "")} ago";
            }
            else
            {
                // More than a month ago
                int monthsAgo = (int)(timeDifference.TotalDays / 30);
                return $"{monthsAgo} month{(monthsAgo != 1 ? "s" : "")} ago";
            }
        }
        #endregion



        #region ExtractFileNameFromGUID

        public static string FileNameFromGUID(string imageUrl)
        {
            // Parse the URL
            Uri uri = new Uri(imageUrl);

            // Get the file name without extension
            string fileNameWithGuid = Path.GetFileNameWithoutExtension(uri.LocalPath);
            // Find the index where the last 32 characters (GUID) start
            int lastGuidIndex = fileNameWithGuid.LastIndexOf('_');
            string result = "";
            if (lastGuidIndex != -1 && lastGuidIndex + 32 <= fileNameWithGuid.Length)
            {
                // Extract the substring without the GUID
                result = fileNameWithGuid.Substring(0, lastGuidIndex);


            }

            return result;
        }

        #endregion


        #region License
        public string LicenseValidator()
        {
            string message = "~/license";
            var _licenseappsettingsSettings = _websettinghelper.GetWebsettingJson("LicenseAppSettings");

            if (_licenseappsettingsSettings != null && !string.IsNullOrEmpty(_licenseappsettingsSettings))
            {

                var json = JsonConvert.DeserializeObject<LicenseAppSettingsModel>(_licenseappsettingsSettings);



                if (json != null)
                {
                    if (json.LicenseKey != "")
                    {
                        message = "Valid";
                    }

                }
            }

            return message;

        }
        #endregion


        #region GeneratBarCode
        public byte[] GenerateBarcodes(string inputData)
        {
            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.CODE_128;

            barcodeWriter.Options.Width = 250; // Adjust width as needed
            barcodeWriter.Options.Height = 100; // Adjust height as needed

            var barcodeBitmap = barcodeWriter.Write(inputData);

            // Convert the image to byte array
            byte[] imageData;
            using (MemoryStream ms = new MemoryStream())
            {
                barcodeBitmap.Save(ms, ImageFormat.Png);
                imageData = ms.ToArray();
            }

            return imageData;
        }

        public byte[] GenerateQRcodes(string inputData)
        {
            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;



            barcodeWriter.Options.Width = 200;
            barcodeWriter.Options.Height = 200;



            var barcodeBitmap = barcodeWriter.Write(inputData);

            // Convert the image to byte array
            byte[] imageData;
            using (MemoryStream ms = new MemoryStream())
            {
                barcodeBitmap.Save(ms, ImageFormat.Png);
                imageData = ms.ToArray();
            }

            return imageData;
        }


    

        #endregion

        #region SKUGenerator

        public string GenerateSKU(string input)
        {
            // If input is already 12 digits or more, return it as-is
            if (input.Length >= 12)
            {
                return input;
            }

            // If input is less than 12 digits, add leading '0's to make it 12 digits
            int zerosToAdd = 12 - input.Length;
            string sku = new string('0', zerosToAdd) + input;

            return sku;
        }

        #endregion


        #region TimeZoneConvertor

        public string GetTimeZoneByCoordinatesAsync(double latitude, double longitude)
        {
            try
            {
                var apiKey = GetGoogleMapAPi();
                using (var httpClient = new HttpClient())
                {
                    // Make a request to the Google Time Zone API
                    string apiUrl = $"https://maps.googleapis.com/maps/api/timezone/json?location={latitude},{longitude}&timestamp={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}&key={apiKey}";
                    var response = httpClient.GetStringAsync(apiUrl).Result; // Using .Result to wait for the task to complete

                    // Deserialize the JSON response
                    var timeZoneInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<TimeZoneResponse>(response);

                    // Return the time zone ID
                    return timeZoneInfo.TimeZoneId;
                }
            }
            catch (HttpRequestException)
            {
                // Handle if there is an issue with the API request
                return "N/A";
            }
            catch (Newtonsoft.Json.JsonException)
            {
                // Handle if there is an issue with JSON deserialization
                return "N/A";
            }
        }

        public static string GetLocalTimeByCoordinatesAsync(string timeZoneId)
        {
            //string timeZoneId = GetTimeZoneByCoordinatesAsync(latitude, longitude);

            if (!string.IsNullOrEmpty(timeZoneId))
            {
                try
                {
                    var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                    return localTime.ToString("hh:mm tt");
                }
                catch (TimeZoneNotFoundException)
                {
                    // Handle if time zone is not found
                    return "N/A";
                }
                catch (InvalidTimeZoneException)
                {
                    // Handle if the time zone is invalid
                    return "N/A";
                }
            }

            // Handle case where no matching time zone is found
            return "N/A";
        }

        private class TimeZoneResponse
        {
            public string TimeZoneId { get; set; }
        }


      
        #endregion

    }


}
