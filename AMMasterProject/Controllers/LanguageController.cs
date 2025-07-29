using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AMMasterProject.Controllers
{
    [Route("controller/[controller]/{action}")]
    [Controller]
    public class LanguageController : Controller
    {
        private readonly WebsettingHelper _websettinghelper;

        public LanguageController(WebsettingHelper websettinghelper)
        {
            _websettinghelper = websettinghelper;
            
        }

        public IActionResult MasterLanguage()
        {
            // Read the contents of the languages.json file
            var _MasterLanguage = _websettinghelper.GetWebsettingJson("MasterLanguage");


            if (_MasterLanguage != null)
            {
                // Return the LabelSettingsModel object as a JSON response
                return Json(_MasterLanguage);
            }

            return Json("Not Loaded");
        }


        public IActionResult LoadTranslations()
        {
            // Read the contents of the languages.json file
            string json = System.IO.File.ReadAllText("languages.json");

            // Return the JSON content as a response
            return Content(json, "application/json");
        }


        public IActionResult LoadLabels(string labelname)
        {
            var _labelSettings = _websettinghelper.GetWebsettingJson("LabelSettings");

            if (_labelSettings != null && !string.IsNullOrEmpty(_labelSettings))
            {
                var json = JsonConvert.DeserializeObject<LabelSettingsModel>(_labelSettings);

                if (json != null)
                {
                    // Use a where clause to filter the properties based on the labelname
                    var labelProperty = typeof(LabelSettingsModel)
                        .GetProperties()
                        .FirstOrDefault(p => p.Name.Equals(labelname, StringComparison.OrdinalIgnoreCase));

                    if (labelProperty != null)
                    {
                        // Retrieve the value of the matching property
                        string value = labelProperty.GetValue(json)?.ToString();
                        return Content(value ?? string.Empty, "application/json");
                    }
                }
            }

            // Return an empty response
            return Content(string.Empty, "application/json");
        }
        public IActionResult LabelLoads()
        {
            try
            {
                // Read the contents of the languages.json file
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "languages.json");
                
                if (System.IO.File.Exists(filePath))
                {
                    string json = System.IO.File.ReadAllText(filePath);
                    
                    // Parse and return the JSON content
                    var translations = JsonConvert.DeserializeObject(json);
                    return Json(translations);
                }
                else
                {
                    // Fallback: Try to get from websetting helper
                    var _labelSettings = _websettinghelper.GetWebsettingJson("LabelSettings");

                    if (_labelSettings != null && !string.IsNullOrEmpty(_labelSettings))
                    {
                        var labelJson = JsonConvert.DeserializeObject<LabelSettingsModel>(_labelSettings);

                        if (labelJson != null)
                        {
                            // Return the LabelSettingsModel object as a JSON response
                            return Json(labelJson);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (you might want to use a proper logging framework)
                Console.WriteLine($"Error loading translations: {ex.Message}");
            }

            // Return an empty JSON response if translations are not available
            return Json(new Dictionary<string, object>());
        }
    }
}
