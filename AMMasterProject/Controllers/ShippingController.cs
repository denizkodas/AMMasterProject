using Amazon.Runtime.Internal.Util;
using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net;
using static AMMasterProject.Helpers.GlobalHelper;

namespace AMMasterProject.Controllers
{

    [Route("controller/[controller]/{action}")]
    [Controller]
    [Authorize]
    [BindProperties]
    public class ShippingController : Controller
    {

        #region Model
        private readonly MyDbContext _dbContext;
        private readonly GlobalHelper _globalhelper;
        private readonly UserHelper _userHelper;


        public List<CustomerAddress> _customeraddresslist { get; set; }

      
        public CustomerAddress _customeraddress { get; set; }

        private readonly WebsettingHelper _websettinghelper;
        #endregion

        #region DI

        public ShippingController(MyDbContext context, GlobalHelper globalHelper, UserHelper userHelper , WebsettingHelper websetting)
        {
            _dbContext = context;
            _globalhelper = globalHelper;
            _userHelper = userHelper;
            _websettinghelper = websetting;

            _customeraddress = new CustomerAddress();
            _customeraddress.IsActive = true;
            _customeraddress.AddressType = "Home";

           
        }


        #endregion


        #region View


        public IActionResult shippingview(Guid CustomerAddressGuid)
        {
            int loginid = 0;
            Guid Userguid= Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                Userguid = Guid.Parse(User.FindFirst("UserGUID")?.Value);
                // continue with loginid variable
            }


            CustomerAddress ca = _dbContext.CustomerAddresses.FirstOrDefault(u => u.BuyerId == loginid && u.CustomerAddressGuid == CustomerAddressGuid);

            if (ca != null)
            {
                _customeraddress = ca;
            }
            else
            {
              CustomerAddress cavalidate = _dbContext.CustomerAddresses.FirstOrDefault(u => u.BuyerId == loginid);

               UserGeneralView user = _userHelper.UserGeneralByGUID(Userguid);

                _customeraddress = new CustomerAddress
                {
                    FirstName = user.FirstName,
                    
                    Email=user.Email,
                    Phone = cavalidate!=null ? cavalidate.Phone:null,
                    AddressType =  "Home",

                };

            }


            var _shippingSettings = _websettinghelper.GetWebsettingJson("ShippingFormSettings");

            if (_shippingSettings != null && !string.IsNullOrEmpty(_shippingSettings))
            {
                var json = JsonConvert.DeserializeObject<ShippingFormSettingsModel>(_shippingSettings);

                if (json != null)
                {
                    ViewBag.IsZipCodeHide = json.IsZipCodeHide;
                    ViewBag.IsStreetHide = json.IsStreetHide;
                    // Add other properties as needed
                }
            }


            return PartialView("/Pages/shipping/_shippingform.cshtml", _customeraddress);
        }
        #endregion
        #region list



        public IActionResult shippinglist()

        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }


            _customeraddresslist = _dbContext.CustomerAddresses.Where(u => u.BuyerId == loginid && u.IsActive ==true).ToList();

            return PartialView("/Pages/shipping/_shippinglist.cshtml", _customeraddresslist);
        }
        #endregion

        #region Up-Sert

        [HttpPost]
        public async Task<IActionResult> shippingupsert(CustomerAddress customerAddress)
        {
            try
            {

                //if (!ModelState.IsValid)
                //{
                //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //    return Json("Model is not valid");
                //}


                string msg = string.Empty;
                int loginid = 0;
                if (User.Identity.IsAuthenticated)
                {
                    loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                    // continue with loginid variable
                }

                GeocodeResult geocodeResult = _globalhelper.GetGeocodeDetails(customerAddress.Address);

                customerAddress.Latitude = geocodeResult.Latitude.ToString();
                customerAddress.Longitude = geocodeResult.Longitude.ToString();
                //customerAddress.Country = geocodeResult.Country.ToString();
                //customerAddress.City = geocodeResult.City.ToString();
                //customerAddress.State = geocodeResult.State;
                //customerAddress.Zipcode = geocodeResult.Zipcode.ToString();


                CustomerAddress validate = _dbContext.CustomerAddresses.FirstOrDefault(u => u.CustomerAddressGuid == customerAddress.CustomerAddressGuid && u.BuyerId == loginid);

                if (validate != null)
                {

                    validate.UpdatedDate = DateTime.Now;
                    validate.FirstName = customerAddress.FirstName;
                    validate.LastName = customerAddress.LastName;
                    validate.Email = customerAddress.Email;
                    validate.Phone = customerAddress.Phone;
                    validate.Address = customerAddress.Address;
                    validate.HouseNumber = customerAddress.HouseNumber;
                    validate.City = customerAddress.City;
                    validate.Country = customerAddress.Country;
                    validate.State = customerAddress.State;
                    validate.Zipcode = customerAddress.Zipcode;
                    validate.Street  = customerAddress.Street;
                    validate.Latitude = customerAddress.Latitude;
                    validate.Longitude = customerAddress.Longitude;
                    validate.IsActive = true;
                    validate.AddressType  = customerAddress.AddressType;

                    _dbContext.CustomerAddresses.Update(validate);
                    _dbContext.SaveChanges();
                    msg = "success";

                }

                else
                {

                    customerAddress.BuyerId = loginid;
                    customerAddress.InsertDate = DateTime.Now;
                    customerAddress.IsActive = true;

                    _dbContext.CustomerAddresses.Add(customerAddress);
                    _dbContext.SaveChanges();
                    msg = "success";

                }





                return Json(msg);
            }
            catch (Exception ex)
            {
                return BadRequest("Error shippingupsert: " + ex.Message);
            }
            finally
            {
                _dbContext.Dispose();
            }
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> shippingdelete(Guid CustomerAddressGUID)
        {
            try
            {
                int loginid = 0;
                if (User.Identity.IsAuthenticated)
                {
                    loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                    // continue with loginid variable
                }


                CustomerAddress validate = _dbContext.CustomerAddresses.FirstOrDefault(u => u.CustomerAddressGuid == CustomerAddressGUID && u.BuyerId == loginid);

                if (validate != null)
                {

                    validate.IsActive = false;
                    _dbContext.CustomerAddresses.Update(validate);
                    _dbContext.SaveChanges();


                }







                return Json("success");
            }
            catch (Exception ex)
            {
                return BadRequest("Error shippingdelete: " + ex.Message);
            }
            finally
            {
                _dbContext.Dispose();
            }
        }

        #endregion
    }
}
