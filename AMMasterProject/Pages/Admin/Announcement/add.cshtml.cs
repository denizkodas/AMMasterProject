using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AMMasterProject.Pages.Admin.Announcement
{
    [Authorize(Policy = "Support")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public AnnouncementNotification _announcement { get; set; }


        #endregion
        #region DI

        public addModel(MyDbContext context)
        {
            _dbContext = context;


            _announcement = new AnnouncementNotification();
            _announcement.IsPublish = true;
            _announcement.StartDate = DateTime.Now;
            _announcement.ExpiryDate = DateTime.Now.AddDays(10);


        }

        #endregion



        public void OnGet()
        {

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

           

            #endregion
            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Insert

                    AnnouncementNotification announcement = new AnnouncementNotification();
                    announcement.Title = _announcement.Title.Trim();
                    announcement.Description = _announcement.Description.Trim();
                    announcement.AnnouncementFor = _announcement.AnnouncementFor;
                    announcement.StartDate = _announcement.StartDate;
                    announcement.ExpiryDate  = _announcement.ExpiryDate;
                    announcement.InsertDate = DateTime.Now;
                    announcement.IsPublish = _announcement.IsPublish;


                    _dbContext.AnnouncementNotification.Add(announcement);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Announcement added successfully";
                   return RedirectToPage("/admin/announcement/index");

                #endregion


                #region Update

                #endregion
            }

            else
            {
              
                return Page();
            }


            return RedirectToPage("/admin/announcement/index");
            #endregion
        }

    }
    
}
