using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.faq
{


    [Authorize(Policy = "Community")]
    [BindProperties]
    public class IndexModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;


        public List<FAQ> faqlist { get; set; }

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




            faqlist = _dbContext.FAQs.ToList();

        }
        #endregion

        public void OnGet()
        {
            setup();
        }

        public IActionResult OnPostDelete(int faqid)
        {
            FAQ del = _dbContext.FAQs.FirstOrDefault(u => u.FAQId == faqid);

            if (del != null)
            {

                _dbContext.FAQs.Remove(del);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/faq/Index");


            }
            setup();
            return Page();
        }
    }
}
