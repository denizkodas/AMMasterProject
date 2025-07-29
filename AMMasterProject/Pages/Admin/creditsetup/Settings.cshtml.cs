using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin.creditsetup
{
    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class SettingsModel : PageModel
    {
        #region Model

        private readonly WebsettingHelper _websettinghelper;
        public CreditSystemSettingsModel creditsystem { get; set; }

       
        #endregion


        #region DI



        public SettingsModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            creditsystem = new CreditSystemSettingsModel();
            
        }

        #endregion


        public void OnGet()
        {
            ////register method
            ///



            var _creditsystemSettings = _websettinghelper.GetWebsettingJson("CreditSystemSettings");

            if (_creditsystemSettings != null && !string.IsNullOrEmpty(_creditsystemSettings))
            {

                var json = JsonConvert.DeserializeObject<CreditSystemSettingsModel>(_creditsystemSettings);



                if (json != null)
                {

                    creditsystem = new CreditSystemSettingsModel
                    {
                        FreeCreditForBuyer = json.FreeCreditForBuyer,
                        FreeCreditForSeller = json.FreeCreditForSeller,
                        DeductionOnAddressView = json.DeductionOnAddressView,
                        DeductionOnContactView = json.DeductionOnContactView,
                        DeductionOnEmailView = json.DeductionOnEmailView,
                        IsEnable =json.IsEnable

                    };

                }
            }


        }

        public IActionResult OnPostCreditSystem()
        {


            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(creditsystem);

                string msg = _websettinghelper.UpdateWebsettingJson("CreditSystemSettings", jsonData);

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



            return Redirect("/admin/creditsetup/settings");


        }
    }
}
