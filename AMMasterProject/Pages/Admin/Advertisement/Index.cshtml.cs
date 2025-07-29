using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace AMMasterProject.Pages.Admin.Advertisement
{

    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;


        public List<Advert> adslist { get; set; }

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




            adslist = _dbContext.Advert.ToList();

        }
        #endregion

        public void OnGet()
        {

            setup();
        }

        public IActionResult OnPostDelete(int adsid)
        {
            Advert del = _dbContext.Advert.FirstOrDefault(u => u.AdvertId == adsid);

            if (del != null)
            {

                _dbContext.Advert.Remove(del);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/Advertisement/Index");


            }
            setup();
            return Page();
        }
    }
}
