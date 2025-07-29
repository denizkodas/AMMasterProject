using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Subscriptions
{

    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region DI


        private readonly MembershipHelper _membershipHelper;
        private readonly WebsettingHelper _websettinghelper;

        public IndexModel(MembershipHelper membershipHelper, WebsettingHelper websettinghelper)
        {

            _membershipHelper = membershipHelper;
            _websettinghelper = websettinghelper;

        }
        #endregion

        #region Model

        public List<SubscriptionPackageViewModel> subscriptionpackagelist { get; set; }
        #endregion
        public void OnGet()
        {

            ///validate if subscription system is disabled so user cannot view this page instead redirect on error page
            ///
            var _subscriptionsystemSettings = _websettinghelper.GetWebsettingJson("SubscriptionSystemSettings");
            if (_subscriptionsystemSettings != null && !string.IsNullOrEmpty(_subscriptionsystemSettings))
            {

                var json = JsonConvert.DeserializeObject<SubscriptionSystemSettingsModel>(_subscriptionsystemSettings);

                if (json != null)
                {
                    if (json.IsEnable == false)
                    {
                        Response.Redirect("/Error?Title=Subscription System&Message=Admin has disabled subscription system.&Body=This feature is disable by admin.");
                    }


                }


            }
            GlobalHelper.SetReturnURL();
            subscriptionpackagelist = _membershipHelper.SubscriptionPackageList();
        }
       
    }
}
