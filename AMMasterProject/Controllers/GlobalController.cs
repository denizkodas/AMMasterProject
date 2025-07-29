using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;

using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using System;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AMMasterProject.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]

   
    [Route("api/[controller]/{action}")]
    [Controller]
    [AjaxOnly]
    public class GlobalController : Controller
    {

        private readonly MyDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly UserHelper _userHelper;
        private readonly WebsettingHelper _websettinghelper;
        public GlobalController(MyDbContext dbContext, IMemoryCache memoryCache, UserHelper userHelper, WebsettingHelper websettinghelper)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _userHelper = userHelper;
            _websettinghelper = websettinghelper;
        }
        // GET: api/<GlobalController>



        #region Setup


        


        //[HttpGet("designby")]

        //public IActionResult DesignBy()
        //{
        //    try
        //    {
        //        string url = "";
        //        var themeselection = _dbContext.WebsiteSetupThemeSelections.Where(u => u.IsActive == true).FirstOrDefault();


        //        //ezycommerce
        //        if (themeselection.WebsiteThemeSelectionId == 1)
        //        {
        //            // Do something

        //            url = "https://amtechnology.info/ezycommerce";
        //        }

        //        //propertyzone
        //        else if (themeselection.WebsiteThemeSelectionId == 2)
        //        {
        //            // Do something

        //            url = "https://amtechnology.info/propertyzone";
        //        }

        //        //servicemate
        //        else if (themeselection.WebsiteThemeSelectionId == 3)
        //        {
        //            // Do something
        //            url = "https://amtechnology.info/servicemate";
        //        }

        //        //pennyauction
        //        else if (themeselection.WebsiteThemeSelectionId == 4)
        //        {
        //            // Do something

        //            url = "https://amtechnology.info/pennyauction";
        //        }

        //        //foodfinder
        //        else if (themeselection.WebsiteThemeSelectionId == 5)
        //        {
        //            // Do something

        //            url = "https://amtechnology.info/foodfinder";
        //        }

        //        //foodcart
        //        else if (themeselection.WebsiteThemeSelectionId == 6)
        //        {
        //            // Do something

        //            url = "https://amtechnology.info/foodcart";
        //        }

        //        //grocerymart
        //        else if (themeselection.WebsiteThemeSelectionId == 7)
        //        {
        //            // Do something
        //            url = "https://amtechnology.info/grocerymart";
        //        }

        //        return Content(url ?? "");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        // Return an error response
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting DesignBy");
        //    }
        //}


        #region License


        [EnableCors("AllowAMTechOriginPolicy")]
        [HttpPost]
        public IActionResult UpdateLicense(string updatetype, string licensekey, string brandkey, DateTime licenseactivationdate, DateTime licenseexpirydate, DateTime brandremovalactivationdate, DateTime brandremovalexpirydate)
        {
            //update type   licenseremoval, brandremoval, licenseupdate, brandupdate

            LicenseAppSettingsModel licensesettings = new LicenseAppSettingsModel();
            var _licenseSettings = _websettinghelper.GetWebsettingJson("LicenseAppSettings");

            if (_licenseSettings != null && !string.IsNullOrEmpty(_licenseSettings))
            {

                var json = JsonConvert.DeserializeObject<LicenseAppSettingsModel>(_licenseSettings);

                if(updatetype == "licenseremoval")
                {
                    

                        licensesettings = new LicenseAppSettingsModel
                        {
                            LicenseKey = null,
                            ActivationDate=null,
                            ExpiryDate =null,
                            LicenseKeyForBrandRemoval =null,
                            BrandRemovalActivationDate =null,
                            BrandRemovalExpiryDate =null,
                        };

                    

                }
                else if(updatetype == "brandremoval")
                {

                    licensesettings = new LicenseAppSettingsModel
                    {
                        LicenseKey = json.LicenseKey,
                        ActivationDate = json.ActivationDate,
                        ExpiryDate = json.ExpiryDate,

                        LicenseKeyForBrandRemoval = null,
                        BrandRemovalActivationDate = null,
                        BrandRemovalExpiryDate = null,
                    };
                }

                else if (updatetype == "licenseupdate")
                {

                    licensesettings = new LicenseAppSettingsModel
                    {
                        LicenseKey = licensekey,
                        ActivationDate = licenseactivationdate,
                        ExpiryDate = licenseexpirydate,

                        LicenseKeyForBrandRemoval = null,
                        BrandRemovalActivationDate = null,
                        BrandRemovalExpiryDate = null,
                    };
                }

                else if (updatetype == "brandupdate")
                {
                    licensesettings = new LicenseAppSettingsModel
                    {
                        LicenseKey = json.LicenseKey,
                        ActivationDate = json.ActivationDate,
                        ExpiryDate = json.ExpiryDate,

                        LicenseKeyForBrandRemoval = brandkey,
                        BrandRemovalActivationDate = brandremovalactivationdate,
                        BrandRemovalExpiryDate = brandremovalexpirydate,
                    };
                }

              

            }


            //update command

            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(licensesettings);

            string msg = _websettinghelper.UpdateWebsettingJson("LicenseAppSettings", jsonData);
            return Content(msg);
        }




        [EnableCors("AllowAMTechOriginPolicy")]
      
        public IActionResult getlicense()
        {
            //update type   licenseremoval, brandremoval, licenseupdate, brandupdate

            LicenseAppSettingsModel licensesettings = new LicenseAppSettingsModel();
            var _licenseSettings = _websettinghelper.GetWebsettingJson("LicenseAppSettings");

            if (_licenseSettings != null && !string.IsNullOrEmpty(_licenseSettings))
            {

                var json = JsonConvert.DeserializeObject<LicenseAppSettingsModel>(_licenseSettings);


                licensesettings = new LicenseAppSettingsModel
                {
                    LicenseKey = json.LicenseKey,
                    ActivationDate = json.ActivationDate,
                    ExpiryDate = json.ExpiryDate,
                    LicenseKeyForBrandRemoval = json.LicenseKeyForBrandRemoval,
                    BrandRemovalActivationDate = json.BrandRemovalActivationDate,
                    BrandRemovalExpiryDate = json.BrandRemovalExpiryDate,
                };
            }


            return Json(licensesettings);

        }

        #endregion







        //[HttpGet("countrylist")]
        //public IActionResult CountryList()
        //{
        //    try
        //    {
        //        List<CountryCode> countylist = new List<CountryCode>();


        //        countylist = (from cc in _dbContext.CountryCodes
        //                      where cc.CActive == true && cc.Currencycode != null
        //                      orderby cc.CountryName
        //                      select cc).ToList();




        //        return new JsonResult(countylist);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        // Return an error response
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting CountryList");
        //    }
        //}








        #endregion




        #region Category







        #endregion











        #region Users
        //[HttpGet("sellerlist")]

        //public IActionResult SellerList()
        //{
        //    try
        //    {
        //        List<UsersProfile> sellerlist = new List<UsersProfile>();

        //        sellerlist = (from up in _dbContext.UsersProfiles
        //                      join pb in _dbContext.ProductBasicV2s on up.ProfileId equals pb.ProfileId
        //                      where up.Type == "Vendor"
        //                      select up).Distinct().ToList();





        //        return new JsonResult(sellerlist);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        // Return an error response
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred while getting SellerList");
        //    }
        //}







        #endregion

        #region DateFormat

        //[HttpPost]
        public IActionResult DateFormat()
        {
            string df = "MMM dd, yyyy";

            var _dateformatSettings = _websettinghelper.GetWebsettingJson("DateFormatSettings");

            if (_dateformatSettings != null && !string.IsNullOrEmpty(_dateformatSettings))
            {

                var json = JsonConvert.DeserializeObject<DateFormatSettingsModel>(_dateformatSettings);



                if (json != null)
                {
                    df = json.DateFormat;

                }
            }

            return Content(df);
        }
        #endregion


    }
}
