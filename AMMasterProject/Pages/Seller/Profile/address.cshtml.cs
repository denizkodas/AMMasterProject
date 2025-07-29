using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Linq.Expressions;
using static AMMasterProject.Helpers.GlobalHelper;

namespace AMMasterProject.Pages.Seller.Profile
{
    [Authorize(Policy = "Seller")]
    [BindProperties]
    public class addressModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;
        private readonly GlobalHelper _globalhelper;
        public SellerAddressModel Address { get; set; }

        public List<AddressMetaData> listAddress { get; set; }
        public ProfileCompletionMetaData profileCompletionMetaData { get; set; }
        #endregion


        #region DI
        public addressModel(MyDbContext context, UserHelper userHelper, GlobalHelper globalHelper)
        {
            _dbContext = context;

            _userHelper = userHelper;
            _globalhelper = globalHelper;

            Address = new SellerAddressModel();
            Address.IsPublish = true;

        }
        #endregion
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


                listAddress = UserHelper.ParseMetaDataAddressList(up.SecondaryAddressMetaData).ToList();

                if (Request.Query.ContainsKey("AddressGUID"))
                {
                    string GUID = Request.Query["AddressGUID"];

                    var parsedData = listAddress.FirstOrDefault(x => x.AddressGUID.ToString() == GUID);

                    if (parsedData != null)
                    {
                        Address = new SellerAddressModel
                        {
                            AddressID = parsedData.AddressID,
                            AddressGUID = parsedData.AddressGUID,
                            Address=parsedData.Address,
                            Type=parsedData.Type,
                            Latitude = parsedData.Latitude,
                            Longitude   = parsedData.Longitude,
                            Country= parsedData.Country,
                            State   = parsedData.State,
                            City    = parsedData.City,
                            ZipCode = parsedData.ZipCode
                            
                        };
                    }
                }
            }









        }


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
                Guid ProfileGUID = Guid.NewGuid();
                if (User.Identity.IsAuthenticated)
                {

                    ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value ?? "0");
                }
                #endregion

                #region ModelValidation


                #endregion


                #region Up-sert


                UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

                if (up != null)
                {

                    ///get address locations
                    ///
                    GeocodeResult geocodeResult =_globalhelper.GetGeocodeDetails(Address.Address);

                    Address.Latitude = geocodeResult.Latitude.ToString();
                    Address.Longitude = geocodeResult.Longitude.ToString();
                    Address.Country = geocodeResult.Country.ToString();
                    Address.City = geocodeResult.City.ToString();
                    Address.State = geocodeResult.State;
                    Address.ZipCode = geocodeResult.Zipcode.ToString();

                    up.SecondaryAddressMetaData = _userHelper.addressmetadata(Address.AddressGUID.ToString(), Address.AddressID.ToString(), Address.Address, Address.Type, Address.Latitude, Address.Longitude, Address.Country, Address.State, Address.City, Address.ZipCode,  up.SecondaryAddressMetaData);

                    if (up.ProfileVerificationMetaData == null)
                    {
                        up.ProfileVerificationMetaData = _userHelper.profilecompletionmetadata(false, false, false, true, false, false, false, false);

                    }
                    else
                    {
                        // Parse the JSON string into a dynamic object
                        dynamic jsonObject = JsonConvert.DeserializeObject(up.ProfileVerificationMetaData);

                        // Update the "address" value
                        jsonObject.SecondaryAddress = true;

                        // Convert the updated object back to a JSON string
                        string updatedJson = JsonConvert.SerializeObject(jsonObject);

                        up.ProfileVerificationMetaData = updatedJson;
                    }



                    _dbContext.UsersProfiles.Update(up);
                    _dbContext.SaveChanges();

                    //TempData["success"] = "Contacts Updated successfully";
                    //setup();
                    //return Page();



                }

                TempData["success"] = "Contacts Updated successfully";


                return RedirectToPage("/seller/profile/address");
                #endregion

            }
            catch (Exception ex)
            {

                TempData["success"] = ex.Message;
                setup();
                return Page();
            }
        }

        public IActionResult OnPostDelete(string id)
        {
            Guid ProfileGUID = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {
                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value ?? "0");
            }

            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

            if (up != null)
            {
                
                listAddress = UserHelper.ParseMetaDataAddressList(up.SecondaryAddressMetaData).ToList();



                

                // Find the index of the item to be deleted
                int indexToDelete = listAddress.FindIndex(x => x.AddressGUID.ToString() == id);

                if (indexToDelete >= 0)
                {
                    // Remove the item from the list
                    listAddress.RemoveAt(indexToDelete);

                    // Serialize the updated list back to JSON
                    string updatedJson = JsonConvert.SerializeObject(listAddress);

                    // Update the SecondaryContactMetaData property with the updated JSON
                    up.SecondaryAddressMetaData = updatedJson;

                    _dbContext.UsersProfiles.Update(up);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Deleted successfully";
                }
            }

            return RedirectToPage("/seller/profile/address");
        }
    }
}
