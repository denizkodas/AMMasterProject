using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Drawing2D;

namespace AMMasterProject.Pages.Admin.pagessetup
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class EditpagecontentModel : PageModel
    {
        #region Models

        public PageName PageDetail { get; set; }

        public string PageName { get; set; }


        public string pageJson { get; set; }

        public int pagenameid { get; set; }

        #endregion


        #region DI
        private readonly MyDbContext _dbContext;


        public EditpagecontentModel(MyDbContext context)
        {
            _dbContext = context;
            PageDetail =new PageName ();
            PageDetail.IsPublish = true;

        }

        #endregion


        #region DataPopulate

        public void setup()
        {
            if (Request.Query.ContainsKey("ID"))
            {

                 pagenameid = int.Parse(Request.Query["ID"].ToString());
                PageDetail = _dbContext.PageNames.FirstOrDefault(u=>u.PageNameId== pagenameid);
                 //pageJson = PageDetail != null ? PageDetail.PageJson : null;


                PageName = Request.Query["PageName"].ToString();
                
            }
        }


        #endregion
        public void OnGet()
        {
            setup();
        }

        //public IActionResult OnPost()
        //{
        //    if (Request.Query.ContainsKey("ID"))
        //    {

        //        int loginid = 0;
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
        //            //continue with loginid variable
        //        }

        //        int pagenameid = int.Parse(Request.Query["ID"].ToString());
        //        PageDetail update = _dbContext.PageDetails.FirstOrDefault(u => u.PageNameId == pagenameid);
        //        if (update != null)
        //        {
        //            update.PageDescription = PageDetail.PageDescription;
        //            update.InsertDate = DateTime.Now;
        //            update.ProfileId = loginid;
        //            _dbContext.PageDetails.Update(update);

        //            _dbContext.SaveChanges();

        //            TempData["info"] = "Updated successfully";




        //            return RedirectToPage("/admin/pagessetup/index");
        //        }
        //        else
        //        {
        //            PageDetail insert = new PageDetail();

        //            insert.PageNameId = pagenameid;
        //            insert.PageDescription = PageDetail.PageDescription;
        //            insert.IsPublish = true;
        //            insert.ProfileId = loginid;
        //            _dbContext.PageDetails.Add(insert);
        //            _dbContext.SaveChanges();


        //            TempData["info"] = "Added successfully";




        //            return RedirectToPage("/admin/pagessetup/index");
        //        }


        //    }

        //    setup();
        //    return Page();
        //}
    }
}
