using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PayPal.Api;

namespace AMMasterProject.Pages.advertise
{

  

    [Authorize(Policy = "SellerAdmin")]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly ProductHelper _producthelper;
        private readonly UserHelper _userhelper;
        private readonly GlobalHelper _globalhelper;
        public  AdvertiseViewModel advertiseviewmodel;

        private readonly WebsettingHelper _websettinghelper;
        public ListingBoostViewModel listingboost { get; set; }

      
        public string ID { get; set; }
        public string type { get; set; }
        public string itemtype { get; set; }
        public string DateFormat { get; set; }


        public bool IsAlreadyboosted { get; set; }     //false means not boosted true means boosted 
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        #endregion

        #region DI


        public IndexModel(ProductHelper producthelper, WebsettingHelper websettinghelper, GlobalHelper globalhelper, UserHelper userhelper)
        {
            _producthelper = producthelper;

            _websettinghelper = websettinghelper;
            listingboost = new ListingBoostViewModel();
            listingboost.NoofDays = 7;
            _globalhelper = globalhelper;
            _userhelper = userhelper; 
        }

        #endregion

        public void OnSet()
        {
            if (RouteData.Values["ID"] == null || RouteData.Values["type"] == null)
            {
                // Redirect to error page or return an error response


                Response.Redirect("/Error?Title=Selection Fail For Advertise&Message=Something went wrong&Body=Please try again later.");
            }
            else
            {
                ID = (string)RouteData.Values["ID"];
                type = (string)RouteData.Values["type"];
                itemtype= (string)RouteData.Values["itemtype"];
                DateFormat = _globalhelper.Dateformat();


               

                if (itemtype == "item")
                {
                    var userselectedcurrency = _globalhelper.GetUserCurrency();
                    var conversionrate = _globalhelper.ConversionRate(userselectedcurrency);
                    string basecurrency = _globalhelper.BaseCurrency();
                    var _listingboostSettings = _websettinghelper.GetWebsettingJson("ListingBoostSettings");
                    if (_listingboostSettings != null && !string.IsNullOrEmpty(_listingboostSettings))
                    {
                        var json = JsonConvert.DeserializeObject<ListingBoostSettingsModel>(_listingboostSettings);

                        if (json != null)
                        {
                            listingboost = new ListingBoostViewModel
                            {
                                NoofDays = json.NoofDays,
                                Amount = json.Amount,
                                Credit = json.Credit,
                                DeductionType = json.DeductionType,
                                CustomAmount = json.CustomAmount,
                                CustomCredit = json.CustomCredit,

                             
                                ConversionCurrency=userselectedcurrency,
                                ConversionAutoAmount= Math.Round((decimal)json.Amount * conversionrate, 2),
                                ConversionCustomizedAmount = Math.Round((decimal)json.CustomAmount * conversionrate, 2),

                               
                            };

                        }
                    }




                    item(ID);


                }
                else if(itemtype=="profile")
                {


                    var userselectedcurrency = _globalhelper.GetUserCurrency();
                    var conversionrate = _globalhelper.ConversionRate(userselectedcurrency);
                    string basecurrency = _globalhelper.BaseCurrency();
                    var _listingboostSettings = _websettinghelper.GetWebsettingJson("ProfileBoostSettings");
                    if (_listingboostSettings != null && !string.IsNullOrEmpty(_listingboostSettings))
                    {
                        var json = JsonConvert.DeserializeObject<ListingBoostSettingsModel>(_listingboostSettings);

                        if (json != null)
                        {
                            listingboost = new ListingBoostViewModel
                            {
                                NoofDays = json.NoofDays,
                                Amount = json.Amount,
                                Credit = json.Credit,
                                DeductionType = json.DeductionType,
                                CustomAmount = json.CustomAmount,
                                CustomCredit = json.CustomCredit,


                                ConversionCurrency = userselectedcurrency,

                                ConversionAutoAmount = Math.Round((decimal)json.Amount * conversionrate, 2),
                                ConversionCustomizedAmount = Math.Round((decimal)json.CustomAmount * conversionrate, 2),


                            };

                        }
                    }

                    profile(Guid.Parse(ID));
                }


                ///first validate if item already boosted so do not show the button
                //false means not boosted true means boosted 
                object[] result = _producthelper.BoostValidation(Guid.Parse(ID), itemtype);

                bool boostExists = (bool)result[0];
            

                if (boostExists)
                {

                    string dateformat = _globalhelper.Dateformat();

                    IsAlreadyboosted = boostExists;
                    StartDate = DateTime.Parse(result[1].ToString()).ToString(dateformat);
                    EndDate= DateTime.Parse(result[2].ToString()).ToString(dateformat);
                }
                else
                {
                    Console.WriteLine("Boost does not exist.");
                }

               
            }
        }

      

