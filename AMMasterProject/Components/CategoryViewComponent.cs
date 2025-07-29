using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AMMasterProject.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly MyDbContext _dbcontext;
        private readonly IMemoryCache _cache;
        public CategoryViewComponent(MyDbContext context, IMemoryCache cache)
        {
            _dbcontext = context;
            _cache = cache;
        }





        //[ResponseCache(Duration = 86400)]
        public async Task<IViewComponentResult> InvokeAsync(string viewName, string methodName)
        {
            object model = null;
            string cacheKey = string.Empty;



                    //model = await _dbcontext.CategoryMasters
                    //    .Where(firstlevel => firstlevel.IsPublished && !firstlevel.IsDeleted && firstlevel.ParentCategoryId == 0)
                    //    .OrderBy(firstlevel => firstlevel.Sortnumber)
                    //    .Select(firstlevel => new CategoryViewModel
                    //    {
                    //        ParentCategoryId = firstlevel.ParentCategoryId,
                    //        CategoryId = firstlevel.CategoryId,
                    //        CategoryName = firstlevel.CategoryName,
                    //        Icon = firstlevel.Icon,
                    //        SecondLevel = _dbcontext.CategoryMasters
                    //            .Where(secondlevel => secondlevel.ParentCategoryId == firstlevel.CategoryId && secondlevel.IsPublished && !secondlevel.IsDeleted)
                    //            .OrderBy(secondlevel => secondlevel.Sortnumber)
                    //            .Select(secondlevel => new SecondCategoryViewModel
                    //            {
                    //                CategoryId = secondlevel.CategoryId,
                    //                CategoryName = secondlevel.CategoryName,
                    //                Icon = secondlevel.Icon,
                    //                ThirdLevel = _dbcontext.CategoryMasters
                    //                    .Where(thirdlevel => thirdlevel.ParentCategoryId == secondlevel.CategoryId && thirdlevel.IsPublished && !thirdlevel.IsDeleted)
                    //                    .OrderBy(thirdlevel => thirdlevel.Sortnumber)
                    //                    .Select(thirdlevel => new ThirdCategoryViewModel
                    //                    {
                    //                        CategoryId = thirdlevel.CategoryId,
                    //                        CategoryName = thirdlevel.CategoryName,
                    //                        Icon = thirdlevel.Icon,
                    //                    })
                    //                    .ToList()
                    //            })
                    //            .ToList()
                    //    })
                    //    .ToListAsync();

                /*_cache.Set(cacheKey, model, TimeSpan.FromDays(7)); */// Cache for 7 days


            //else if (methodName == "Blog")
            //{
            //    cacheKey = "CacheHomeBlog";

            //    if (!_cache.TryGetValue(cacheKey, out model))
            //    {
                    model = await (from category in _dbcontext.BlogCategories
                                   where _dbcontext.Bloggings.Any(blog => blog.Categoryid == category.BlogCategoryId)
                                         && category.IsPublish
                                   orderby category.BlogCategoryName
                                   select new CommunityCategory
                                   {
                                       CategoryId = category.BlogCategoryId,
                                       CategoryName = category.BlogCategoryName,
                                       SeoURL = "blog/" + GlobalHelper.SEOURL(category.BlogCategoryName)
                                   }
                                  ).ToListAsync();

            //        _cache.Set(cacheKey, model, TimeSpan.FromDays(7)); // Cache for 7 days
            //    }
            //}

            return View(viewName, model); // Return a view to display the data
        }
    }
}
