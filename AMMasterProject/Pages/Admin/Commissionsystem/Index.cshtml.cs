using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

namespace AMMasterProject.Pages.Admin.Commissionsystem
{

    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model

        private readonly WebsettingHelper _websettinghelper;
        public CommissionTaxBuyerSettingsModel CommissionBuyerSettings { get; set; }

        public CommissionTaxSellerSettingsModel CommissionSellerSettings { get; set; }

        public List<CommissionTaxBuyerSettingsModel> CommissionBuyerSettingsList { get; set; }

        public List<CommissionTaxSellerSettingsModel> CommissionSellerSettingsList { get; set; }
        #endregion


        #region DI

        public IndexModel(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            
        }
        #endregion
        public void OnGet()
        {
            ////commission setting
            ///

            var commissionBuyerSettingsJson = _websettinghelper.GetWebsettingJson("CommissionBuyerSettings");


            if (!string.IsNullOrEmpty(commissionBuyerSettingsJson))
            {
                CommissionBuyerSettingsList = JsonConvert.DeserializeObject<List<CommissionTaxBuyerSettingsModel>>(commissionBuyerSettingsJson);
            }
            else
            {
                CommissionBuyerSettingsList = new List<CommissionTaxBuyerSettingsModel>();
            }





            ////seller setting
            ///



            var commissionSellerSettingsJson = _websettinghelper.GetWebsettingJson("CommissionSellerSettings");


            if (commissionSellerSettingsJson != null)
            {
                CommissionSellerSettingsList = JsonConvert.DeserializeObject<List<CommissionTaxSellerSettingsModel>>(commissionSellerSettingsJson);

            }

            else
            {
                CommissionSellerSettingsList = new List<CommissionTaxSellerSettingsModel>();
            }



        }

      
        public IActionResult OnPostBuyerCommission()
        {

            var jsonExistingMetaData = _websettinghelper.GetWebsettingJson("CommissionBuyerSettings");



            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<CommissionTaxBuyerSettingsModel> existingMetadata = JsonConvert.DeserializeObject<List<CommissionTaxBuyerSettingsModel>>(jsonExistingMetaData ?? "[]");

            if (CommissionBuyerSettings!=null)
            {
                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata


                // Create a new instance of ContactMetaData
                var newMetadata = new CommissionTaxBuyerSettingsModel
                {
                    ID = GlobalHelper.RandomNumber(),
                    CommissionType= CommissionBuyerSettings.CommissionType.ToString(),
                    Amount = CommissionBuyerSettings.Amount,
                    Label = CommissionBuyerSettings.Label,
                    UpdatedDate =DateTime.Now,


                };

                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);
            }
           

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            _websettinghelper.UpdateWebsettingJsonList("CommissionBuyerSettings", updatedJson);


            return Redirect("/admin/commissionsystem#buyer");
        }

        public IActionResult OnPostSellerCommission()
        {
            var jsonExistingMetaData = _websettinghelper.GetWebsettingJson("CommissionSellerSettings");
            // Deserialize the existing metadata JSON string into a list of ContactMetaData
            List<CommissionTaxSellerSettingsModel> existingMetadata = JsonConvert.DeserializeObject<List<CommissionTaxSellerSettingsModel>>(jsonExistingMetaData ?? "[]");

            if (CommissionSellerSettings  != null)
            {
                // Adding a new record

                // Determine the next ContactID based on the count of existing metadata


                // Create a new instance of ContactMetaData
                var newMetadata = new CommissionTaxSellerSettingsModel
                {
                    ID = GlobalHelper.RandomNumber(),
                    CommissionType = CommissionSellerSettings.CommissionType.ToString(),
                    Amount = CommissionSellerSettings.Amount,
                    Label = CommissionSellerSettings.Label,
                    UpdatedDate = DateTime.Now,


                };

                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);
            }


            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

         

            _websettinghelper.UpdateWebsettingJsonList("CommissionSellerSettings", updatedJson);

            return Redirect("/admin/commissionsystem#seller");
        }


     
      
    }
}
