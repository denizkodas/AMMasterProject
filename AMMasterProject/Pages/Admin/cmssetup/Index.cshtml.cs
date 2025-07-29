using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.cmssetup
{
    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;

        public List<WebsiteSetupCm> cms { get; set; }


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






            cms = _dbContext.WebsiteSetupCms.OrderBy(u => u.Cmsid).ToList();

        }
        #endregion
        public void OnGet()
        {
            setup();
        }

           
    }
}
