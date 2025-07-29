using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel;
using ThirdParty.Json.LitJson;

namespace AMMasterProject.Pages.Admin
{
    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class scriptmanagerModel : PageModel
    {
     
       
        private readonly MyDbContext _dbContext;
        private readonly WebsettingHelper _websettinghelper;

        public ScriptManagerSettingsViewModel scriptmanager { get; set; }

        public List<ScriptManagerSettingsViewModel> listscriptmanager { get; set; }

        public scriptmanagerModel(WebsettingHelper websettinghelper, MyDbContext dbContext)
        {
            _websettinghelper = websettinghelper;
            _dbContext = dbContext;
            scriptmanager = new ScriptManagerSettingsViewModel();
            scriptmanager.IsPublish = true;


        }
        public void setup()
        {
            var _scriptmanagerSettings = _websettinghelper.GetWebsettingJson("ScriptManagerSettings");

            if (_scriptmanagerSettings != null && !string.IsNullOrEmpty(_scriptmanagerSettings))
            {
                List<ScriptManagerSettingsViewModel> listparse = JsonConvert.DeserializeObject<List<ScriptManagerSettingsViewModel>>(_scriptmanagerSettings);



                listscriptmanager = listparse.ToList();

                if (Request.Query.ContainsKey("ScriptGUID"))
                {
                    string GUID = Request.Query["ScriptGUID"];

                    var parsedData = listscriptmanager.FirstOrDefault(x => x.ID.ToString() == GUID);

                    if (parsedData != null)
                    {
                        scriptmanager = new ScriptManagerSettingsViewModel
                        {
                            ID= parsedData.ID,
                            Name= parsedData.Name,
                            Script = parsedData.Script,
                            IsPublish= parsedData.IsPublish,
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
                var _scriptmanagerSettings = _websettinghelper.GetWebsettingJson("ScriptManagerSettings");
               


                var jsonData = scriptmanagermetadata(scriptmanager.ID,scriptmanager.Name,scriptmanager.Script, scriptmanager.IsPublish, _scriptmanagerSettings);


                Websetting existingSetting = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "ScriptManagerSettings");

                if (existingSetting != null)
                {

                    existingSetting.WebsettingValue = jsonData;

                    _dbContext.Websettings.Update(existingSetting);
                    _dbContext.SaveChanges();



                }

                else
                {
                    Websetting newSetting = new Websetting();

                    newSetting.WebsettingKey = "ScriptManagerSettings";
                    newSetting.WebsettingValue = jsonData;


                    _dbContext.Websettings.Add(newSetting);
                    _dbContext.SaveChanges();
                }

                TempData["success"] = "Script Updated successfully";


                return RedirectToPage("/admin/scriptmanager");
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
            var _scriptmanagerSettings = _websettinghelper.GetWebsettingJson("ScriptManagerSettings");

            if (_scriptmanagerSettings != null && !string.IsNullOrEmpty(_scriptmanagerSettings))
            {
                
                List<ScriptManagerSettingsViewModel> listparse = JsonConvert.DeserializeObject<List<ScriptManagerSettingsViewModel>>(_scriptmanagerSettings);

                listscriptmanager = listparse.ToList();
                // Find the item to be deleted
                ScriptManagerSettingsViewModel itemToDelete = listscriptmanager.FirstOrDefault(x => x.ID == id);

                if (itemToDelete != null)
                {
                    // Remove the item from the list
                    listscriptmanager.Remove(itemToDelete);

                    // Serialize the updated list back to JSON
                    string updatedJson = JsonConvert.SerializeObject(listscriptmanager);

                    _websettinghelper.DeletedJson("ScriptManagerSettings", updatedJson);

                    TempData["success"] = "Deleted successfully";
                }
            }

            return RedirectToPage("/admin/scriptmanager");
        }



        protected string scriptmanagermetadata(string id, string name, string script, bool ispublish, string existingMetaData)
        {
            // Deserialize the existing metadata JSON string into a list of ScriptManagerSettingsViewModel
            List<ScriptManagerSettingsViewModel> existingMetadata = JsonConvert.DeserializeObject<List<ScriptManagerSettingsViewModel>>(existingMetaData ?? "[]");

            if (string.IsNullOrEmpty(id))
            {
                // Adding a new record

                // Create a new instance of ScriptManagerSettingsViewModel
                var newMetadata = new ScriptManagerSettingsViewModel
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = name,
                    Script = script,
                    IsPublish = ispublish
                };

                // Add the new metadata to the existing list
                existingMetadata.Add(newMetadata);
            }
            else
            {
                // Updating an existing record

                // Find the existing metadata record with the matching ID
                ScriptManagerSettingsViewModel existingRecord = existingMetadata.FirstOrDefault(m => m.ID == id);

                if (existingRecord != null)
                {
                    // Update the properties of the existing record
                    existingRecord.Name = name;
                    existingRecord.Script = script;
                    existingRecord.IsPublish = ispublish;
                }
            }

            // Serialize the updated list back to JSON
            string updatedJson = JsonConvert.SerializeObject(existingMetadata);

            return updatedJson;
        }
    
    
    
    }
}
