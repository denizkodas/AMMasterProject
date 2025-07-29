using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin
{
    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class locationapiModel : PageModel
    {
        private readonly WebsettingHelper _websettinghelper;

        public GoogleMapsApiSettingsModel googlemapapi { get; set; }


        public locationapiModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;

        }
        public void OnGet()
        {
            ////apilayer
            ///
            


            var _googlemapSettings = _websettinghelper.GetWebsettingJson("GoogleMapsApiSettings");

            if (_googlemapSettings != null && !string.IsNullOrEmpty(_googlemapSettings))
            {

                var json = JsonConvert.DeserializeObject<GoogleMapsApiSettingsModel>(_googlemapSettings);



                if (json != null)
                {

                    googlemapapi = new GoogleMapsApiSettingsModel
                    {
                        APIKey = json.APIKey,
                        IsPublish = json.IsPublish

                    };

                }
            }

        }

        public IActionResult OnPostApIGoogle()
        {

            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(googlemapapi);

                string msg = _websettinghelper.UpdateWebsettingJson("GoogleMapsApiSettings", jsonData);

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



            }






            return Redirect("/admin/locationapi");
        }
    }
}
