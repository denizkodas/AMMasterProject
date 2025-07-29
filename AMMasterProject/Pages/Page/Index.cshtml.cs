using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Page
{
    
    public class IndexModel : PageModel
    {

        private readonly MyDbContext _dbContext;
        private readonly ILogger<IndexModel> _logger;
        [BindProperty]
        public PageDetailViewModel listpagecategory { get; set; }

        public IndexModel(MyDbContext dbContext, ILogger<IndexModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

        }

      
       
       

        public void OnGet()
        {
           string pagepath = (string)RouteData.Values["pagepath"];

#pragma warning disable CS8601 // Possible null reference assignment.


            listpagecategory = (from p in _dbContext.PageNames
                              

                               where p.PageType =="page" &&  (string.IsNullOrEmpty(p.SeoPageName) ? p.PageCategoryId.ToString() : p.SeoPageName) == pagepath
                                select new PageDetailViewModel
                                {
                                    PageDescription = p.PageDescription ?? "",
                                    PageName=p.Name ,
                                    SEOTitle =p.SeoTitle,
                                    SEOKeyword=p.SeoKeyword,
                                    SEOKeyDescription=p.SeoDescription
                                }).FirstOrDefault();
            if(listpagecategory==null)
            {
                RedirectToPage("/Error", new { Title = "404", Message = "Page you are looking for does not exist or temporarily removed" });
            }

#pragma warning restore CS8601 // Possible null reference assignment.
        }
    }
}
