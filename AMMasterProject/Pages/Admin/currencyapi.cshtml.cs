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
    public class currencyapiModel : PageModel
    {
        private readonly WebsettingHelper _websettinghelper;

        public CurrencyAPILayerSettingsModel apilayer { get; set; }


        public currencyapiModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
       
        }
        public void OnGet()
        {
            ////apilayer
            ///



            var _apilayerSettings = _websettinghelper.GetWebsettingJson("CurrencyAPILayerSettings");

            if (_apilayerSettings != null && !string.IsNullOrEmpty(_apilayerSettings))
            {

                var json = JsonConvert.DeserializeObject<CurrencyAPILayerSettingsModel>(_apilayerSettings);



                if (json != null)
                {

                    apilayer = new CurrencyAPILayerSettingsModel
                    {
                       APIKey=json.APIKey,
                       IsPublish =json.IsPublish

                    };

                }
            }

        }

        public IActionResult OnPostApILayer()
        {

            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(apilayer);

                string msg = _websettinghelper.UpdateWebsettingJson("CurrencyAPILayerSettings", jsonData);

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






            return Redirect("/admin/currencyapi#apilayer");
        }
    }
}
