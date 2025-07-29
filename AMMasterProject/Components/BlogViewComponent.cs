using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static AMMasterProject.ViewModel.BlogViewModel;

namespace AMMasterProject.Components
{
    public class BlogViewComponent : ViewComponent
    {

        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _cache;
        public BlogViewComponent(MyDbContext context, IMemoryCache cache)
        {
            _dbcontext = context;
            _cache = cache;
        }


        //change db

        [ResponseCache(Duration = 86400)]
        public async Task<IViewComponentResult> InvokeAsync(string viewName, string methodName)
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
                    .Where(blog => blog.IsPublish);

                model = await query.ToListAsync(); // Changed from AsQueryable to directly ToListAsync
             
           

            //IQueryable<BlogViewModel> filteredQuery = model.AsQueryable();

            if (methodName == "homepage")
            {
                model = model.Where(blog => blog.Isaddonhomepage).ToList();
            }
            else if (methodName == "featured")
            {
                model = model.Where(blog => blog.IsFeatured).ToList();
            }
            else if (methodName == "all")
            {
                if (RouteData.Values["category"] != null)
                {
                    var category = RouteData.Values["category"].ToString();
                    model = model.Where(blog => blog.SEOCategory == category).ToList();
                }

                if (Request.Query.ContainsKey("q"))
                {
                    var keyword = Request.Query["q"].ToString().ToLower();
                    model = model.Where(blog => blog.Title.ToLower().Contains(keyword) || blog.Summary.Contains(keyword)).ToList();
                }
            }

          

            return View(viewName, model);
        }



    }
}
