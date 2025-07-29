using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Email
{

    [Authorize(Policy = "Setup")]

    [Authorize]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Models

        public PageName PageDetail { get; set; }

        public string PageName { get; set; }


        public string pageHtml { get; set; }

        public string pageDescription { get; set; }
        public string pageCss { get; set; }
        public string pageJson { get; set; }

        public int pagenameid { get; set; }

        #endregion


        #region DI
        private readonly MyDbContext _dbContext;


        public addModel(MyDbContext context)
        {
            _dbContext = context;
            PageDetail = new PageName();
            PageDetail.IsPublish = true;

        }

        #endregion


        #region DataPopulate

        public void setup()
        {
            if (Request.Query.ContainsKey("ID"))
            {

                pagenameid = int.Parse(Request.Query["ID"].ToString());
                PageDetail = _dbContext.PageNames.FirstOrDefault(u => u.PageNameId == pagenameid);

                //pageHtml = PageDetail != null ? PageDetail.PageHTML : null;
                //pageCss = PageDetail != null ? PageDetail.PageCSS : null;
                //pageJson = PageDetail != null ? PageDetail.PageJson : null;

                pageDescription= PageDetail != null ? PageDetail.PageDescription : null;

                PageName = Request.Query["PageName"].ToString();

            }
        }


        #endregion
        public void OnGet()
        {
            setup();
        }

        public IActionResult OnPost()
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }


            if (Request.Query.ContainsKey("ID"))
            {

                pagenameid = int.Parse(Request.Query["ID"].ToString());
                PageName pg = _dbContext.PageNames.FirstOrDefault(u => u.PageNameId == pagenameid);


                pg.PageDescription = PageDetail.PageDescription;
                _dbContext.SaveChanges();
               

            }




       

            return RedirectToPage("/admin/email/list");



           
        }
    }
}
