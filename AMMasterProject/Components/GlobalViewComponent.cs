using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AMMasterProject.Components
{
    public class GlobalViewComponent : ViewComponent
    {

        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _cache;
        private readonly WebsettingHelper _websettinghelper;
        private readonly GlobalHelper _globalHelper;
        public GlobalViewComponent(MyDbContext context, IMemoryCache cache, WebsettingHelper websettinghelper, GlobalHelper globalHelper)
        {
            _dbcontext = context;
            _cache = cache;
            _websettinghelper = websettinghelper;
            _globalHelper = globalHelper;
        }


        [ResponseCache(Duration = 86400)]
        public async Task<IViewComponentResult> InvokeAsync(string viewName, string methodname, int id, string value)
        {
            object model = null;
           

            if (methodname == "cms")
            {

                try
                {
                

                        model = await
                    (from p in _dbcontext.PageNames


                     where p.CMSKey == value.ToString()
                     select new PageDetailViewModel
                     {
                         PageDescription = p.PageDescription ?? "",

                     }).FirstAsync();


                        
                   

                }
                catch (Exception ex)
                {

                    model = new PageDetailViewModel
                    {
                        PageDescription = "CMS not exist name = " + id + value
                    };
                }

            }
            else if (methodname == "header")
            {

             
                    model = await _dbcontext.PageNames
                    .Where(u => u.IsPublish == true && (u.Type == "Header" || u.Type == "Both"))
                    .OrderBy(u => u.Sortnumber)

                    .Select(u => new PageName { IsUrl = u.IsUrl, Url = u.Url, Name = u.Name, SeoPageName = u.SeoPageName, PageCategoryId = u.PageCategoryId })

                    .ToListAsync();



            }

            else if (methodname == "footer")
            {
              
                    var footers1 = await _dbcontext.PageCategories
            .Where(u => u.IsPublish == true)
               .OrderBy(u => u.Category)
             .Select(u => new PageCategory { PageCategoryId = u.PageCategoryId, Category = u.Category })

             .ToListAsync();

                    var footers2 = await _dbcontext.PageNames
                        .Where(u => u.IsPublish == true && (u.Type == "Footer" || u.Type == "Both"))
                         .OrderBy(u => u.Sortnumber)
                        .Select(u => new PageName { IsUrl = u.IsUrl, Url = u.Url, Name = u.Name, SeoPageName = u.SeoPageName, PageCategoryId = u.PageCategoryId })

                        .ToListAsync();

                    var viewModel = new FooterViewModel
                    {
                        Footer1 = footers1,
                        Footer2 = footers2
                    };


                    model = viewModel;

                   
            }

            else if (methodname == "cookie")
            {
                if (GlobalHelper.ReadCookie("cookiepolicy") == null)
                {

                    var _cookieSettings = _websettinghelper.GetWebsettingJson("CookieSettings");

                    if (_cookieSettings != null && !string.IsNullOrEmpty(_cookieSettings))
                    {

                        var json = JsonConvert.DeserializeObject<CookieSettingsModel>(_cookieSettings);

                        if (json != null)
                        {
                            model = json;
                        }

                    }

                }
            }

            else if (methodname == "partner")
            {
               

                    model = await (from wss in _dbcontext.WebsitesetupPartners
                                   where wss.IsPublish == true && wss.Isaddonhomepage == true
                                   select wss).ToListAsync();

                  

                

            }
            else if (methodname == "sliderhome")
            {
              

                    var _herobannerSettings = _websettinghelper.GetWebsettingJson("HeroBannerSettings");

                    if (_herobannerSettings != null && !string.IsNullOrEmpty(_herobannerSettings))
                    {
                        List<HeroBannerSettingsViewModel> listparse = JsonConvert.DeserializeObject<List<HeroBannerSettingsViewModel>>(_herobannerSettings);



                        model = listparse.Where(u => u.IsPublish == true).ToList();


                        
                    }


            }

            else if (methodname == "socialmediafooter")
            {
               

                    model = await _dbcontext.WebsiteSetupSocialMedia.Where(u => u.IsPublish == true).ToListAsync();



            }


            else if (methodname == "scripts")
            {
                
                    model = _globalHelper.GetScriptCustom();

                    
                

            }

            else if (methodname == "regionalsetting")
            {
               
                    var _languagesetupSettings = _websettinghelper.GetWebsettingJson("LanguageSetupSettings");
                    var ismultilingual = JsonConvert.DeserializeObject<LanguageSetupSettingsModel>(_languagesetupSettings);

                    var _currencysetupSettings = _websettinghelper.GetWebsettingJson("DefaultCurrencySettings");
                    var ismulticurrency = JsonConvert.DeserializeObject<DefaultCurrencySettingsModel>(_currencysetupSettings);

                    var _iscountrysetupSettings = _websettinghelper.GetWebsettingJson("GlobalAppSettings");
                    var isCountrySelection = JsonConvert.DeserializeObject<GlobalAppSettingsModel>(_iscountrysetupSettings);


                    model = new RegionalSettingViewModel
                    {
                        IsMultilingual = ismultilingual.IsMultilingual,
                        IsMultiCurrency = ismulticurrency.IsMultiCurrency,
                        IsCountrySelectionEnabled = isCountrySelection.IsCountrySelectionEnabled
                    };



                


            }

            return View(viewName, model); // Return a view to display the data
        }
    }
}
