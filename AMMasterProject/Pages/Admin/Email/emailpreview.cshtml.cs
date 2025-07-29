using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Email
{
    public class emailpreviewModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        [BindProperty]
        public PageDetailViewModel listpagecategory { get; set; }

        public emailpreviewModel(MyDbContext dbContext)
        {
            _dbContext = dbContext;
           

        }
        public void OnGet()
        {
            int pagepath = int.Parse(RouteData.Values["pagepath"].ToString());

#pragma warning disable CS8601 // Possible null reference assignment.


            listpagecategory = (from p in _dbContext.PageNames
                                


                                where p.PageType == "email" && p.PageNameId== pagepath
                                select new PageDetailViewModel
                                {
                                    PageDescription = p.PageDescription ?? "",
                                    PageName = p.Name,
                                    SEOTitle = p.SeoTitle,
                                    SEOKeyword = p.SeoKeyword,
                                    SEOKeyDescription = p.SeoDescription
                                }).FirstOrDefault();
            if (listpagecategory == null)
            {
                RedirectToPage("/Error", new { Title = "404", Message = "Page you are looking for does not exist or temporarily removed" });
            }

#pragma warning restore CS8601 // Possible null reference assignment.
        }
    }
}
