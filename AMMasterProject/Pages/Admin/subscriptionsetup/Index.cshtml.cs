using AMMasterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.subscriptionsetup
{

    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public List<RevenueSubscriptionPackage> revenuesubscriptionpackage { get; set; }


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






            revenuesubscriptionpackage = _dbContext.RevenueSubscriptionPackage.Where(u => u.IsDeleted == false).OrderBy(u => u.RevenuePackageName).ToList();

        }
        #endregion
        public void OnGet()
        {
            setup();
        }
        
    }
}
