using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Linq;

namespace AMMasterProject.Pages.Admin.Advertisement
{


    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class pricingModel : PageModel
    {
        #region Model
        private readonly WebsettingHelper _websettinghelper;
        public AdvertisementBoostSettingsModel advertisementboost { get; set; }



        #endregion

        #region DI


        public pricingModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            advertisementboost = new AdvertisementBoostSettingsModel();
        }

        #endregion
        public void OnGet()
        {

            var _advertisementboostSettings = _websettinghelper.GetWebsettingJson("AdvertisementBoostSettings");
            if (_advertisementboostSettings != null && !string.IsNullOrEmpty(_advertisementboostSettings))
            {
                var json = JsonConvert.DeserializeObject<AdvertisementBoostSettingsModel>(_advertisementboostSettings);

                if (json != null)
                {
                    advertisementboost = new AdvertisementBoostSettingsModel
                    {
                        NoofDays = json.NoofDays,
                        Amount = json.Amount,
                        Credit = json.Credit,
                        DeductionType = json.DeductionType,
                        CustomAmount = json.CustomAmount,
                        CustomCredit = json.CustomCredit,
                    };

                }
            }



        }

        public IActionResult OnPostAdvertisementBoost()
        {

            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(advertisementboost);

                string msg = _websettinghelper.UpdateWebsettingJson("AdvertisementBoostSettings", jsonData);

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


            return Page();
        }
    }
}
