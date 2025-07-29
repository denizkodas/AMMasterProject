using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Editor
{

    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Models

        public PageName PageDetail { get; set; }

        public string PageName { get; set; }


        public string pageHtml { get; set; }
        public string pageCss { get; set; }
        public string pageJson { get; set; }

        public int pagenameid { get; set; }

        #endregion


        #region DI
        private readonly MyDbContext _dbContext;


        public IndexModel(MyDbContext context)
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
                 
                pageHtml = PageDetail != null ? PageDetail.PageHTML : null;
                pageCss = PageDetail != null ? PageDetail.PageCSS : null;
                pageJson= PageDetail != null ? PageDetail.PageJson : null;

                PageName = Request.Query["PageName"].ToString();

            }
        }


        #endregion
        public void OnGet()
        {
            setup();
        }
    }
}
