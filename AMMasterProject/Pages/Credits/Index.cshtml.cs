using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Credits
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

        public List<CreditPackageViewModel> creditpackagelist { get; set; }
        #endregion
        public void OnGet()
        {

            ///validate if credit system is disabled so user cannot view this page instead redirect on error page
            ///
            var _creditsystemSettings = _websettinghelper.GetWebsettingJson("CreditSystemSettings");
            if (_creditsystemSettings != null && !string.IsNullOrEmpty(_creditsystemSettings))
            {

                var json = JsonConvert.DeserializeObject<CreditSystemSettingsModel>(_creditsystemSettings);

                if (json != null)
                {
                    if(json.IsEnable ==false)
                    {
                        Response.Redirect("/Error?Title=Credit System&Message=Admin has disabled credit system.&Body=This feature is disable by admin.");
                    }


                }


            }

            GlobalHelper.SetReturnURL();
            creditpackagelist = _membershipHelper.CreditPackageList();
        }
    }
}
