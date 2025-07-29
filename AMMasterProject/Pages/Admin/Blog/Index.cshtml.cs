using AMMasterProject.Components;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static AMMasterProject.ViewModel.BlogViewModel;

namespace AMMasterProject.Pages.Admin.Blog
{

    [Authorize(Policy = "Community")]
    [BindProperties]
    public class IndexModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;

     
        public List<BlogViewModel> bloglist { get; set; }

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




            bloglist = (from blogging in _dbContext.Bloggings
                        join category in _dbContext.BlogCategories on blogging.Categoryid equals category.BlogCategoryId
                        select new BlogViewModel
                        {
                            Title = blogging.Title,
                            Category=category.BlogCategoryName,
                            InsertDate = DateTime.Now,
                            IsPublish = blogging.IsPublish,
                            Image= blogging.Image,
                            BlogId = blogging.BlogId,

                        }
                        
                        ).ToList();

        }
        #endregion

        public void OnGet()
        {

            setup();
        }

        public IActionResult OnPostDelete(int blogid)
        {
            Blogging del = _dbContext.Bloggings.FirstOrDefault(u => u.BlogId == blogid);

            if (del != null)
            {

                _dbContext.Bloggings.Remove(del);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/blog/Index");


            }
            setup();
            return Page();
        }
    }
}
