using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.cmssetup
{

    [Authorize]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Models

        public WebsiteSetupCm websitecms { get; set; }



        #endregion


        #region DI
        private readonly MyDbContext _dbContext;


        public addModel(MyDbContext context)
        {
            _dbContext = context;
          

        }

        #endregion


        #region DataPopulate

        public void setup()
        {
            if (Request.Query.ContainsKey("ID"))
            {

                int cmsid = int.Parse(Request.Query["ID"].ToString());
                websitecms = _dbContext.WebsiteSetupCms.FirstOrDefault(u => u.Cmsid == cmsid);

            }
        }


        #endregion
        public void OnGet()
        {
            setup();
        }

        public IActionResult OnPost()
        {
            if (Request.Query.ContainsKey("ID"))
            {

                int loginid = 0;
                if (User.Identity.IsAuthenticated)
                {
                    loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                    // continue with loginid variable
                }

                int cmsid = int.Parse(Request.Query["ID"].ToString());
                WebsiteSetupCm update = _dbContext.WebsiteSetupCms.FirstOrDefault(u => u.Cmsid == cmsid);
                if (update != null)
                {
                    update.Cmscontent = websitecms.Cmscontent;
                    update.InsertDate = DateTime.Now;
                 
                    _dbContext.WebsiteSetupCms.Update(update);

                    _dbContext.SaveChanges();

                    TempData["submit"] = "Updated successfully";




                    return RedirectToPage("/admin/cmssetup/index");
                }
               


            }

            setup();
            return Page();
        }
    }
}
