using AMMasterProject.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.pagessetup
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class IndexModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;
        private readonly WebsettingHelper _websettingHelper;
        public List<PageName> pagename { get; set; }


        #endregion

        #region DI
        
        public IndexModel(MyDbContext context, WebsettingHelper websettingHelper)
        {
            _dbContext = context;
            _websettingHelper=websettingHelper;

        }

        #endregion

        #region DataPopulate    

        public void setup()
        {


            

            

            pagename = _dbContext.PageNames.Where(u=>u.PageType =="page" || u.PageType == "cms").OrderBy(u => u.Type).ToList();

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
                page.PageType = "page";

                page.InsertDate = DateTime.Now;
                page.ProfileId = loginid;

                _dbContext.PageNames.Add(page);
                _dbContext.SaveChanges();


                ///Then duplicate the content
                ///
                if (page != null)
                {
                    PageName update = _dbContext.PageNames.FirstOrDefault(u => u.PageNameId == pagenameid);

                    if (update != null)
                    {
                        string message = _websettingHelper.updatehtmlcontent(update.PageDescription, update.PageHTML, update.PageHTML, update.PageJson, page.PageNameId, loginid);
                    
                       
                    
                    }

                   
                   TempData["success"] = "Duplicate created successfully";
                  

                }



                return RedirectToPage("/admin/pagessetup/index");


            }
            setup();
            return Page();
        }
        public IActionResult OnPostDelete(int pagenameid)
        {
            PageName pagename = _dbContext.PageNames.FirstOrDefault(u => u.PageNameId == pagenameid );

            if (pagename != null)
            {

                _dbContext.PageNames.Remove(pagename);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/pagessetup/index");

                
            }
            setup();
            return Page();
        }
    }
}
