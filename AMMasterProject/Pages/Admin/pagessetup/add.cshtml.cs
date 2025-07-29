using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AMMasterProject.Pages.Admin.pagessetup
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public PageName pagename { get; set; }
        public int pagenameid { get; set; }
        public IEnumerable<SelectListItem> listpagecategory { get; set; } 

        #endregion
        #region DI

        public addModel(MyDbContext context)
        {
            _dbContext = context;


            pagename = new PageName();
            pagename.IsPublish = true;
            pagename.IsUrl = false;
            pagename.IsAdminDefine = false;

           
        }

        #endregion

        #region DataPopulate    

        public void setup()
        {


            listpagecategory = _dbContext.PageCategories
            .Where(u => u.IsPublish == true)
             .Select(u => new SelectListItem
             {
                 Value = u.PageCategoryId.ToString(),
                 Text = u.Category
             }).OrderBy(u=>u.Text).ToList();





        }
        #endregion

        public void OnGet()
        {
            setup();

            if (Request.Query.ContainsKey("ID"))
            {
                pagenameid = int.Parse(Request.Query["ID"].ToString());


                pagename = _dbContext.PageNames.FirstOrDefault(u => u.PageNameId==pagenameid);


            }
        }

        public IActionResult OnPost()
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

            #region ModelValidation

            if (pagename.Sortnumber <= 0)
            {
                ModelState.AddModelError("pagename.SortOrder", "Sort order must be greater than 1");

                setup();
                return Page();
            }

            if (pagename.IsUrl ==true)
            {
                if (pagename.Url == null)
                {
                    ModelState.AddModelError("pagename.URL", "URL is required");

                    setup();
                    return Page();
                }
            }
            else
            {
                pagename.Url = string.Empty;
            }


            if (pagename.Type != "Header")
            {
                if (pagename.PageCategoryId == null || pagename.PageCategoryId == 0)
                {
                    ModelState.AddModelError("pagename.PageCategoryId", "Page Category is required");
                }
            }
            else
            {
                pagename.PageCategoryId = 0;
            }

            PageName pagenameduplication = _dbContext.PageNames.FirstOrDefault(u => u.Name ==pagename.Name.Trim().ToLower() && u.PageNameId !=pagename.PageNameId);

            if(pagenameduplication!=null)
            {
                ModelState.AddModelError("pagename.Name", "Page Name is already exist");

                setup();
                return Page();
            }
            PageName seopagename = _dbContext.PageNames.FirstOrDefault(u => u.Name == pagename.SeoPageName.Trim().ToLower() && u.PageNameId != pagename.PageNameId);

            if (seopagename != null)
            {
                ModelState.AddModelError("pagename.SeoPageName", "SEO Page Name is already exist");

                setup();
                return Page();
            }

          

            #endregion
            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Insert

                if (pagename.PageNameId == 0)
                {
                    PageName page = new PageName();
                    page.Name = pagename.Name.Trim();
                    page.PageCategoryId = pagename.PageCategoryId;
                    page.Type = pagename.Type;
                    page.IsPublish = pagename.IsPublish;
                    page.IsUrl = pagename.IsUrl;
                    page.Url = pagename.Url;
                    page.InsertDate = DateTime.Now;
                    page.Sortnumber=pagename.Sortnumber;
                    page.SeoPageName   = pagename.SeoPageName.Trim();
                    page.SeoTitle = pagename.SeoTitle.Trim();
                    page.SeoKeyword = pagename.SeoKeyword.Trim();
                    page.SeoDescription = pagename.SeoDescription.Trim();
                    page.IsAdminDefine = false;
                    page.PageType = "page";
                    

                    page.InsertDate = DateTime.Now;
                    page.ProfileId = loginid;

                    _dbContext.PageNames.Add(page);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Page Name added successfully";
                }

                #endregion
                else
                {
                    PageName update = _dbContext.PageNames.FirstOrDefault(u => u.PageNameId == pagename.PageNameId);

                    if (update != null)
                    {
                        update.Name = pagename.Name.Trim();
                        update.PageCategoryId = pagename.PageCategoryId;
                        update.Type = pagename.Type;
                        update.IsPublish = pagename.IsPublish;
                        update.IsUrl = pagename.IsUrl;
                        update.Url = pagename.Url;
                        update.InsertDate = DateTime.Now;
                        update.Sortnumber = pagename.Sortnumber;
                        update.SeoPageName = pagename.SeoPageName.Trim();
                        update.SeoTitle = pagename.SeoTitle.Trim();
                        update.SeoKeyword = pagename.SeoKeyword.Trim();
                        update.SeoDescription = pagename.SeoDescription.Trim();
                        update.PageType = "page";//cms
                        update.CMSKey = pagename.CMSKey;
                        update.InsertDate = DateTime.Now;
                        update.ProfileId = loginid;

                        _dbContext.PageNames.Update(update);
                        _dbContext.SaveChanges();
                    }
                }

                #region Update

                #endregion
            }

            else
            {
                setup();
                return Page();
            }


            return RedirectToPage("/admin/pagessetup/index");
            #endregion
        }
    }
}
