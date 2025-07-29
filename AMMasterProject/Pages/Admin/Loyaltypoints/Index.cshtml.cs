using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin.Loyaltypoints
{
    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly WebsettingHelper _websettinghelper;
        public LoyaltyPointSettingsModel loyalpoints { get; set; }



        #endregion

        #region DI


        public IndexModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            loyalpoints = new LoyaltyPointSettingsModel();
        }

        #endregion
        public void OnGet()
        {

            var _loyaltpointSettings = _websettinghelper.GetWebsettingJson("LoyaltyPointSettings");
            if (_loyaltpointSettings != null && !string.IsNullOrEmpty(_loyaltpointSettings))
            {
                var json = JsonConvert.DeserializeObject<LoyaltyPointSettingsModel>(_loyaltpointSettings);

                if (json != null)
                {
                    loyalpoints = new LoyaltyPointSettingsModel
                    {
                        PointsConversionRate = json.PointsConversionRate,
                        MinPointsRedeem = json.MinPointsRedeem,
                        PointsExpiry = json.PointsExpiry,
                        IsEnable = json.IsEnable,

                    };

                }
            }


      
        }

        public IActionResult OnPostLoyalty()
        {

          

            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(loyalpoints);

                string msg = _websettinghelper.UpdateWebsettingJson("LoyaltyPointSettings", jsonData);

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


            return Redirect("/admin/Loyaltypoints");
        }
    }
}
