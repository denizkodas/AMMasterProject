using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AMMasterProject.Pages.Admin.pagessetup
{


    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class categoryModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;

        public PageCategory pagecategory { get; set; }

        public List<PageCategory> Listpagecategory { get; set; }

        #endregion

        #region DI

        public categoryModel(MyDbContext context)
        {
            _dbContext = context;


            pagecategory = new PageCategory();
            pagecategory.IsPublish = true;



        }

        #endregion

        #region DataPopulate    

        public void setup()
        {





            Listpagecategory = _dbContext.PageCategories.ToList();
            
        }
        #endregion
        public void OnGet()
        {

            setup();

            if (Request.Query.ContainsKey("ID") )
            {
                int pagecategoryid = int.Parse(Request.Query["ID"].ToString());
                

                

                pagecategory = _dbContext.PageCategories.FirstOrDefault(u => u.PageCategoryId ==pagecategoryid);


              
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
            PageCategory duplicate = _dbContext.PageCategories.FirstOrDefault(u => u.Category.Trim().ToLower() == pagecategory.Category.Trim().ToLower() && u.PageCategoryId != pagecategory.PageCategoryId);

            if (duplicate != null)
            {
                ModelState.AddModelError("pagecategory.Category", "Category is already exist");

                setup();
                return Page();
            }


            #endregion
            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Insert

                if (pagecategory.PageCategoryId == 0)
                {
                    PageCategory pc = new PageCategory();

                    pc.Category = pagecategory.Category.Trim();
                    pc.IsPublish = pagecategory.IsPublish;
                    pc.ProfileId = loginid;
                    _dbContext.PageCategories.Add(pc);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Page Category added successfully";
                }

                #endregion
                else
                {
                    PageCategory update = _dbContext.PageCategories.FirstOrDefault(u => u.PageCategoryId == pagecategory.PageCategoryId);

                    if (update != null)
                    {
                        update.Category = pagecategory.Category.Trim();
                        update.IsPublish = pagecategory.IsPublish;
                        update.ProfileId = loginid;


                        _dbContext.PageCategories.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Page Category updated successfully";
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


            return RedirectToPage("/admin/pagessetup/category");
            #endregion
        }

        public IActionResult OnPostDelete(int pagecategoryid)
        {
            PageCategory pagecategory = _dbContext.PageCategories.FirstOrDefault(u => u.PageCategoryId == pagecategoryid);

            if (pagecategory != null)
            {

                _dbContext.PageCategories.Remove(pagecategory);
                _dbContext.SaveChanges();



                TempData["warning"] = "Deleted successfully";

               


                return RedirectToPage("/admin/pagessetup/category");
            }

            setup();

            return Page();
        }
    }
}
