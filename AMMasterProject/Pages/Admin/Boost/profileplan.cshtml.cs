using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Linq;

namespace AMMasterProject.Pages.Admin.Boost
{

    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class profileplanModel : PageModel
    {
        #region Model

        private readonly WebsettingHelper _websettinghelper;

        public ProfileBoostSettingsModel profileboost { get; set; }

       

        #endregion

        #region DI


     
        public profileplanModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            profileboost = new ProfileBoostSettingsModel();
        }

        #endregion
        public void OnGet()
        {
           
            var _profileboostSettings = _websettinghelper.GetWebsettingJson("ProfileBoostSettings");
            if (_profileboostSettings != null && !string.IsNullOrEmpty(_profileboostSettings))
            {

                var json = JsonConvert.DeserializeObject<ProfileBoostSettingsModel>(_profileboostSettings);

                if (json != null)
                {

                    profileboost = new ProfileBoostSettingsModel
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

        public IActionResult OnPostProfileBoost()
        {

            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(profileboost);

                string msg = _websettinghelper.UpdateWebsettingJson("ProfileBoostSettings", jsonData);

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
