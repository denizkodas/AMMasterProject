using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Partners
{

    [Authorize(Policy = "Community")]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;


        public List<WebsitesetupPartner> partnerlist { get; set; }

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




            partnerlist = _dbContext.WebsitesetupPartners.ToList();

        }
        #endregion

        public void OnGet()
        {

            setup();
        }

        public IActionResult OnPostDelete(int partnerid)
        {
            WebsitesetupPartner del = _dbContext.WebsitesetupPartners.FirstOrDefault(u => u.PartnerId == partnerid);

            if (del != null)
            {

                _dbContext.WebsitesetupPartners.Remove(del);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/partners/Index");


            }
            setup();
            return Page();
        }
    }
}
