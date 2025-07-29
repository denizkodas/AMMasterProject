using AMMasterProject.Helpers;
using AMMasterProject.Pages.Payment;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AMMasterProject.Controllers
{

    [Route("controller/[controller]/{action}")]
    [Controller]
    public class HomeController : Controller
    {
        // GET: HomeController
        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _cache;
        private readonly WebsettingHelper _websettinghelper;
        private readonly GlobalHelper _globalHelper;
        private readonly ProductHelper _producthelper;
        private readonly UserHelper _userhelper;

        private readonly NotificationHelper _notificationHelper;
        private readonly InboxHelper _inboxHelper;
        public HomeController(MyDbContext context, IMemoryCache cache, WebsettingHelper websettinghelper, GlobalHelper globalHelper , ProductHelper producthelper, UserHelper userhelper, NotificationHelper notificationHelper, InboxHelper inboxHelper)
        {
            _dbcontext = context;
            _cache = cache;
            _websettinghelper = websettinghelper;
            _globalHelper = globalHelper;
            _producthelper = producthelper;
            _userhelper = userhelper;
            _notificationHelper = notificationHelper;
            _inboxHelper = inboxHelper;
        }




     
        public IActionResult MyAction(string myString)
        {
            ViewBag.MyString = myString;
            return PartialView("/Pages/Shared/setups/_search-address.cshtml");
        }


      
        public async Task<IActionResult> headerlinkview()
        {

           var model = await _dbcontext.PageNames
            .Where(u => u.IsPublish == true && (u.Type == "Header" || u.Type == "Both"))
            .OrderBy(u => u.Sortnumber)

            .Select(u => new PageName { IsUrl = u.IsUrl, Url = u.Url, Name = u.Name, SeoPageName = u.SeoPageName, PageCategoryId = u.PageCategoryId })

            .ToListAsync();

            return PartialView("/Pages/shared/setups/_headerlinkview.cshtml", model);
        }


       
        public async Task<IActionResult> footerlinkview()
        {
            // TODO: Retrieve the product data based on the SEO URL
            //var product = await _productService.GetProductBySeoUrl(seoUrl);

            // Render the partial view with the product data
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


         

            return PartialView("/Pages/shared/setups/_footerlinkview.cshtml", viewModel);
        }


      
        public async Task<IActionResult> herobanner()
        {
            object model = null;
            
            var _herobannerSettings = _websettinghelper.GetWebsettingJson("HeroBannerSettings");

            if (_herobannerSettings != null && !string.IsNullOrEmpty(_herobannerSettings))
            {
                List<HeroBannerSettingsViewModel> listparse = JsonConvert.DeserializeObject<List<HeroBannerSettingsViewModel>>(_herobannerSettings);



                model = listparse.Where(u => u.IsPublish == true).ToList();



            }

            return PartialView("/Pages/shared/setups/_herobanner.cshtml", model);
        }

        public async Task<IActionResult> cmsview(string cmskey)
        {
            object model;
            try
            {


                model = await
            (from p in _dbcontext.PageNames


             where p.CMSKey == cmskey.ToString()
             select new PageDetailViewModel
             {
                 PageDescription = p.PageDescription ?? "",

             }).FirstAsync();





            }
            catch (Exception ex)
            {

                model = new PageDetailViewModel
                {
                    PageDescription = "CMS not exist name = " +  cmskey
                };
            }

            return PartialView("/Pages/shared/setups/_cmsview.cshtml", model);
        }

       

     
        public async Task<IActionResult> productboosthome()
        {
           

          var model = _producthelper.productmasterdataV2(0, "boost", 30, 1, 0);

            return PartialView("/Pages/shared/setups/_productboost.cshtml", model);
        }


     
        public async Task<IActionResult> productdiscounthome()
        {
            

            var model = _producthelper.productmasterdataV2(0, "discount", 20, 1, 0);

            return PartialView("/Pages/shared/setups/_productdiscount.cshtml", model);
        }



       
        public async Task<IActionResult> partnerhome()
        {

            List<WebsitesetupPartner> model = await (from wss in _dbcontext.WebsitesetupPartners
                           where wss.IsPublish == true && wss.Isaddonhomepage == true
                           select wss).ToListAsync();

            return PartialView("/Pages/shared/setups/_partnerhome.cshtml", model);

           
        }

     
        public async Task<IActionResult> socialmediafooter()
        {

          var  model = await _dbcontext.WebsiteSetupSocialMedia.Where(u => u.IsPublish == true).ToListAsync();



            return PartialView("/Pages/shared/setups/_socialmediafooter.cshtml", model);


        }


      
        public async Task<IActionResult> featuredsellerhome()
        {

            var productBoosts = _dbcontext.ProductBoosts.AsEnumerable().ToList();

            // Materialize the SellerList results with ToList()
            var sellerList = _userhelper.SellerList().ToList();

            var q = from pb in productBoosts
                    join order in _dbcontext.OrderMasters on pb.InvoiceNumber equals order.InvoiceNumber
                    join up in sellerList on pb.ItemBoostGUID equals up.ProfileGuid
                    orderby Guid.NewGuid()
                    where pb.StartDate <= DateTime.Now && pb.EndDate >= DateTime.Now &&
                          pb.BoostType == "profile" && order.PaymentStatus == "paid"
                    select new SellerViewModel
                    {
                        BusinessUrlpath = up.BusinessUrlpath,
                        Image = up.Image,
                        BusinessName = up.BusinessName,
                        ProfileId = up.ProfileId,
                        ProductTotal = up.ProductTotal //_dbcontext.ItemListings.Count(u => u.ProfileId == up.ProfileId && u.IsAdminLocked == false && u.IsPublish)
                    };

            var model = q.Distinct().ToList();


            return PartialView("/Pages/shared/setups/_featuredseller.cshtml", model);


        }

      
        public async Task<IActionResult> regionalsettingfooter()
        {

            var _languagesetupSettings = _websettinghelper.GetWebsettingJson("LanguageSetupSettings");
            var ismultilingual = JsonConvert.DeserializeObject<LanguageSetupSettingsModel>(_languagesetupSettings);

            var _currencysetupSettings = _websettinghelper.GetWebsettingJson("DefaultCurrencySettings");
            var ismulticurrency = JsonConvert.DeserializeObject<DefaultCurrencySettingsModel>(_currencysetupSettings);

            var _iscountrysetupSettings = _websettinghelper.GetWebsettingJson("GlobalAppSettings");
            var isCountrySelection = JsonConvert.DeserializeObject<GlobalAppSettingsModel>(_iscountrysetupSettings);


           var model = new RegionalSettingViewModel
            {
                IsMultilingual = ismultilingual.IsMultilingual,
                IsMultiCurrency = ismulticurrency.IsMultiCurrency,
                IsCountrySelectionEnabled = isCountrySelection.IsCountrySelectionEnabled
            };




            return PartialView("/Pages/shared/setups/_regionalsettingfooter.cshtml", model);


        }


       
        public async Task<IActionResult> bloghome()
        {

            List<BlogViewModel> model;


            IQueryable<BlogViewModel> query = _dbcontext.Bloggings
                .Join(_dbcontext.BlogCategories, blog => blog.Categoryid, category => category.BlogCategoryId,
                    (blog, category) => new BlogViewModel
                    {
                        BlogId = blog.BlogId,
                        Title = blog.Title,
                        Image = blog.Image,
                        Category = category.BlogCategoryName,
                        SEOCategory = GlobalHelper.SEOURL(category.BlogCategoryName),
                        InsertDate = blog.InsertDate,
                        IsPublish = blog.IsPublish,
                        SeoPageName = blog.SeoPageName,
                        Summary = blog.Summary,
                        Isaddonhomepage = blog.Isaddonhomepage,
                        IsFeatured = blog.isfeatured
                    })
                .Where(blog => blog.IsPublish==true &&  blog.Isaddonhomepage==true);

            model = await query.ToListAsync();

            return PartialView("/Pages/shared/setups/_bloghome.cshtml", model);
        }

      

      
        public async Task<IActionResult> homecategories()
        {
            object model = null;
           

          
                model = await _dbcontext.CategoryMasters
                    .Where(firstlevel => firstlevel.IsPublished && !firstlevel.IsDeleted && firstlevel.ParentCategoryId == 0)
                    .OrderBy(firstlevel => firstlevel.Sortnumber)
                    .Select(firstlevel => new CategoryViewModel
                    {
                        ParentCategoryId = firstlevel.ParentCategoryId,
                        CategoryId = firstlevel.CategoryId,
                        CategoryName = firstlevel.CategoryName,
                        Icon = firstlevel.Icon,
                        SecondLevel = _dbcontext.CategoryMasters
                            .Where(secondlevel => secondlevel.ParentCategoryId == firstlevel.CategoryId && secondlevel.IsPublished && !secondlevel.IsDeleted)
                            .OrderBy(secondlevel => secondlevel.Sortnumber)
                            .Select(secondlevel => new SecondCategoryViewModel
                            {
                                CategoryId = secondlevel.CategoryId,
                                CategoryName = secondlevel.CategoryName,
                                Icon = secondlevel.Icon,
                                ThirdLevel = _dbcontext.CategoryMasters
                                    .Where(thirdlevel => thirdlevel.ParentCategoryId == secondlevel.CategoryId && thirdlevel.IsPublished && !thirdlevel.IsDeleted)
                                    .OrderBy(thirdlevel => thirdlevel.Sortnumber)
                                    .Select(thirdlevel => new ThirdCategoryViewModel
                                    {
                                        CategoryId = thirdlevel.CategoryId,
                                        CategoryName = thirdlevel.CategoryName,
                                        Icon = thirdlevel.Icon,
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToListAsync();

            
            

            return PartialView("/Pages/shared/setups/_categories.cshtml", model);
        }

        public async Task<IActionResult> homecategorieswithbanner()
        {
            object model = null;



            model = await _dbcontext.CategoryMasters
                .Where(firstlevel => firstlevel.IsPublished && !firstlevel.IsDeleted && firstlevel.ParentCategoryId == 0)
                .OrderBy(firstlevel => firstlevel.Sortnumber)
                .Select(firstlevel => new CategoryViewModel
                {
                    ParentCategoryId = firstlevel.ParentCategoryId,
                    CategoryId = firstlevel.CategoryId,
                    CategoryName = firstlevel.CategoryName,
                    Icon = firstlevel.Icon,
                    Banner=firstlevel.Banner,
                    SecondLevel = _dbcontext.CategoryMasters
                        .Where(secondlevel => secondlevel.ParentCategoryId == firstlevel.CategoryId && secondlevel.IsPublished && !secondlevel.IsDeleted)
                        .OrderBy(secondlevel => secondlevel.Sortnumber)
                        .Select(secondlevel => new SecondCategoryViewModel
                        {
                            CategoryId = secondlevel.CategoryId,
                            CategoryName = secondlevel.CategoryName,
                            Icon = secondlevel.Icon,
                            ThirdLevel = _dbcontext.CategoryMasters
                                .Where(thirdlevel => thirdlevel.ParentCategoryId == secondlevel.CategoryId && thirdlevel.IsPublished && !thirdlevel.IsDeleted)
                                .OrderBy(thirdlevel => thirdlevel.Sortnumber)
                                .Select(thirdlevel => new ThirdCategoryViewModel
                                {
                                    CategoryId = thirdlevel.CategoryId,
                                    CategoryName = thirdlevel.CategoryName,
                                    Icon = thirdlevel.Icon,
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();




            return PartialView("/Pages/shared/setups/_categorieswithbanner.cshtml", model);
        }

        public async Task<IActionResult> firstlevelcategories()
        {
            object model = null;



            model = await _dbcontext.CategoryMasters
                .Where(firstlevel => firstlevel.IsPublished && !firstlevel.IsDeleted && firstlevel.ParentCategoryId == 0)
                .OrderBy(firstlevel => firstlevel.Sortnumber)
                .Select(firstlevel => new CategoryViewModel
                {
                    ParentCategoryId = firstlevel.ParentCategoryId,
                    CategoryId = firstlevel.CategoryId,
                    CategoryName = firstlevel.CategoryName,
                    Icon = firstlevel.Icon,
                    Banner = firstlevel.Banner,

                })
                .ToListAsync();




            return PartialView("/Pages/shared/setups/_categoriesmain.cshtml", model);
        }

        public async Task<IActionResult> SecondLevelCategories()
        {
            var categories = await _dbcontext.CategoryMasters
                .Where(firstlevel => firstlevel.IsPublished && !firstlevel.IsDeleted && firstlevel.ParentCategoryId != 0)
                .OrderBy(firstlevel => firstlevel.CategoryName)
                .Select(firstlevel => new CategoryViewModel
                {
                    ParentCategoryId = firstlevel.ParentCategoryId,
                    CategoryId = firstlevel.CategoryId,
                    CategoryName = firstlevel.CategoryName,
                    Icon = firstlevel.Icon,
                    Banner = firstlevel.Banner,
                })
                .ToListAsync();

            return Json(categories);
        }

        public async Task<IActionResult> IncludeInMenucategories()
        {
            object model = null;



            model = await _dbcontext.CategoryMasters
                .Where(firstlevel => firstlevel.IsPublished && !firstlevel.IsDeleted  && firstlevel.IsIncludeMenu==true)
                .OrderBy(firstlevel => firstlevel.Sortnumber)
                .Select(firstlevel => new CategoryViewModel
                {
                    ParentCategoryId = firstlevel.ParentCategoryId,
                    CategoryId = firstlevel.CategoryId,
                    CategoryName = firstlevel.CategoryName,
                    Icon = firstlevel.Icon,
                    Banner = firstlevel.Banner,

                })
                .ToListAsync();




            return PartialView("/Pages/shared/setups/_categoriesslider.cshtml", model);
        }

        public async Task<IActionResult> inboxcounterheader()
        {
            if (User.Identity.IsAuthenticated)
            {
               int loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");// Assuming profile ID is stored in NameIdentifier claim
                var model = await _inboxHelper.InboxUnreadCount(loginid);

                return PartialView("/Pages/shared/setups/_inboxcounter.cshtml", model);
            }

            // Handle the case when the user is not authenticated
            // You can return a different response or redirect to a login page
            return Json(new { success = true, message="Not login" });
        }

       
        public async Task<IActionResult> Cookie(string seoUrl)
        {
            // TODO: Retrieve the product data based on the SEO URL
            //var product = await _productService.GetProductBySeoUrl(seoUrl);

            // Render the partial view with the product data
            return PartialView("/Pages/products/_product-view.cshtml", seoUrl);
        }
     
        public async Task<IActionResult> userdefinedscripts(string scriptkey)
        {
            if (scriptkey != null)
            {
                var model = _globalHelper.GetScriptCustom().Where(u=>u.ScriptName == scriptkey);

                return PartialView("/Pages/shared/setups/_scriptsuserdefined.cshtml", model.ToList());
            }
            else
            {
                var model = _globalHelper.GetScriptCustom();

                return PartialView("/Pages/shared/setups/_scriptsuserdefined.cshtml", model);

            }
        }



    }
}
