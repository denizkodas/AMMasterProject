using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Blog
{
    [BindProperties]
    public class detailModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<IndexModel> _logger;
        
        public BlogViewModel blog { get; set; }
        public detailModel(MyDbContext dbContext, ILogger<IndexModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

       
        public void OnGet()
        {
            string blogurlpath = (string)RouteData.Values["blogurlpath"];



            blog = (from blog in _dbContext.Bloggings
                    join category in _dbContext.BlogCategories on blog.Categoryid equals category.BlogCategoryId
                    where blog.IsPublish == true && blog.SeoPageName==blogurlpath
                    select new BlogViewModel
                    {
                        BlogId = blog.BlogId,
                        Title = blog.Title,
                        Image = blog.Image,
                        Category = category.BlogCategoryName,
                        InsertDate = blog.InsertDate,
                        IsPublish = blog.IsPublish,
                        Summary=blog.Summary,
                        Description =blog.Description,
                        SeoPageKeyword=blog.SeoKeyword,
                        SeoPageDescription=blog.SeoDescription,
                        SeoPageTitle=blog.Title
                    }

                          ).FirstOrDefault();

        }
    }
}
