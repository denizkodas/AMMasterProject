using Amazon.S3.Model.Internal.MarshallTransformations;
using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using PayPal.Api;

namespace AMMasterProject.Pages.Admin
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class SmsapiModel : PageModel
    {
        #region Model
        private readonly WebsettingHelper _websettinghelper;
        public TwilioSettingsModel twilio { get; set; }

        public FireBaseSettingsModel firebase { get; set; }

        #endregion

        #region DI



        public SmsapiModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            twilio = new TwilioSettingsModel();
            firebase = new FireBaseSettingsModel();
         
        }


        #endregion
        public void OnGet()
        {

            ////Twilio


            var _twilioSettings = _websettinghelper.GetWebsettingJson("TwilioSettings");
            if (_twilioSettings != null && !string.IsNullOrEmpty(_twilioSettings))
            {
                var json = JsonConvert.DeserializeObject<TwilioSettingsModel>(_twilioSettings);

                if (json != null)
                {
                    twilio = new TwilioSettingsModel
                    {
                        AccountSid=json.AccountSid,
                        AuthToken=json.AuthToken,
                        Phone=json.Phone,
                    };

                }
            }


            ///Firebase
            ///

            var _firebaseSettings = _websettinghelper.GetWebsettingJson("FireBaseSettings");
            if (_firebaseSettings != null && !string.IsNullOrEmpty(_firebaseSettings))
            {
                var json = JsonConvert.DeserializeObject<FireBaseSettingsModel>(_firebaseSettings);

                if (json != null)
                {
                    firebase = new FireBaseSettingsModel
                    {
                        apiKey = json.apiKey,
                        authDomain = json.authDomain,
                        projectId = json.projectId,
                        storageBucket = json.storageBucket,
                        messagingSenderId = json.messagingSenderId,
                        appId = json.appId,
                        measurementId = json.measurementId,
                    };

                }
            }
        }

        public IActionResult OnPostTwilio()
        {


            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(twilio);

            string msg = _websettinghelper.UpdateWebsettingJson("TwilioSettings", jsonData);

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


            return Redirect("/admin/smsapi#twiliosetup");
        }


        public IActionResult OnPostFireBase()
        {


            // Convert the model to JSON
            var jsonData = JsonConvert.SerializeObject(firebase);

            string msg = _websettinghelper.UpdateWebsettingJson("FireBaseSettings", jsonData);

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


            return Redirect("/admin/smsapi#firebasesetup");
        }

    }
}
