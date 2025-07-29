using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin.affiliate
{
    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly WebsettingHelper _websettinghelper;
        public AffiliateSettingsModel affiliatepoints { get; set; }



        #endregion

        #region DI


        public IndexModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            affiliatepoints = new AffiliateSettingsModel();
        }

        #endregion
        public void OnGet()
        {


            var _affiliatepointSettings = _websettinghelper.GetWebsettingJson("AffiliateSettings");
            if (_affiliatepointSettings != null && !string.IsNullOrEmpty(_affiliatepointSettings))
            {
                var json = JsonConvert.DeserializeObject<AffiliateSettingsModel>(_affiliatepointSettings);

                if (json != null)
                {
                    affiliatepoints = new AffiliateSettingsModel
                    {
                        AffiliatePercentage = json.AffiliatePercentage,
                        ReferalPercentage = json.ReferalPercentage,
                        IsEnable = json.IsEnable,


                    };

                }
            }

           
        }

        public IActionResult OnPostAffiliate()
        {

            string Modelname = "AffiliateSettings";


            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(affiliatepoints);

                string msg = _websettinghelper.UpdateWebsettingJson("AffiliateSettings", jsonData);

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

            return Redirect("/admin/affiliate");
        }
    }
}
