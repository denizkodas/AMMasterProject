using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AMMasterProject.Pages.Admin.Blog
{


    [Authorize(Policy = "Community")]
    [BindProperties]
    public class categoryModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;

        public BlogCategory blogcategory { get; set; }

        public List<BlogCategory> blogcategorylist { get; set; }

        #endregion

        #region DI

        public categoryModel(MyDbContext context)
        {
            _dbContext = context;


            blogcategory = new BlogCategory();
            blogcategory.IsPublish = true;



        }
        #endregion

        #region DataPopulate    

        public void setup()
        {

         

            if (Request.Query.ContainsKey("ID"))
            {
               int  categoryid = int.Parse(Request.Query["ID"].ToString());


                blogcategory = _dbContext.BlogCategories.FirstOrDefault(u => u.BlogCategoryId == categoryid);


            }

            blogcategorylist = _dbContext.BlogCategories.ToList();

        }
        #endregion

      
        public void OnGet()
        {
            setup();

        }

        public IActionResult OnPost()
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

          
            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Duplicat

                BlogCategory duplicate = _dbContext.BlogCategories.FirstOrDefault(u => u.BlogCategoryName.ToLower().Trim() == blogcategory.BlogCategoryName.ToLower().Trim() && u.BlogCategoryId != blogcategory.BlogCategoryId);
                if (duplicate != null)
                {


                    ModelState.AddModelError("blogcategory.BlogCategoryName", "Category Name already exist.");
                    setup();
                    return Page();
                }


                #endregion



                #region Insert

                if (blogcategory.BlogCategoryId == 0)
                {
                    BlogCategory insert = new BlogCategory();


                    insert.BlogCategoryName=blogcategory.BlogCategoryName.Trim();
                    insert.ProfileId = loginid;
                    insert.InsertDate= DateTime.Now;
                    insert.IsPublish = blogcategory.IsPublish;
              
                    _dbContext.BlogCategories.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Blog Category added successfully";
                }

                #endregion
                else
                {
                    BlogCategory update = _dbContext.BlogCategories.FirstOrDefault(u => u.BlogCategoryId == blogcategory.BlogCategoryId);

                    if (update != null)
                    {



                        update.BlogCategoryName = blogcategory.BlogCategoryName.Trim();
                        update.ProfileId = loginid;
                     
                        update.IsPublish = blogcategory.IsPublish;

                        _dbContext.BlogCategories.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Blog Category updated successfully";
                    }
                }

                #region Update

                #endregion
            }

            else
            {
                setup();
                return Page();
            }


      
                return RedirectToPage("/admin/blog/category");
           


            #endregion
        }

        public IActionResult OnPostDelete(int blogcategoryid)
        {
            BlogCategory del = _dbContext.BlogCategories.FirstOrDefault(u => u.BlogCategoryId == blogcategoryid);

            if (del != null)
            {

                _dbContext.BlogCategories.Remove(del);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/blog/category");


            }
            setup();
            return Page();
        }
    }
}
