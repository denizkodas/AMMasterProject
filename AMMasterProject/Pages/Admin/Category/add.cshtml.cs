using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AMMasterProject.Pages.Admin.Category
{
    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;

        private readonly ProductHelper _productHelper;

        public CategoryMaster categorymaster { get; set; }


        public IEnumerable<SelectListItem> ProductType { get; set; }
        //public IEnumerable<SelectListItem> SellingType { get; set; }

        //public IEnumerable<SelectListItem> ParentCategory { get; set; }


        public List<SellingTypeMetaData> sellingtype { get; set; }

        public int categoryid { get; set; }

        #endregion


        #region DI






        public addModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            categorymaster = new CategoryMaster();
            categorymaster.IsPublished = true;
            categorymaster.IsShowHomePage = false;
            categorymaster.IsIncludeMenu = false;
            _productHelper = productHelper; 
        }

        #endregion

        #region DataPopulate

        public void setup()
        {

            ///
            sellingtype = _productHelper.GetSellingTypeList();


            //ProductType = _dbContext.GeneralSetups
            //.Where(u => u.GeneralSetupType == "Product Type")
            // .Select(u => new SelectListItem
            // {
            //     Value = u.GeneralSetupId.ToString(),
            //     Text = u.GeneralSetupName
            // }).ToList();

            // SellingType = _dbContext.GeneralSetups
            //.Where(u => u.GeneralSetupType == "Selling Type")
            // .Select(u => new SelectListItem
            // {
            //     Value = u.GeneralSetupId.ToString(),
            //     Text = u.GeneralSetupName
            // }).ToList();



        //    ParentCategory = _dbContext.CategoryMasters
        //        .Where(u=>u.IsDeleted ==false)
        //.OrderBy(u=>u.CategoryName)
        // .Select(u => new SelectListItem
        // {
        //     Value = u.CategoryId.ToString(),
        //     Text = u.CategoryName
        // }).Distinct().ToList();


        }



        #endregion
        public void OnGet()
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }
            setup();

            if (Request.Query.ContainsKey("ID"))
            {
                categoryid = int.Parse(Request.Query["ID"].ToString());

                categorymaster = _dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == categoryid);
            }


        }

        public IActionResult OnPost() 
        {

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

            #region ModelValidation

            if (categorymaster.Sortnumber <= 0)
            {
                ModelState.AddModelError("categorymaster.SortOrder", "Sort order must be greater than 1");

                setup();
                return Page();
            }

          
            CategoryMaster categorymasterduplication = _dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryName == categorymaster.CategoryName && u.ParentCategoryId == categorymaster.ParentCategoryId && u.CategoryId != categorymaster.CategoryId);

            if (categorymasterduplication != null)
            {
                ModelState.AddModelError("categorymaster.CategoryName", "Category Name is already exist");

                setup();
                return Page();
            }




            #endregion

            #region CustomeAssignment

            categorymaster.ProfileId = loginid;
            categorymaster.InsertDate = DateTime.Now;

            #endregion
           
            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Insert

                if (categorymaster.CategoryId == 0)
                {
                    CategoryMaster insert = new CategoryMaster();

                    insert.CategoryName= categorymaster.CategoryName;
                    insert.Urlpath = GlobalHelper.SEOURL(categorymaster.CategoryName.ToLower());
                    insert.ParentCategoryId =categorymaster.ParentCategoryId;
                    insert.IsPublished = categorymaster.IsPublished;
                    insert.IsShowHomePage = categorymaster.IsShowHomePage;
                    insert.IsIncludeMenu = categorymaster.IsIncludeMenu;
                    insert.ListingTypeID  = categorymaster.ListingTypeID;
                    insert.SellingTypeID   = categorymaster.SellingTypeID; 
                    /*insert.CategoryTypeId = categorymaster.CategoryTypeId;*/ ////physical or digial
                    //insert.Sellingtypeid = categorymaster.Sellingtypeid;
                 

                    _dbContext.CategoryMasters.Add(categorymaster);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Category Name added successfully";
                }

                #endregion
                else
                {
                    CategoryMaster update = _dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == categorymaster.CategoryId);

                    if (update != null)
                    {
                        update.CategoryName = categorymaster.CategoryName;
                        update.Urlpath = GlobalHelper.SEOURL(categorymaster.CategoryName.ToLower());
                        update.ParentCategoryId = categorymaster.ParentCategoryId;
                        update.IsPublished = categorymaster.IsPublished;
                        update.IsShowHomePage = categorymaster.IsShowHomePage;
                        update.IsIncludeMenu = categorymaster.IsIncludeMenu;
                        update.ListingTypeID = categorymaster.ListingTypeID;
                        update.SellingTypeID = categorymaster.SellingTypeID;
                        update.InsertDate = DateTime.Now;
                        update.ProfileId = loginid;

                        update.ModifiedDate = DateTime.Now;
                      

                        _dbContext.CategoryMasters.Update(update);
                        _dbContext.SaveChanges();


                        TempData["info"] = "Category Name updated successfully";
                    }
                }

                #region Update

                #endregion

             

                return Redirect("/admin/category/add?ID=" + categorymaster.CategoryId + "#description");
            }

            else
            {
                setup();
                return Page();
            }


          
            #endregion


            
        }


        public IActionResult OnPostDescription()
        {
            if (categorymaster.CategoryId != 0)
            {

                CategoryMaster update = _dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == categorymaster.CategoryId);

                if (update != null)
                {
                    update.Description = categorymaster.Description;
                    update.ModifiedDate = DateTime.Now;


                    _dbContext.CategoryMasters.Update(update);
                    _dbContext.SaveChanges();

                    TempData["info"] = "Description updated successfully";
                }
            }


            return Redirect("/admin/category/add?ID=" + categorymaster.CategoryId + "#images");
        }


        public IActionResult OnPostImages()
        {
            if (categorymaster.CategoryId != 0)
            {

                CategoryMaster update = _dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == categorymaster.CategoryId);

                if (update != null)
                {
                    update.Icon = categorymaster.Icon;
                    update.Banner = categorymaster.Banner;
                    update.ModifiedDate = DateTime.Now;


                    _dbContext.CategoryMasters.Update(update);
                    _dbContext.SaveChanges();

                    TempData["info"] = "Images updated successfully";
                }
            }


            return Redirect("/admin/category/add?ID=" + categorymaster.CategoryId + "#seo");
        }

        public IActionResult OnPostSEO()
        {
            if (categorymaster.CategoryId != 0)
            {

                CategoryMaster update = _dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == categorymaster.CategoryId);

                if (update != null)
                {
                    update.SeoPageName = categorymaster.SeoPageName;
                    update.SeoTitle = categorymaster.SeoTitle;
                    update.SeoKeyword = categorymaster.SeoKeyword;
                    update.SeoDescription=categorymaster .SeoDescription;
                    update.ModifiedDate = DateTime.Now;


                    _dbContext.CategoryMasters.Update(update);
                    _dbContext.SaveChanges();


                    TempData["info"] = "SEO updated successfully";
                }
            }


            return Redirect("/admin/category/add?ID=" + categorymaster.CategoryId + "#seo");
        }
    }
}
