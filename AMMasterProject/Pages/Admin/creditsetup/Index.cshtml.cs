using AMMasterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.creditsetup
{

    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class IndexModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;

        public List<RevenueCreditPackage> revenuecreditpackage { get; set; }


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






            revenuecreditpackage = _dbContext.RevenueCreditPackage.Where(u=>u.IsDeleted==false).OrderBy(u => u.Sortnumber).ToList();

        }
        #endregion
        public void OnGet()
        {
            setup();
        }

        public IActionResult OnPostDelete(int creditid)
        {
            RevenueCreditPackage softDelete = _dbContext.RevenueCreditPackage.FirstOrDefault(u => u.RevenueCreditID == creditid);

            if (softDelete != null)
            {

                softDelete.IsDeleted = true;
                _dbContext.RevenueCreditPackage.Update(softDelete);
                _dbContext.SaveChanges();


                TempData["fail"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/creditsetup/index");


            }
            setup();
            return Page();
        }
    }
}
