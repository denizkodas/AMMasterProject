using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.careers
{


    [Authorize]
    [BindProperties]
    public class CategoryModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public CareerCategory careercategory { get; set; }

        public List<CareerCategory> careercategorylist { get; set; }

        #endregion

        #region DI

        public CategoryModel(MyDbContext context)
        {
            _dbContext = context;


            careercategory = new CareerCategory();
            careercategory.IsPublish = true;



        }
        #endregion

        #region DataPopulate    

        public void setup()
        {



            if (Request.Query.ContainsKey("ID"))
            {
                int categoryid = int.Parse(Request.Query["ID"].ToString());


                careercategory = _dbContext.CareerCategories.FirstOrDefault(u => u.CareerCategoryId == categoryid);


            }

            careercategorylist = _dbContext.CareerCategories.ToList();

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

                CareerCategory duplicate = _dbContext.CareerCategories.FirstOrDefault(u => u.CareerCategoryName.ToLower().Trim() ==careercategory.CareerCategoryName.ToLower().Trim() && u.CareerCategoryId != careercategory.CareerCategoryId);
                if (duplicate != null)
                {


                    ModelState.AddModelError("careercategory.CareerCategoryName", "Category Name already exist.");
                    setup();
                    return Page();
                }


                #endregion



                #region Insert

                if (careercategory.CareerCategoryId == 0)
                {
                    CareerCategory insert = new CareerCategory();


                    insert.CareerCategoryName = careercategory.CareerCategoryName.Trim();
                    insert.ProfileId = loginid;
                    insert.InsertDate = DateTime.Now;
                    insert.IsPublish = careercategory.IsPublish;

                    _dbContext.CareerCategories.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Category added successfully";
                }

                #endregion
                else
                {
                    CareerCategory update = _dbContext.CareerCategories.FirstOrDefault(u => u.CareerCategoryId == careercategory.CareerCategoryId);

                    if (update != null)
                    {



                        update.CareerCategoryName = careercategory.CareerCategoryName.Trim();
                        update.ProfileId = loginid;

                        update.IsPublish = careercategory.IsPublish;

                        _dbContext.CareerCategories.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Category updated successfully";
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



            return RedirectToPage("/admin/careers/category");



            #endregion
        }

        public IActionResult OnPostDelete(int careercategoryid)
        {
            CareerCategory del = _dbContext.CareerCategories.FirstOrDefault(u => u.CareerCategoryId == careercategoryid);

            if (del != null)
            {

                _dbContext.CareerCategories.Remove(del);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/careers/category");


            }
            setup();
            return Page();
        }
    }
}
