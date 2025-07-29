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
            // Label Setting
            var _labelSettings = _websettinghelper.GetWebsettingJson("LabelSettings");

            if (_labelSettings != null && !string.IsNullOrEmpty(_labelSettings))
            {
                var json = JsonConvert.DeserializeObject<LabelSettingsModel>(_labelSettings);

                if (json != null)
                {
                    // Return the LabelSettingsModel object as a JSON response
                    return Json(json);
                }
            }

            // Return an empty JSON response if label settings are not available
            return Json(new LabelSettingsModel());
        }
    }
}