        public void item(string ID)
        {

            try
            {

           
            ProductViewModel queryResult = _producthelper.productmasterdataV2(0, "guididwise", 1, 1, 0, ID.ToString()).FirstOrDefault();

            int loginid = 0;
            string usertype = "";
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                usertype = User.FindFirst("UserType")?.Value ?? "Client";
                // continue with loginid variable
            }

            if (usertype == "Vendor")
            {
                // Assuming ProductViewModel has a property called ProfileID
                queryResult = _producthelper.productmasterdataV2(0, "guididwise", 1, 1, 0, ID.ToString())
                    .Where(u => u.ProfileId == loginid)
                    .FirstOrDefault();
            }



            if (queryResult != null)
            {
                advertiseviewmodel = new AdvertiseViewModel
                {
                    ItemID=queryResult.ProductGUID,
                    Name = queryResult.ProductName,  // Replace 'queryResult.Name' with the actual property in your 'ProductData' type
                    Image = queryResult.ProductImage, // Replace 'queryResult.Image' with the actual property in your 'ProductData' type
                    Description = queryResult.ShortDescription, // Replace 'queryResult.Description' with the actual property in your 'ProductData' type
                    SeoURL=queryResult.ProductSeourl

                };

                // Now, you have a single AdvertiseViewModel instance.
            }
            else
            {
                // Handle the case where the query result is null.


                Response.Redirect("/Error?Title=Selection Fail For Advertise&Message=Something went wrong&Body=Please try again later.");
            }

            }
            catch (Exception ex)
            {
                //how to send message on error page that advertise model is null therefore its giving error
                Response.Redirect("/Error?Title=Item does not exist&Message=Item does not exist&Body=Item does not exist");
            }
        }
        public void profile(Guid ID)
        {
            try
            {

          
            UserGeneralView queryResult = _userhelper.UserGeneralByGUID(ID);

           

            if (queryResult != null)
            {
                advertiseviewmodel = new AdvertiseViewModel
                {
                    ItemID = queryResult.ProfileGuid,
                    Name = queryResult.sellerviewmodel.BusinessName,  // Replace 'queryResult.Name' with the actual property in your 'ProductData' type
                    Image = queryResult.Image, // Replace 'queryResult.Image' with the actual property in your 'ProductData' type
                    Description = queryResult.About, // Replace 'queryResult.Description' with the actual property in your 'ProductData' type
                    SeoURL = queryResult.sellerviewmodel.BusinessUrlpath

                };

                // Now, you have a single AdvertiseViewModel instance.
            }
            else
            {
                // Handle the case where the query result is null.


                Response.Redirect("/Error?Title=Selection Fail For Advertise profile&Message=Something went wrong in fetching profile data&Body=Please try again later.");
            }

            }
            catch (Exception ex)
            {

                Response.Redirect("/Error?Title=Profile does not exist&Message=Profile does not exist&Body=Profile does not exist");
            }
        }
        public void OnGet()
        {
            OnSet();
        }


        public IActionResult OnGetDateforAdvertise(int noofdays, string vstartdate)
        {
            DateTime startdate = vstartdate==null? DateTime.Now: DateTime.Parse(vstartdate.ToString());
            DateTime enddate = startdate.AddDays(noofdays);
            DateFormat = _globalhelper.Dateformat();
            // Create an anonymous object to store the date values
            var dateInfo = new
            {
                StartDate = startdate.ToString(DateFormat),
                EndDate = enddate.ToString(DateFormat)
            };

            // Return the data as JSON
            return new JsonResult(dateInfo);
        }
    }
}
