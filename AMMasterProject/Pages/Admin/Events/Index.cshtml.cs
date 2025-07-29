using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace AMMasterProject.Pages.Admin.Events
{

    [Authorize(Policy = "Community")]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;


        public List<EventListView> eventlist { get; set; }

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




            eventlist = (from events in _dbContext.Events
                        join category in _dbContext.EventCategories on events.Categoryid equals category.EventCategoryId
                        select new EventListView
                        {
                            Title = events.Title,
                            Category = category.EventCategoryName,
                            InsertDate = DateTime.Now,
                            IsPublish = events.IsPublish,
                            Image = events.EventImage,
                            EventId = events.EventId,
                            EventStartDate=events.EventStartDate

                        }

                        ).ToList();

        }
        #endregion

        public void OnGet()
        {

            setup();
        }

        public IActionResult OnPostDelete(int eventid)
        {
            Event del = _dbContext.Events.FirstOrDefault(u => u.EventId == eventid);

            if (del != null)
            {

                _dbContext.Events.Remove(del);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/events/Index");


            }
            setup();
            return Page();
        }
    }
}
