using AMMasterProject.Helpers;
using AMMasterProject.Migrations;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using static AMMasterProject.Helpers.GlobalHelper;

namespace AMMasterProject.Pages.Seller.Profile
{

    [Authorize(Policy = "Seller")]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;
        private readonly GlobalHelper _globalhelper;

        public SellerProfileModel UserProfile { get; set; }

        public ProfileCompletionMetaData profileCompletionMetaData { get; set; }
        #endregion

        #region DI

        public IndexModel(MyDbContext context, UserHelper userHelper, GlobalHelper globalHelper)
        {
            _dbContext = context;
            _userHelper = userHelper;
            _globalhelper = globalHelper;
            profileCompletionMetaData = new ProfileCompletionMetaData();

        }

        #endregion

        #region DataPopulate    

        public void setup()
        {
            Guid ProfileGUID = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {

                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value);
                // continue with loginid variable
            }



            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

            if (up != null)
            {

                profileCompletionMetaData = _userHelper.profilecompletestatus(up.ProfileVerificationMetaData);


                UserProfile = new SellerProfileModel
                {
                    ProfileGuid=up.ProfileGuid,
                    Firstname = up.Firstname,
                    Lastname = up.Lastname,
                    BusinessName = up.BusinessName,

                    ContactNumber = up.ContactNumber,
                    Email = up.Email,
                    Address = up.Address,
                    SellerImage = up.SellerImage ?? "/images/user-image-not-found.png",
                    SellerCoverImage = up.SellerCoverImage,

                    Provider =up.Provider,
                    SellerVideoURl = up.SellerVideoURl,
                };



            }

        }
        #endregion


        public void OnGet()
        {

            setup();
        }


        public IActionResult OnPost()
        {
            try
            {

           
                #region ID
                //int loginid = 0;
                Guid profileguid = Guid.NewGuid();
                if (User.Identity.IsAuthenticated)
                {
                    //loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                    // continue with loginid variable
                    profileguid = Guid.Parse(User.FindFirst("UserGUID")?.Value ?? "0");
                }
                #endregion

                #region ModelValidation

                #endregion


                #region Up-sert
                if (ModelState.IsValid)
                {

                    UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == UserProfile.ProfileGuid);

                    if (up != null)
                    {

                        up.Firstname = UserProfile.Firstname;
                        up.Lastname = UserProfile.Lastname;
                        up.BusinessName = UserProfile.BusinessName;

                        up.ContactNumber = UserProfile.ContactNumber;
                        up.Email = UserProfile.Email;
                        up.Address = UserProfile.Address;
                        up.SellerImage = UserProfile.SellerImage ?? "/images/user-image-not-found.png";
                        up.SellerCoverImage = UserProfile.SellerCoverImage;

                        up.Provider = UserProfile.Provider;
                        up.SellerVideoURl = UserProfile.SellerVideoURl;

                        ///get address locations
                        ///
                        GeocodeResult geocodeResult =_globalhelper.GetGeocodeDetails(UserProfile.Address);

                        UserProfile.Latitude = geocodeResult.Latitude.ToString();
                        UserProfile.Longitude = geocodeResult.Longitude.ToString();
                        UserProfile.Country = geocodeResult.Country.ToString();
                        UserProfile.City = geocodeResult.City.ToString();
                        UserProfile.State= geocodeResult.State;
                        UserProfile.ZipCode = geocodeResult.Zipcode.ToString();
                        UserProfile.Country2DigitCode = geocodeResult.Country2DigitCode;
                        UserProfile.CountryImageURL = geocodeResult.CountryFlagUrlPath;
                        UserProfile.TimeZone=geocodeResult.TimeZone;

                        up.PrimaryAddressMetaData = _userHelper.addressmetadata(UserProfile.Address, UserProfile.Latitude, UserProfile.Longitude,
                            UserProfile.Country, UserProfile.City, UserProfile.State, UserProfile.ZipCode, UserProfile.CountryImageURL, UserProfile.Country2DigitCode, UserProfile.TimeZone);


                        ///if null so create json
                        if (up.ProfileVerificationMetaData == null)
                        {
                            up.ProfileVerificationMetaData = _userHelper.profilecompletionmetadata(true, false, false, false, false, false, false, false);

                        }
                        else
                        {
                            // Parse the JSON string into a dynamic object
                            dynamic jsonObject = JsonConvert.DeserializeObject(up.ProfileVerificationMetaData);

                            // Update the "Profile" value
                            jsonObject.Profile = true;

                            // Convert the updated object back to a JSON string
                            string updatedJson = JsonConvert.SerializeObject(jsonObject);

                            up.ProfileVerificationMetaData = updatedJson;
                        }


                        _dbContext.UsersProfiles.Update(up);
                        _dbContext.SaveChanges();
                       

                    }
                }

                else
                {
                    setup();
                    return Page();
                }
               
                TempData["success"] = "Profile Updated successfully";
                setup();
                return Page();
                #endregion

            }
            catch (Exception ex)
            {


                TempData["success"] = ex.Message;
                setup();
                return Page();
            }

        }




    }
}
