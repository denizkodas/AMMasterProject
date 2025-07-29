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
    public class serviceplanModel : PageModel
    {
        #region Model

        private readonly WebsettingHelper _websettinghelper;
        public ServiceBoostSettingsModel serviceboost { get; set; }



        #endregion

        #region DI


        public serviceplanModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            serviceboost = new ServiceBoostSettingsModel();
        }

        #endregion
        public void OnGet()
        {

            var _serviceboostSettings = _websettinghelper.GetWebsettingJson("ServiceBoostSettings");
            if (_serviceboostSettings != null && !string.IsNullOrEmpty(_serviceboostSettings))
            {
                var json = JsonConvert.DeserializeObject<ServiceBoostSettingsModel>(_serviceboostSettings);

                if (json != null)
                {

                    serviceboost = new ServiceBoostSettingsModel
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

        public IActionResult OnPostServiceBoost()
        {
            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(serviceboost);

                string msg = _websettinghelper.UpdateWebsettingJson("ServiceBoostSettings", jsonData);

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
