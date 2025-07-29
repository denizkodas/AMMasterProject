using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Category
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class IndexModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;

        public List<CategoryMaster> categorymaster { get; set; }


        #endregion

        #region DI

        public IndexModel(MyDbContext context)
        {
            _dbContext = context;


        }

        #endregion

        #region DataPopulate    

        public void setup()
        {



            categorymaster = (from cm in _dbContext.CategoryMasters
                            
                              select new CategoryMaster
                              {
                                  CategoryId=cm.CategoryId,
                                  CategoryName =cm.CategoryName,
                                  ParentCategoryId =cm.ParentCategoryId,
                                  IsDeleted =cm.IsDeleted,
                                  ModifiedDate =cm.ModifiedDate,
                              }
                              
                              ).ToList();


           

        }
        #endregion
        public void OnGet()
        {
            setup();
        }

        public IActionResult OnPostSoftDelete(int categorymasterid) 
        {
            CategoryMaster cm = _dbContext.CategoryMasters.FirstOrDefault(u=>u.CategoryId==categorymasterid);

            if(cm != null) 
            {
                cm.IsDeleted = true;
                cm.ModifiedDate = DateTime.Now;
                _dbContext.CategoryMasters.Update(cm);
                _dbContext.SaveChanges();

                TempData["warning"] = "Category Name deleted successfully";

                setup();
                return RedirectToPage("/admin/category/index");

            }
            setup();
            return Page();
        }
        public IActionResult OnPostUnSoftDelete(int categorymasterid)
        {
            CategoryMaster cm = _dbContext.CategoryMasters.FirstOrDefault(u => u.CategoryId == categorymasterid);

            if (cm != null)
            {
                cm.IsDeleted = false;
                cm.ModifiedDate = DateTime.Now;
                _dbContext.CategoryMasters.Update(cm);
                _dbContext.SaveChanges();

                TempData["warning"] = "Category Name Un-DO successfully";

                setup();
                return RedirectToPage("/admin/category/index");

            }
            setup();
            return Page();
        }
    }
}
