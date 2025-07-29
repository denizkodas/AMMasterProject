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
    public class listingplanModel : PageModel
    {
        #region Model
        private readonly WebsettingHelper _websettinghelper;
        public ListingBoostSettingsModel listingboost { get; set; }



        #endregion

        #region DI


        public listingplanModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            listingboost = new ListingBoostSettingsModel();
        }

        #endregion
        public void OnGet()
        {

            var _listingboostSettings = _websettinghelper.GetWebsettingJson("ListingBoostSettings");
            if (_listingboostSettings != null && !string.IsNullOrEmpty(_listingboostSettings))
            {
                var json = JsonConvert.DeserializeObject<ListingBoostSettingsModel>(_listingboostSettings);

                if (json != null)
                {
                    listingboost = new ListingBoostSettingsModel
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

        public IActionResult OnPostListingBoost()
        {

            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(listingboost);

                string msg = _websettinghelper.UpdateWebsettingJson("ListingBoostSettings", jsonData);

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
