using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin
{

    [Authorize(Policy = "Admin")]
    [BindProperties]
    public class pagecontrolsettingModel : PageModel
    {

        #region Model

        private readonly WebsettingHelper _websettinghelper;

        private readonly GlobalHelper _globalHelper;

        public ShippingFormSettingsModel shippingForm { get; set; }

        #endregion


        #region DI



        public pagecontrolsettingModel(WebsettingHelper websettinghelper, GlobalHelper globalHelper)
        {
            _websettinghelper = websettinghelper;
            _globalHelper = globalHelper;
           

        }

        #endregion

        public void OnGet()
        {
            var _shippingSettings = _websettinghelper.GetWebsettingJson("ShippingFormSettings");

            if (_shippingSettings != null && !string.IsNullOrEmpty(_shippingSettings))
            {

                var json = JsonConvert.DeserializeObject<ShippingFormSettingsModel>(_shippingSettings);



                if (json != null)
                {

                    shippingForm = new ShippingFormSettingsModel
                    {
                        IsZipCodeHide = json.IsZipCodeHide,
                        IsStreetHide = json.IsStreetHide,
                    };

                }
            }
        }

        public IActionResult OnPostShipping()
        {



            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(shippingForm);

            



            string msg = _websettinghelper.UpdateWebsettingJson("ShippingFormSettings", jsonData);

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







            return Redirect("/admin/pagecontrolsetting#shippingform");


        }
    }
}
