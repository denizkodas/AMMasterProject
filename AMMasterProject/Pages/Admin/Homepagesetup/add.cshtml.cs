using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AMMasterProject.Pages.Admin.Homepagesetup
{


    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class addModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;

        public ItemHomePageDesignViewModel websitesetupproductsetting { get; set; }

        public IEnumerable<SelectListItem> WebsiteItemSetupId { get; set; }  ///manual, top rated
        public IEnumerable<SelectListItem> SellingTypeId { get; set; }  //sale auction

        public int websitesetuppageid { get; set; }
        #endregion

        #region DI

        public addModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;


            websitesetupproductsetting = new ItemHomePageDesignViewModel();
            websitesetupproductsetting.IsPublish = true;

            websitesetupproductsetting.ItemDesignMetaData = new ItemHomePageDesignMetaData();
            //websitesetupproductsetting.IsEditable = true;
            //websitesetupproductsetting.ItemDesignMetaData.ShowBanner = false;
            //websitesetupproductsetting.ItemDesignMetaData.ShowTitle = false;
            //websitesetupproductsetting.ItemDesignMetaData.ShowItemSlider = false;

        }

        #endregion

        #region DataPopulate    

        public void setup()
        {





            //WebsiteItemSetupId = _dbContext.GeneralSetups
            //.Where(u => u.GeneralSetupType == "Website Item Setup")
            // .Select(u => new SelectListItem
            // {
            //     Value = u.GeneralSetupId.ToString(),
            //     Text = u.GeneralSetupName
            // }).ToList();


          

            SellingTypeId = _productHelper.GetSellingTypeList()
           
             .Select(u => new SelectListItem
             {
                 Value = u.ID.ToString(),
                 Text = u.Name
             }).ToList();
        }
        #endregion
        public void OnGet()
        {
            setup();
            if (Request.Query.ContainsKey("ID"))
            {
                websitesetuppageid = int.Parse(Request.Query["ID"].ToString());


                ItemPageDesign items = _dbContext.ItemPageDesign.FirstOrDefault(u => u.ItemPageDesignID == websitesetuppageid);
                if (items == null)
                {
                    TempData["success"] = "Listing does not exist. You can create new listing.";
                    RedirectToPage("/admin/Homepagesetup");
                }

                else
                {

                    var json = ProductHelper.ParseMetaDataItemHomePageDesign(items.PageDesignMetaData);


                    websitesetupproductsetting.ItemDesignId = items.ItemPageDesignID;
                    websitesetupproductsetting.IsPublish = items.Ispublish;
                    websitesetupproductsetting.ItemDesignMetaData.SellingType = json.SellingType;
                    websitesetupproductsetting.Title = items.Title;
                    websitesetupproductsetting.SortOrder = items.SortOrder;
                    websitesetupproductsetting.ItemDesignMetaData.Style = json.Style;
                    websitesetupproductsetting.ItemDesignMetaData.PreselectedCategory = json.PreselectedCategory;
                    websitesetupproductsetting.ItemDesignMetaData.NoofItemsDisplay = json.NoofItemsDisplay;
                    websitesetupproductsetting.ItemDesignMetaData.Background = json.Background;
                    websitesetupproductsetting.ItemDesignMetaData.Banner = json.Banner;
                    websitesetupproductsetting.ItemDesignMetaData.ShowTitle = json.ShowTitle;
                    websitesetupproductsetting.ItemDesignMetaData.ShowBanner = json.ShowBanner;
                    websitesetupproductsetting.ItemDesignMetaData.ShowItemSlider = json.ShowItemSlider;
                    websitesetupproductsetting.ItemDesignMetaData.IsURL=json.IsURL;
                    websitesetupproductsetting.ItemDesignMetaData.URL = json.URL;


                }
            }
        }

        public IActionResult OnPost()
        {
            try
            {

          
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

            #region ModelValidation

            if (websitesetupproductsetting.SortOrder <= 0)
            {
                ModelState.AddModelError("websitesetupproductsetting.SortOrder", "Sort order must be greater than 1");

                setup();
                return Page();
            }

            if (websitesetupproductsetting.ItemDesignMetaData.NoofItemsDisplay < 4 && websitesetupproductsetting.ItemDesignMetaData.PreselectedCategory != "onlybanner")
            {
                ModelState.AddModelError("websitesetupproductsetting.NoofItemsDisplay", "No of Items Display must be equal or greater than 4");

                setup();
                return Page();
            }

                if (websitesetupproductsetting.ItemDesignMetaData.IsURL ==true && websitesetupproductsetting.ItemDesignMetaData.URL==null)
                {
                    ModelState.AddModelError("websitesetupproductsetting.URL", "URL is required");

                    setup();
                    return Page();
                }

                #endregion
                #region Up-sert


                string json = _productHelper.ItemHomePageDesignmetadata(websitesetupproductsetting.ItemDesignMetaData.SellingType,

                websitesetupproductsetting.ItemDesignMetaData.Style,
                websitesetupproductsetting.ItemDesignMetaData.PreselectedCategory,
                websitesetupproductsetting.ItemDesignMetaData.NoofItemsDisplay,
                websitesetupproductsetting.ItemDesignMetaData.Background,
                websitesetupproductsetting.ItemDesignMetaData.Banner,
                websitesetupproductsetting.ItemDesignMetaData.ShowTitle,
                websitesetupproductsetting.ItemDesignMetaData.ShowBanner,
                websitesetupproductsetting.ItemDesignMetaData.ShowItemSlider,
                 websitesetupproductsetting.ItemDesignMetaData.IsURL,
                websitesetupproductsetting.ItemDesignMetaData.URL
                );

            #region Insert

            if (websitesetupproductsetting.ItemDesignId == 0)
            {
                ItemPageDesign wsps = new ItemPageDesign();

                wsps.Title = websitesetupproductsetting.Title;
                wsps.SortOrder = websitesetupproductsetting.SortOrder;
                wsps.PageDesignMetaData = json;
                wsps.Ispublish = websitesetupproductsetting.IsPublish;

                wsps.InsertDate = DateTime.Now;
                wsps.ProfileId = loginid;
                _dbContext.ItemPageDesign.Add(wsps);
                _dbContext.SaveChanges();

                TempData["success"] = "Setting added successfully";


                if (websitesetupproductsetting.ItemDesignMetaData.PreselectedCategory == "product")
                {


                        return RedirectToPage("/admin/Homepagesetup/customhomepage", new { ID = wsps.ItemPageDesignID });
                 }
                else
                {
                    return RedirectToPage("/admin/Homepagesetup/index");
                }
            }

            #endregion
            else
            {
                ItemPageDesign update = _dbContext.ItemPageDesign.FirstOrDefault(u => u.ItemPageDesignID == websitesetupproductsetting.ItemDesignId);

                if (update != null)
                {
                    update.PageDesignMetaData = json;
                    update.Title = websitesetupproductsetting.Title;
                    update.SortOrder = websitesetupproductsetting.SortOrder;
                    update.Ispublish = websitesetupproductsetting.IsPublish;

                    update.InsertDate = DateTime.Now;
                    update.ProfileId = loginid;

                    _dbContext.ItemPageDesign.Update(update);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Setting updated successfully";

                    if (websitesetupproductsetting.ItemDesignMetaData.PreselectedCategory == "product")
                    {

                            return RedirectToPage("/admin/Homepagesetup/customhomepage", new { ID = update.ItemPageDesignID });
                            
                    }
                    else
                    {
                        return RedirectToPage("/admin/Homepagesetup/index");
                    }
                }
            }

            #region Update

            #endregion




           

                #endregion

                setup();
                return Page();
            }
            catch (Exception ex)
            {

                TempData["success"] = ex.Message;
                setup();
                return Page();
            }
        }
    }
}
