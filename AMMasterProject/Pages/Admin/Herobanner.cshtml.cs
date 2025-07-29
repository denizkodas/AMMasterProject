using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class HerobannerModel : PageModel
    {
      

        private readonly MyDbContext _dbContext;
        private readonly WebsettingHelper _websettinghelper;

        public HeroBannerSettingsViewModel herobanner { get; set; }

        public List<HeroBannerSettingsViewModel> listherobanner { get; set; }

        public HerobannerModel(WebsettingHelper websettinghelper, MyDbContext dbContext)
        {
            _websettinghelper = websettinghelper;
            _dbContext = dbContext;
            herobanner = new HeroBannerSettingsViewModel();
            herobanner.IsPublish = true;


        }
        public void setup()
        {
            var _herobannerSettings = _websettinghelper.GetWebsettingJson("HeroBannerSettings");

            if (_herobannerSettings != null && !string.IsNullOrEmpty(_herobannerSettings))
            {
                List<HeroBannerSettingsViewModel> listparse = JsonConvert.DeserializeObject<List<HeroBannerSettingsViewModel>>(_herobannerSettings);



                listherobanner = listparse.ToList();

                if (Request.Query.ContainsKey("HeroBannerGUID"))
                {
                    string GUID = Request.Query["HeroBannerGUID"];

                    var parsedData = listherobanner.FirstOrDefault(x => x.ID.ToString() == GUID);

                    if (parsedData != null)
                    {
                        herobanner = new HeroBannerSettingsViewModel
                        {
                            ID = parsedData.ID,
                         
                            Banner = parsedData.Banner,
                            IsPublish = parsedData.IsPublish,
                        };
                    }
                }



            }










        }
        public void OnGet()
        {
            setup();
        }


        public IActionResult OnPost()
        {

            try
            {




                #region Up-sert
                var _herobannerSettings = _websettinghelper.GetWebsettingJson("HeroBannerSettings");



                var jsonData = herobannermetadata(herobanner.ID, herobanner.Banner, herobanner.IsPublish, _herobannerSettings);


                Websetting existingSetting = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "HeroBannerSettings");

                if (existingSetting != null)
                {

                    existingSetting.WebsettingValue = jsonData;

                    _dbContext.Websettings.Update(existingSetting);
                    _dbContext.SaveChanges();



                }

                else
                {
                    Websetting newSetting = new Websetting();

                    newSetting.WebsettingKey = "HeroBannerSettings";
                    newSetting.WebsettingValue = jsonData;


                    _dbContext.Websettings.Add(newSetting);
                    _dbContext.SaveChanges();
                }

                TempData["success"] = "Banner Updated successfully";


                return RedirectToPage("/admin/herobanner");
                #endregion

            }
            catch (Exception ex)
            {

                TempData["success"] = ex.Message;
                setup();
                return Page();
            }





        }







        public IActionResult OnPostDelete(string id)
        {
            var _scriptmanagerSettings = _websettinghelper.GetWebsettingJson("HeroBannerSettings");

            if (_scriptmanagerSettings != null && !string.IsNullOrEmpty(_scriptmanagerSettings))
            {

                List<HeroBannerSettingsViewModel> listparse = JsonConvert.DeserializeObject<List<HeroBannerSettingsViewModel>>(_scriptmanagerSettings);

                listherobanner = listparse.ToList();
                // Find the item to be deleted
                HeroBannerSettingsViewModel itemToDelete = listherobanner.FirstOrDefault(x => x.ID == id);

                if (itemToDelete != null)
                {
                    // Remove the item from the list
                    listherobanner.Remove(itemToDelete);

                    // Serialize the updated list back to JSON
                    string updatedJson = JsonConvert.SerializeObject(listherobanner);

                    _websettinghelper.DeletedJson("HeroBannerSettings", updatedJson);

                    TempData["success"] = "Deleted successfully";
                }
            }

            return RedirectToPage("/admin/herobanner");
        }



        protected string herobannermetadata(string id, string banner,  bool ispublish, string existingMetaData)
        {
            // Deserialize the existing metadata JSON string into a list of ScriptManagerSettingsViewModel
            List<HeroBannerSettingsViewModel> existingMetadata = JsonConvert.DeserializeObject<List<HeroBannerSettingsViewModel>>(existingMetaData ?? "[]");

            if (string.IsNullOrEmpty(id))
            {
                // Adding a new record

                // Create a new instance of ScriptManagerSettingsViewModel
                var newMetadata = new HeroBannerSettingsViewModel
                {
                    ID = Guid.NewGuid().ToString(),
                    Banner = banner,
                    
                    IsPublish = ispublish
                };

                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);
            }
            else
            {
                // Updating an existing record

                // Find the existing metadata record with the matching ID
                HeroBannerSettingsViewModel existingRecord = existingMetadata.FirstOrDefault(m => m.ID == id);

                if (existingRecord != null)
                {
                    // Update the properties of the existing record
                    existingRecord.Banner = banner;
                  
                    existingRecord.IsPublish = ispublish;
                }
            }

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }



    }
}
