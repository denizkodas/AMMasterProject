using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Events
{
    [Authorize(Policy = "Community")]
    [BindProperties]
    public class categoryModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;

        public EventCategory category { get; set; }

        public List<EventCategory> categorylist { get; set; }

        #endregion

        #region DI

        public categoryModel(MyDbContext context)
        {
            _dbContext = context;


            category = new EventCategory();
            category.IsPublish = true;



        }
        #endregion

        #region DataPopulate    

        public void setup()
        {



            if (Request.Query.ContainsKey("ID"))
            {
                int categoryid = int.Parse(Request.Query["ID"].ToString());


                category = _dbContext.EventCategories.FirstOrDefault(u => u.EventCategoryId == categoryid);


            }

            categorylist = _dbContext.EventCategories.ToList();

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

                EventCategory duplicate = _dbContext.EventCategories.FirstOrDefault(u => u.EventCategoryName.ToLower().Trim() == category.EventCategoryName.ToLower().Trim() && u.EventCategoryId != category.EventCategoryId);
                if (duplicate != null)
                {


                    ModelState.AddModelError("category.EventCategoryName", "Category Name already exist.");
                    setup();
                    return Page();
                }


                #endregion



                #region Insert

                if (category.EventCategoryId == 0)
                {
                    EventCategory insert = new EventCategory();


                    insert.EventCategoryName = category.EventCategoryName.Trim();
                    insert.ProfileId = loginid;
                    insert.InsertDate = DateTime.Now;
                    insert.IsPublish = category.IsPublish;

                    _dbContext.EventCategories.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Event Category added successfully";
                }

                #endregion
                else
                {
                    EventCategory update = _dbContext.EventCategories.FirstOrDefault(u => u.EventCategoryId == category.EventCategoryId);

                    if (update != null)
                    {



                        update.EventCategoryName = category.EventCategoryName.Trim();
                        update.ProfileId = loginid;

                        update.IsPublish = category.IsPublish;

                        _dbContext.EventCategories.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Event Category updated successfully";
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



            return RedirectToPage("/admin/blog/category");



            #endregion
        }

        public IActionResult OnPostDelete(int eventcategoryid)
        {
            EventCategory del = _dbContext.EventCategories.FirstOrDefault(u => u.EventCategoryId == eventcategoryid);

            if (del != null)
            {

                _dbContext.EventCategories.Remove(del);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/events/category");


            }
            setup();
            return Page();
        }
    }
}
