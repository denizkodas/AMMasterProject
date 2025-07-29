using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Linq.Expressions;
using ThirdParty.Json.LitJson;
using static AMMasterProject.Helpers.GlobalHelper;

namespace AMMasterProject.Pages.Admin
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class locationsetupModel : PageModel
    {

        #region DI
        private readonly MyDbContext _dbContext;
        private readonly WebsettingHelper _websettinghelper;
        private readonly GlobalHelper _globalhelper;

        public AdminAddressModel Address { get; set; }

        public List<AdminAddressMetaData> listAddress { get; set; }


        public locationsetupModel(WebsettingHelper websettinghelper, MyDbContext dbContext, GlobalHelper globalhelper)
        {
            _websettinghelper = websettinghelper;
            _dbContext = dbContext;
            _globalhelper = globalhelper;
        }
        #endregion

        public void setup()
        {
            var _addressSettings = _websettinghelper.GetWebsettingJson("AdminLocationSettings");

            if (_addressSettings != null && !string.IsNullOrEmpty(_addressSettings))
            {
                listAddress = _websettinghelper.ParseMetaDataAddressList(_addressSettings).ToList();

                if (Request.Query.ContainsKey("AddressGUID"))
                {
                    string GUID = Request.Query["AddressGUID"];

                    var parsedData = listAddress.FirstOrDefault(x => x.AddressGUID.ToString() == GUID);

                    if (parsedData != null)
                    {
                        Address = new AdminAddressModel
                        {
                            AddressID = parsedData.AddressID,
                            AddressGUID = parsedData.AddressGUID,
                            Address = parsedData.Address,
                            Type = parsedData.Type,
                            StoreName= parsedData.StoreName,
                            ContactNumber   = parsedData.ContactNumber,
                            Email = parsedData.Email,
                            Latitude = parsedData.Latitude,
                            Longitude = parsedData.Longitude,
                            Country = parsedData.Country,
                            State = parsedData.State,
                            City = parsedData.City,
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




                #region Up-sert
                var _addressSettings = _websettinghelper.GetWebsettingJson("AdminLocationSettings");
                ///get address locations
                ///
                GeocodeResult geocodeResult = _globalhelper.GetGeocodeDetails(Address.Address);

                Address.Latitude = geocodeResult.Latitude.ToString();
                Address.Longitude = geocodeResult.Longitude.ToString();
                Address.Country = geocodeResult.Country.ToString();
                Address.City = geocodeResult.City.ToString();
                Address.State = geocodeResult.State;
                Address.ZipCode = geocodeResult.Zipcode.ToString();


                var jsonData = _websettinghelper.addressmetadata(Address.AddressGUID.ToString(), Address.AddressID.ToString(), Address.Address, Address.Type, Address.StoreName, Address.ContactNumber, Address.Email, Address.Latitude, Address.Longitude, Address.Country, Address.State, Address.City, Address.ZipCode, _addressSettings);


                Websetting existingSetting = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "AdminLocationSettings");

                if (existingSetting != null)
                {

                    existingSetting.WebsettingValue = jsonData;

                    _dbContext.Websettings.Update(existingSetting);
                    _dbContext.SaveChanges();



                }

                else
                {
                    Websetting newSetting = new Websetting();

                    newSetting.WebsettingKey = "AdminLocationSettings";
                       newSetting.WebsettingValue = jsonData;
                    

                    _dbContext.Websettings.Add(newSetting);
                    _dbContext.SaveChanges();
                }

                TempData["success"] = "Location Updated successfully";


                return RedirectToPage("/admin/locationsetup");
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
            var _addressSettings = _websettinghelper.GetWebsettingJson("AdminLocationSettings");

            if (_addressSettings != null && !string.IsNullOrEmpty(_addressSettings))
            {



                listAddress = _websettinghelper.ParseMetaDataAddressList(_addressSettings).ToList();






                // Find the index of the item to be deleted


                AdminAddressMetaData itemToDelete = listAddress.FirstOrDefault(x => x.AddressGUID.ToString() == id);

                if (itemToDelete !=null)
                {
                    // Remove the item from the list
                   
                    listAddress.Remove(itemToDelete);
                    // Serialize the updated list back to JSON
                    string updatedJson = JsonConvert.SerializeObject(listAddress);

                    

                    _websettinghelper.DeletedJson("AdminLocationSettings", updatedJson);

                    TempData["success"] = "Deleted successfully";
                }
            }

            return RedirectToPage("/admin/locationsetup");
        }





    }
}
