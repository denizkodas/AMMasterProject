using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin
{
    [Authorize(Policy = "Admin")]
    [BindProperties]
    public class masterlanguageModel : PageModel
    {
        #region Model
        private readonly WebsettingHelper _websettinghelper;
        private readonly MyDbContext _dbcontenxt;

        public List<MasterLangaugeSettingModel> LanguageTexts { get; set; }
        #endregion




        #region DI
        public masterlanguageModel(WebsettingHelper websettinghelper, MyDbContext myDbContext)
        {
            _websettinghelper = websettinghelper;
            _dbcontenxt = myDbContext;
        }


        #endregion
        public void OnGet()
        {
            LoadLanguageTexts();
        }

        private void LoadLanguageTexts()
        {
            var _masterlanguageSettings = _websettinghelper.GetWebsettingJson("MasterLanguage");

            var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(_masterlanguageSettings);

            LanguageTexts = new List<MasterLangaugeSettingModel>();

            foreach (var item in data)
            {
                LanguageTexts.Add(new MasterLangaugeSettingModel
                {
                    Key = item.Key,
                    Translations = item.Value
                });
            }
        }
        public IActionResult OnPost(string key, Dictionary<string, string> translations)
        {
            var websetting = _dbcontenxt.Websettings.FirstOrDefault(u => u.WebsettingKey == "MasterLanguage");

            if (websetting != null)
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(websetting.WebsettingValue);

                if (data.ContainsKey(key))
                {
                    data[key] = translations;
                    websetting.WebsettingValue = JsonConvert.SerializeObject(data, Formatting.Indented);
                    _dbcontenxt.SaveChanges();
                }

                // Reload the data to reflect changes
                LoadLanguageTexts();

                return RedirectToPage(); // Redirect to avoid reposting the form on refresh
            }

            return Page();
        }


        public IActionResult OnPostAddKey(string newKey)
        {
            var websetting = _dbcontenxt.Websettings.FirstOrDefault(u => u.WebsettingKey == "MasterLanguage");

            if (websetting != null)
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(websetting.WebsettingValue);

                if (!data.ContainsKey(newKey))
                {
                    var allLanguages = data.Values.SelectMany(x => x.Keys).Distinct();
                    var newTranslations = allLanguages.ToDictionary(language => language, language => string.Empty);

                    data[newKey] = newTranslations;
                    websetting.WebsettingValue = JsonConvert.SerializeObject(data, Formatting.Indented);
                    _dbcontenxt.SaveChanges();
                }

                LoadLanguageTexts();

                return RedirectToPage();
            }

            return Page();
        }
    }
}
