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
                
                // Debug: Log file path and existence
                Console.WriteLine($"[LabelLoads] Looking for file at: {filePath}");
                Console.WriteLine($"[LabelLoads] File exists: {System.IO.File.Exists(filePath)}");
                Console.WriteLine($"[LabelLoads] Current directory: {Directory.GetCurrentDirectory()}");
                
                if (System.IO.File.Exists(filePath))
                {
                    // Force refresh - read file directly without cache
                    string json = System.IO.File.ReadAllText(filePath);
                    
                    // Log for debugging
                    Console.WriteLine($"[LabelLoads] Reading languages.json, file size: {json.Length} chars");
                    Console.WriteLine($"[LabelLoads] First 200 chars: {json.Substring(0, Math.Min(200, json.Length))}...");
                    
                    // Return raw JSON content directly
                    Console.WriteLine($"[LabelLoads] Returning raw JSON content");
                    
                    // Add cache-busting headers
                    Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
                    Response.Headers.Add("Pragma", "no-cache");
                    Response.Headers.Add("Expires", "0");
                    
                    // Return raw JSON string
                    return Content(json, "application/json");
                }
                else
                {
                    Console.WriteLine("[LabelLoads] languages.json NOT FOUND - Using fallback websetting helper");
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
