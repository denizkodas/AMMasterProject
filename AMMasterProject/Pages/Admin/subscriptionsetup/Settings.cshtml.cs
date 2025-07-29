using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin.subscriptionsetup
{

    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class SettingsModel : PageModel
    {
        #region Model

        private readonly WebsettingHelper _websettinghelper;
        public SubscriptionSystemSettingsModel subscriptionsystem { get; set; }


        #endregion


        #region DI



        public SettingsModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            subscriptionsystem = new SubscriptionSystemSettingsModel();

        }

        #endregion
        public void OnGet()
        {
            var _subscriptionsystemSettings = _websettinghelper.GetWebsettingJson("SubscriptionSystemSettings");

            if (_subscriptionsystemSettings != null && !string.IsNullOrEmpty(_subscriptionsystemSettings))
            {

                var json = JsonConvert.DeserializeObject<SubscriptionSystemSettingsModel>(_subscriptionsystemSettings);



                if (json != null)
                {

                    subscriptionsystem = new SubscriptionSystemSettingsModel
                    {
                       
                        IsEnable = json.IsEnable

                    };

                }
            }
        }

        public IActionResult OnPostSubscriptionSystem()
        {


            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(subscriptionsystem);

                string msg = _websettinghelper.UpdateWebsettingJson("SubscriptionSystemSettings", jsonData);

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



            return Redirect("/admin/subscriptionsetup/Settings");


        }
    }
}
