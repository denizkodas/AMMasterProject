using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AMMasterProject.Pages.Admin.careers
{

    [Authorize]
    [BindProperties]
    public class addModel : PageModel
    {



        #region Model
        private readonly MyDbContext _dbContext;

        public Career career { get; set; }

        public IEnumerable<SelectListItem> CareerCategory { get; set; }


        #endregion

        #region DI

        public addModel(MyDbContext context)
        {
            _dbContext = context;


            career = new Career();
            career.IsPublish = true;
            career.Isaddonhomepage = true;



        }
        #endregion

        #region DataPopulate    

        public void setup()
        {

            CareerCategory = _dbContext.CareerCategories
            .Where(u => u.IsPublish == true)
             .Select(u => new SelectListItem
             {
                 Value = u.CareerCategoryId.ToString(),
                 Text = u.CareerCategoryName
             }).ToList();


            if (Request.Query.ContainsKey("ID"))
            {
                int careerid = int.Parse(Request.Query["ID"].ToString());


                career = _dbContext.Careers.FirstOrDefault(u => u.CareerId == careerid);


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


            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Duplicat

                Career duplicate = _dbContext.Careers.FirstOrDefault(u => u.Title.ToLower().Trim() == career.Title.ToLower().Trim() && u.CareerId != career.CareerId);
                if (duplicate != null)
                {


                    ModelState.AddModelError("career.Title", "Title already exist.");
                    setup();
                    return Page();
                }

                Career duplicateseo = _dbContext.Careers.FirstOrDefault(u => u.SeoPageName.ToLower().Trim() == career.SeoPageName.ToLower().Trim() && u.CareerId != career.CareerId);
                if (duplicateseo != null)
                {


                    ModelState.AddModelError("career.SeoPageName", "Seo Page Name already exist.");
                    setup();
                    return Page();
                }



                #endregion



                #region Insert

                if (career.CareerId == 0)
                {
                    Career insert = new Career();

                    insert.Title = career.Title;
                    insert.Categoryid = career.Categoryid;
                    //insert.Image = blog.Image;
                    insert.Summary = career.Summary;
                    insert.Description = career.Description;
                    insert.Isaddonhomepage = career.Isaddonhomepage;



                    if (career.Externalurl != null)
                    {
                        insert.Externalurl = career.Externalurl;
                    }

                    insert.SeoPageName = career.SeoPageName;
                    insert.SeoTitle = career.SeoTitle;
                    insert.SeoKeyword = career.SeoKeyword;
                    insert.SeoDescription = career.SeoDescription;


                    insert.InsertDate = DateTime.Now;
                    insert.IsPublish = career.IsPublish;
                    insert.ProfileId = loginid;

                    _dbContext.Careers.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Career added successfully";
                }

                #endregion
                else
                {
                    Career update = _dbContext.Careers.FirstOrDefault(u => u.CareerId == career.CareerId);

                    if (update != null)
                    {



                        update.Title = career.Title;
                        update.Categoryid = career.Categoryid;
                        //update.Image = blog.Image;
                        update.Summary = career.Summary;
                        update.Description = career.Description;
                        update.Isaddonhomepage = career.Isaddonhomepage;


                        //if (blog.Fileattached != null)
                        //{

                        //    update.Fileattached = blog.Fileattached;
                        //}

                        if (career.Externalurl != null)
                        {
                            update.Externalurl = career.Externalurl;
                        }

                        update.SeoPageName = career.SeoPageName;
                        update.SeoTitle = career.SeoTitle;
                        update.SeoKeyword = career.SeoKeyword;
                        update.SeoDescription = career.SeoDescription;


                        update.InsertDate = DateTime.Now;
                        update.IsPublish = career.IsPublish;
                        update.ProfileId = loginid;

                        _dbContext.Careers.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Career updated successfully";
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



            return RedirectToPage("/admin/careers/Index");



            #endregion
        }
    }
}
