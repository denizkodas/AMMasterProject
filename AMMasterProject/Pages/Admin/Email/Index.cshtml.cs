using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin.Email
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model

        private readonly WebsettingHelper _websettinghelper;
        public EmailSettingsModel emailsetting { get; set; }


        #endregion

        #region DI
       
        public IndexModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            emailsetting = new EmailSettingsModel();
           
        }
        #endregion

        public void OnGet()
        {

            var _emailSettings = _websettinghelper.GetWebsettingJson("EmailSettings");
            if (_emailSettings != null && !string.IsNullOrEmpty(_emailSettings))
            {

                var json = JsonConvert.DeserializeObject<EmailSettingsModel>(_emailSettings);


                if (json != null)
                {
                    emailsetting = new EmailSettingsModel
                    {
                        FromEmail = json.FromEmail,
                        Password = json.Password,
                        Port = json.Port,
                        SMTP = json.SMTP,
                        EnableSSL = json.EnableSSL,
                        BCC = json.BCC

                    };

                }
            }

              
        }

        public IActionResult OnPostEmail()
        {

            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(emailsetting);

                string msg = _websettinghelper.UpdateWebsettingJson("EmailSettings", jsonData);

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

            return Redirect("/admin/email/index");


          
        }
    }
}
