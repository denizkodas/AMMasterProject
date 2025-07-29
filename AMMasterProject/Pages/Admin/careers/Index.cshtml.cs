using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AMMasterProject.Pages.Admin.careers
{
    [Authorize]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;


        public List<CareerListView> careerlist { get; set; }

        #endregion

        #region DI

        public IndexModel(MyDbContext context)
        {
            _dbContext = context;





        }

        #endregion

        #region DataPopulate    

        public void setup()
        {




            careerlist = (from career in _dbContext.Careers
                          join category in _dbContext.CareerCategories on career.Categoryid equals category.CareerCategoryId
                          select new CareerListView
                          {
                              Title = career.Title,
                              Category = category.CareerCategoryName,
                              InsertDate = DateTime.Now,
                              IsPublish = career.IsPublish,

                              CareerId = career.CareerId,

                              TotalApplication = _dbContext.CareerApplications.Count(u => u.CareerGuid == career.CareerGuid)

                          }

                        ).ToList();

        }
        #endregion

        public void OnGet()
        {

            setup();
        }

        public IActionResult OnPostDelete(int careerId)
        {
            Career del = _dbContext.Careers.FirstOrDefault(u => u.CareerId == careerId);

            if (del != null)
            {

                _dbContext.Careers.Remove(del);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/careers/Index");


            }
            setup();
            return Page();
        }
    }
}
