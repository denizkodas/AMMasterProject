using AMMasterProject.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text;

namespace AMMasterProject.Pages.Admin
{
    public class LanguageUpdatesModel : PageModel
    {
        private readonly WebsettingHelper _websettinghelper;
        private readonly MyDbContext _dbContext;
        public LanguageUpdatesModel(WebsettingHelper websettinghelper, MyDbContext dbContext)
        {
            _websettinghelper = websettinghelper;
            _dbContext = dbContext;
            


        }
        public void OnGet()
        {
            // Step 1: Get the existing language JSON
            var existingLanguageJson = _websettinghelper.GetWebsettingJson("MasterLanguage");

            // Step 2: Get the new language JSON file
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "masterlanguageV2.json");

            // Read the JSON file content
            var newLanguageJsonContent = System.IO.File.ReadAllText(path, Encoding.UTF8); // Fully qualify the File class

            // Step 3: Merge the existing JSON with the new JSON
            var mergedJson = MergeJson(existingLanguageJson, newLanguageJsonContent);

            // Step 4: Update the JSON in the database
            var message = _websettinghelper.UpdateWebsettingJson("MasterLanguage", mergedJson);
        }

        private string MergeJson(string existingJson, string newJson)
        {
            var existingJObject = JObject.Parse(existingJson);
            var newJObject = JObject.Parse(newJson);

            existingJObject.Merge(newJObject, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });

            return existingJObject.ToString();
        }
    }
}
