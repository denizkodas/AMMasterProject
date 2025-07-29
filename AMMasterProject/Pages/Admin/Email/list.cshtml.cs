using AMMasterProject.Helpers;
using AMMasterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Email
{


    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class listModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        //public List<NotificationContent> notificationcontent { get; set; }
        private readonly WebsettingHelper _websettingHelper;
        public List<PageName> pagename { get; set; }
        #endregion

        #region DI

        public listModel(MyDbContext context, WebsettingHelper websettingHelper)
        {
            _dbContext = context;
            _websettingHelper = websettingHelper;

        }

        #endregion

        #region DataPopulate    

        public void setup()
        {



            pagename = _dbContext.PageNames.Where(u => u.PageType == "email").OrderBy(u => u.Sortnumber).ToList();


            //notificationcontent = _dbContext.NotificationContent.ToList();

        }
        #endregion
        public void OnGet()
        {
            setup();
        }

        public IActionResult OnPostDuplicate(int pagenameid)
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }
            PageName pagenameValidate = _dbContext.PageNames.FirstOrDefault(u => u.PageNameId == pagenameid);

            if (pagenameValidate != null)
            {

                ///First create duplicated page

                PageName page = new PageName();


                page.Name = pagenameValidate.Name.Trim() + " -Duplicate";
                page.PageCategoryId = pagenameValidate.PageCategoryId;
                page.Type = pagenameValidate.Type;
                page.IsPublish = false;
                page.IsUrl = pagenameValidate.IsUrl;
                page.Url = pagenameValidate.Url;
                page.InsertDate = DateTime.Now;
                page.Sortnumber = pagenameValidate.Sortnumber;
                page.SeoPageName = pagenameValidate.SeoPageName.Trim();
                page.SeoTitle = pagenameValidate.SeoTitle.Trim();
                page.SeoKeyword = pagenameValidate.SeoKeyword.Trim();
                page.SeoDescription = pagenameValidate.SeoDescription.Trim();
                page.IsAdminDefine = false;
                page.PageType = "email";

                page.InsertDate = DateTime.Now;
                page.ProfileId = loginid;

                _dbContext.PageNames.Add(page);
                _dbContext.SaveChanges();


                ///Then duplicate the content
                ///
                if (page != null)
                {
                    PageName update = _dbContext.PageNames.FirstOrDefault(u => u.PageNameId == pagenameid);

                  
                        string message = _websettingHelper.updatehtmlcontent(update.PageDescription, update.PageHTML, update.PageHTML, update.PageJson, page.PageNameId, loginid);



                    


                    TempData["success"] = "Duplicate created successfully";


                }



                return RedirectToPage("/admin/email/list");


            }
            setup();
            return Page();
        }
    }
}
