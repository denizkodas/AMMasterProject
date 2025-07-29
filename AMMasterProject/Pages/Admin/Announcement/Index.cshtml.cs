using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Announcement
{

    [Authorize(Policy = "Support")]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public List<AnnouncementNotification> announcement { get; set; }


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






            announcement = _dbContext.AnnouncementNotification.OrderBy(u => u.AnnouncementId).ToList();

        }
        #endregion
        public void OnGet()
        {
            setup();


        }
    }
}
