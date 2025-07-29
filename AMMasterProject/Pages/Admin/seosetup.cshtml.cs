using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin
{
    [Authorize]
    [BindProperties]
    public class seosetupModel : PageModel
    {

        #region Models
        private readonly MyDbContext _dbContext;

        public CompanySEOModel CompanySetup { get; set; }

        #endregion

        #region DI

        public seosetupModel(MyDbContext context)
        {
            _dbContext = context;

        }

        #endregion


        #region DataPopulate    

        public void setup()
        {
            CompanySetup companysetup = _dbContext.CompanySetups.FirstOrDefault();

            if (companysetup != null)
            {
                CompanySetup = new CompanySEOModel
                {
                    MetaTitle = companysetup.MetaTitle,
                    MetaKeyword = companysetup.MetaKeyword,
                    MetaDescription = companysetup.MetaDescription,
                };
            }
        }
        #endregion
        public void OnGet()
        {
            setup();
        }

        public IActionResult OnPost()
        {
            #region Up-sert

            if (ModelState.IsValid)
            {
                CompanySetup cs = _dbContext.CompanySetups.FirstOrDefault();

                if (cs != null)
                {
                    cs.MetaTitle = CompanySetup.MetaTitle;
                    cs.MetaKeyword = CompanySetup.MetaKeyword;
                    cs.MetaDescription = CompanySetup.MetaDescription;



                    _dbContext.CompanySetups.Update(cs);
                    _dbContext.SaveChanges();

                    TempData["success"] = "SEO updated successfully";
                    setup();
                    return Page();
                }
            }

            else
            {
                setup();
                return Page();
            }
            return Page();
            #endregion
        }
    }
}
