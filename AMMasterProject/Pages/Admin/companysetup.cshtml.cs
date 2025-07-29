using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AMMasterProject.Pages.Admin
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class companysetupModel : PageModel
    {

        #region Models
        //private readonly MyDbContext _dbContext;
        private readonly WebsettingHelper _websettinghelper;

        public CompanySetupModel CompanySetup { get; set; }

        #endregion

        #region DI

        public companysetupModel(WebsettingHelper websettinghelper)
        {
            //_dbContext = context;
            _websettinghelper = websettinghelper;
            CompanySetup = new CompanySetupModel();


        }

        #endregion

        #region DataPopulate    

        public void setup()
        {
            //CompanySetup companysetup = _dbContext.CompanySetups.FirstOrDefault();

          
            string websetting = _websettinghelper.GetWebsettingJson("CompanySetupSettings");

           
            if (websetting != null && !string.IsNullOrEmpty(websetting))
            {
                var companysetup = JsonConvert.DeserializeObject<CompanySetupModel>(websetting);

                if (companysetup != null)
                {
                    CompanySetup = new CompanySetupModel
                    {
                        Logo = companysetup.Logo,
                        Favicon = companysetup.Favicon,
                        CompanyName = companysetup.CompanyName,
                        CompanyDescription = companysetup.CompanyDescription,
                        CompanyAddress = companysetup.CompanyAddress,
                        SupportContact = companysetup.SupportContact,
                        SupportEmail = companysetup.SupportEmail,
                        MetaTitle = companysetup.MetaTitle,
                        MetaDescription = companysetup.MetaDescription,
                        MetaKeyword = companysetup.MetaKeyword,
                        IsMultiVendor= companysetup.IsMultiVendor
                    };
                }

                // Rest of the code to set up the CompanySetup model
            }
            else
            {
                // Handle the case when the JSON value is not found
            }

        }
        #endregion

        public void OnGet()
        {
            setup();
        }

       

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                // Convert the model to JSON
                var jsonData = JsonConvert.SerializeObject(CompanySetup);

                string msg = _websettinghelper.UpdateWebsettingJson("CompanySetupSettings", jsonData);

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


                setup(); // Refresh the setup values
            }

            return Page();
        }
    }
}
