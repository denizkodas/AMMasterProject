using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AMMasterProject.Pages.Admin
{



    [Authorize(Policy = "Admin")]
    [BindProperties]
    public class settingsModel : PageModel
    {
        #region Model

        private readonly WebsettingHelper _websettinghelper;

        private readonly GlobalHelper _globalHelper;
        public RegisterSettingsModel registermethod { get; set; }

        //public RegisterVerificationSettingsModel registerverification { get; set; }

     

        public LanguageSetupSettingsModel languagesetup { get; set; }

        public List<LanguageViewModel> Listlanguagesetup { get; set; }

        

        public DefaultCurrencySettingsModel defaultcurrency { get; set; }

        public GlobalAppSettingsModel globalAppSettings { get; set; }

        public List<CountryViewModel> ListCurrencies { get; set; }

        public List<CountryViewModel> ListCountries { get; set; }

        public DateFormatSettingsModel dateformat { get; set; }


        public SellerProfileSettingsModel sellerprofilesettings { get; set; }

        public LabelSettingsModel labelsettings { get; set; }

        public CookieSettingsModel cookiesetting { get; set; }



     

        public IEnumerable<SelectListItem> ListLanguage { get; set; }

        public IEnumerable<SelectListItem> ListCurrency { get; set; }
        #endregion


        #region DI



        public settingsModel(WebsettingHelper websettinghelper, GlobalHelper globalHelper)
        {
            _websettinghelper = websettinghelper;
            registermethod = new RegisterSettingsModel();
            //registerverification = new RegisterVerificationSettingsModel();
            _globalHelper = globalHelper;
        }

        #endregion


        public void OnGet()
        {
            ////register method
            ///



            var _registerSettings = _websettinghelper.GetWebsettingJson("RegisterSettings");

            if (_registerSettings != null && !string.IsNullOrEmpty(_registerSettings))
            {

                var json = JsonConvert.DeserializeObject<RegisterSettingsModel>(_registerSettings);



                if (json != null)
                {

                    registermethod = new RegisterSettingsModel
                    {
                        IsEmail = json.IsEmail,
                        IsPhone = json.IsPhone,
                        IsGoogleLogin =json.IsGoogleLogin,
                        IsFacebookLogin =json.IsFacebookLogin,
                        ClientId= json.ClientId,
                        ClientSecret    = json.ClientSecret,
                        AppId = json.AppId, 
                        AppSecret = json.AppSecret,
                        RegisterImage  =json.RegisterImage,
                        LoginImage=json.LoginImage,
                        VerificationPlatform = json.VerificationPlatform,
                        IsRegisterVerification=json.IsRegisterVerification
                    };

                }
            }








            ///register verification
            ///


            //var _registerverificationSettings = _websettinghelper.GetWebsettingJson("RegisterVerificationSettings");

            //if (_registerverificationSettings != null && !string.IsNullOrEmpty(_registerverificationSettings))
            //{

            //    var json = JsonConvert.DeserializeObject<RegisterVerificationSettingsModel>(_registerverificationSettings);



            //    if (json != null)
            //    {

            //        registerverification = new RegisterVerificationSettingsModel
            //        {
            //            RegisterVerification = json.RegisterVerification

            //        };

            //    }
            //}



          

           

            //language

            var _languagesetupSettings = _websettinghelper.GetWebsettingJson("LanguageSetupSettings");

            if (_languagesetupSettings != null && !string.IsNullOrEmpty(_languagesetupSettings))
            {

                var json = JsonConvert.DeserializeObject<LanguageSetupSettingsModel>(_languagesetupSettings);



                if (json != null)
                {

                    languagesetup = new LanguageSetupSettingsModel
                    {
                        
                        IsMultilingual = json.IsMultilingual,
                        DefaultLanguage =json.DefaultLanguage ?? "en"
                    };




                }


                //list of languages

                ListLanguage = _globalHelper.GetLanguageList()
                   .Where(u => u.IsPublish == true)
                   .Select(u => new SelectListItem
                   {
                       Value = u.Code.ToString(),
                       Text = u.Name,
                       Selected =json.DefaultLanguage == u.Code

                   }).ToList();
            }



            //defaultcurrency

            var _defaultcurrencySettings = _websettinghelper.GetWebsettingJson("DefaultCurrencySettings");

            if (_defaultcurrencySettings != null && !string.IsNullOrEmpty(_defaultcurrencySettings))
            {

                var json = JsonConvert.DeserializeObject<DefaultCurrencySettingsModel>(_defaultcurrencySettings);



                if (json != null)
                {

                    defaultcurrency = new DefaultCurrencySettingsModel
                    {
                       BaseCurrency=json.BaseCurrency,
                       IsMultiCurrency = json.IsMultiCurrency,
                        


                    };

                }


                //list of languages

                ListCurrency = _globalHelper.GetCurrencyList()
                   .Where(u => u.IsCurrencyPublish == true)
                   .Select(u => new SelectListItem
                   {
                       Value = u.CurrencyCode.ToString(),
                       Text = u.CurrencyName +" " + u.CurrencyCode + " " + u.CurrencySymbol ,
                       Selected = json.BaseCurrency == u.CurrencyCode

                   }).ToList();
            }



            //GlobalAppsettings

            var _globalappsettingsSettings = _websettinghelper.GetWebsettingJson("GlobalAppSettings");

            if (_globalappsettingsSettings != null && !string.IsNullOrEmpty(_globalappsettingsSettings))
            {

                var json = JsonConvert.DeserializeObject<GlobalAppSettingsModel>(_globalappsettingsSettings);



                if (json != null)
                {

                    globalAppSettings = new GlobalAppSettingsModel
                    {

                        IsCountrySelectionEnabled = json.IsCountrySelectionEnabled,


                    };

                }
            }


            



            //dateformat

            var _dateformatSettings = _websettinghelper.GetWebsettingJson("DateFormatSettings");

            if (_dateformatSettings != null && !string.IsNullOrEmpty(_dateformatSettings))
            {

                var json = JsonConvert.DeserializeObject<DateFormatSettingsModel>(_dateformatSettings);



                if (json != null)
                {

                    dateformat = new DateFormatSettingsModel
                    {
                        DateFormat = json.DateFormat,
                        TimeFormat=json.TimeFormat
                        

                    };

                }
            }



            ///seller profile setting
            var _sellerprofileSettings = _websettinghelper.GetWebsettingJson("SellerProfileSettings");

            if (_sellerprofileSettings != null && !string.IsNullOrEmpty(_sellerprofileSettings))
            {

                var json = JsonConvert.DeserializeObject<SellerProfileSettingsModel>(_sellerprofileSettings);



                if (json != null)
                {

                    sellerprofilesettings = new SellerProfileSettingsModel
                    {
                        Profile =json.Profile,
                        BusinessInfo=json.BusinessInfo,
                       
                        SecondaryContactDetails=json.SecondaryContactDetails,
                        SecondaryAddress=json.SecondaryAddress,
                        IdentityProof=json.IdentityProof,
                        Certificate=json.Certificate ,
                        IsVideo=json.IsVideo,
                        SocialMediaLink=json.SocialMediaLink,
                        TeamMemmber=json.TeamMemmber,
                        //TotalTabsRequired=json.TotalTabsRequired
                    };

                }
            }



            ///Label Setting
            var _labelSettings = _websettinghelper.GetWebsettingJson("LabelSettings");

            if (_labelSettings != null && !string.IsNullOrEmpty(_labelSettings))
            {

                var json = JsonConvert.DeserializeObject<LabelSettingsModel>(_labelSettings);



                if (json != null)
                {

                    labelsettings = new LabelSettingsModel
                    {
                       Seller=json.Seller,
                        Buyer=json.Buyer,
                        Listing=json.Listing,
                        Individual=json.Individual,
                        Company=json.Company,
                        Address=json.Address,
                        BecomeaSeller=json.BecomeaSeller,
                        IdentityProof=json.IdentityProof,
                        Certificate=json.Certificate,
                        PostRequest=json.PostRequest,
                        SellerLogo=json.SellerLogo,
                        SellerBusiness=json.SellerBusiness,
                        HeaderSearchPlaceHolder=json.HeaderSearchPlaceHolder,
                        ViewProfile=json.ViewProfile,

                        ShippingPhone =json.ShippingPhone,
                        ShippingState=json.ShippingState,
                        ShippingCity   =json.ShippingCity,
                        ShippingLandmark=json.ShippingLandmark, 
                        ShippingStreet=json.ShippingStreet,
                        ShippingZipcode=json.ShippingZipcode,
                    };

                }
            }


            ///coookie setting

            var _cookieSettings = _websettinghelper.GetWebsettingJson("CookieSettings");

            if (_cookieSettings != null && !string.IsNullOrEmpty(_cookieSettings))
            {

                var json = JsonConvert.DeserializeObject<CookieSettingsModel>(_cookieSettings);



                if (json != null)
                {

                    cookiesetting = new CookieSettingsModel
                    {
                       IsCookie =json.IsCookie,
                       URL = json.URL,
                       CookieText=json.CookieText
                    };

                }
            }


            ///List of Language
            ///
            Listlanguagesetup = _globalHelper.GetLanguageList().ToList();
            ListCurrencies = _globalHelper.GetCurrencyList().ToList();
            ListCountries= _globalHelper.GetCountryList().OrderBy(u=>u.Name).ToList();

        }

        public IActionResult OnPostRegister()
        {


           
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(registermethod);

            if(registermethod.IsGoogleLogin==true)
            {
                

                if (registermethod.ClientId == null)
                {
                    ModelState.AddModelError("registermethod.ClientId", "Client id is required.");

                    return Page();
                }

                if (registermethod.ClientSecret == null)
                {
                    ModelState.AddModelError("registermethod.ClientSecret", "Client secret is required.");

                    return Page();
                }
            }

            if (registermethod.IsGoogleLogin == true)
            {
                if (registermethod.AppId == null )
                {
                    ModelState.AddModelError("registermethod.AppId", "App Id is required.");

                    return Page();
                }

                if ( registermethod.AppSecret == null)
                {
                    ModelState.AddModelError("registermethod.AppSecret", "App secret is required.");

                    return Page();
                }
            }

            if(registermethod.RegisterImage==null)
            {

                ModelState.AddModelError("registermethod.RegisterImage", "Register Image Height should be 650px & Width should be 470px .");

                return Page();
            }

            if (registermethod.LoginImage == null)
            {

                ModelState.AddModelError("registermethod.LoginImage", "Login Image Height should be 650px & Width should be 470px .");

                return Page();
            }

            string msg = _websettinghelper.UpdateWebsettingJson("RegisterSettings", jsonData);

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



            



            return Redirect("/admin/settings#registermethod");


        }


        //public IActionResult OnPostRegisterVerification()
        //{

        //    if (ModelState.IsValid)
        //    {
        //        // Convert the model to JSON
        //        var jsonData = JsonConvert.SerializeObject(registerverification);

        //        string msg = _websettinghelper.UpdateWebsettingJson("RegisterVerificationSettings", jsonData);

        //        if (msg == "insert")
        //        {
        //            TempData["success"] = "Inserted successfully";
        //        }

        //        if (msg == "update")
        //        {
        //            TempData["success"] = "Updated successfully";
        //        }

        //        else
        //        {
        //            TempData["success"] = msg;
        //        }



        //    }






        //    return Redirect("/admin/settings#registermethod");
        //}



       

        public IActionResult OnPostLanguage()
        {


            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(languagesetup);

            string msg = _websettinghelper.UpdateWebsettingJson("LanguageSetupSettings", jsonData);

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






            return Redirect("/admin/settings#languagesetup");
        }

        public IActionResult OnPostCurrency()
        {

           

            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(defaultcurrency);

            string msg = _websettinghelper.UpdateWebsettingJson("DefaultCurrencySettings", jsonData);

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






            return Redirect("/admin/settings#currencysetup");
        }


        public IActionResult OnPostCountry()
        {


            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(globalAppSettings);

            string msg = _websettinghelper.UpdateWebsettingJson("GlobalAppSettings", jsonData);

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






            return Redirect("/admin/settings#countriessetup");
        }
        public IActionResult OnPostDateFormat()
        {


            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(dateformat);

            string msg = _websettinghelper.UpdateWebsettingJson("DateFormatSettings", jsonData);

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






            return Redirect("/admin/settings#globalsetup");
        }


        public IActionResult OnPostSellerProfile()
        {


            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(sellerprofilesettings);

            string msg = _websettinghelper.UpdateWebsettingJson("SellerProfileSettings", jsonData);

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






            return Redirect("/admin/settings#sellerprofilesetup");
        }


        public IActionResult OnPostLabelSetting()
        {


            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(labelsettings);

            string msg = _websettinghelper.UpdateWebsettingJson("LabelSettings", jsonData);

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






            return Redirect("/admin/settings#labelingsetup");
        }


        public IActionResult OnPostCookie()
        {


            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(cookiesetting);

            string msg = _websettinghelper.UpdateWebsettingJson("CookieSettings", jsonData);

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






            return Redirect("/admin/settings#cookiesetup");
        }
    }
}
