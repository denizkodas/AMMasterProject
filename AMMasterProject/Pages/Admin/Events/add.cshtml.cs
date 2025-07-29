using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace AMMasterProject.Pages.Admin.Events
{

    [Authorize(Policy = "Community")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public Event events { get; set; }

        public IEnumerable<SelectListItem> EventCategory { get; set; }

        #endregion
        #region DI

        public addModel(MyDbContext context)
        {
            _dbContext = context;


            events = new Event();
            events.IsPublish = true;
            events.Isaddonhomepage = true;



        }
        #endregion


        #region DataPopulate    

        public void setup()
        {

            EventCategory = _dbContext.EventCategories
            .Where(u => u.IsPublish == true)
             .Select(u => new SelectListItem
             {
                 Value = u.EventCategoryId.ToString(),
                 Text = u.EventCategoryName
             }).ToList();

            if (Request.Query.ContainsKey("ID"))
            {
                int eventid = int.Parse(Request.Query["ID"].ToString());


                events = _dbContext.Events.FirstOrDefault(u => u.EventId == eventid);


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

                Event duplicate = _dbContext.Events.FirstOrDefault(u => u.Title.ToLower().Trim() == events.Title.ToLower().Trim() && u.EventId != events.EventId);
                if (duplicate != null)
                {


                    ModelState.AddModelError("events.Title", "Title already exist.");
                    setup();
                    return Page();
                }

                Event duplicateseo = _dbContext.Events.FirstOrDefault(u => u.SeoPageName.ToLower().Trim() == events.SeoPageName.ToLower().Trim() && u.EventId != events.EventId);
                if (duplicateseo != null)
                {


                    ModelState.AddModelError("events.SeoPageName", "Seo Page Name already exist.");
                    setup();
                    return Page();
                }



                #endregion



                #region Insert

                if (events.EventId == 0)
                {
                    Event insert = new Event();

                    insert.Title = events.Title;
                    insert.Categoryid = events.Categoryid;
                    insert.EventImage = events.EventImage;
                    insert.Summary = events.Summary;
                    insert.Description = events.Description;
                    insert.EventStartDate=events.EventStartDate;
                    insert.EventStartTime = events.EventStartTime;
                    insert.EventEndDate = events.EventEndDate;
                    insert.EventEndTime = events.EventEndTime;
                    insert.TotalSeats = events.TotalSeats;
                    insert.Amount  = events.Amount;
                    insert.Isaddonhomepage = events.Isaddonhomepage;
                    insert.LastDateOfRegistration = events.LastDateOfRegistration;
                    insert.LastTimeOfRegistration= events.LastTimeOfRegistration;

                  

                    if (events.Externalurl != null)
                    {
                        insert.Externalurl = events.Externalurl;
                    }

                    insert.SeoPageName = events.SeoPageName;
                    insert.SeoTitle = events.SeoTitle;
                    insert.SeoKeyword = events.SeoKeyword;
                    insert.SeoDescription = events.SeoDescription;


                    insert.InsertDate = DateTime.Now;
                    insert.IsPublish = events.IsPublish;
                    insert.ProfileId = loginid;

                    _dbContext.Events.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Event added successfully";
                }

                #endregion
                else
                {
                    Event update = _dbContext.Events.FirstOrDefault(u => u.EventId == events.EventId);

                    if (update != null)
                    {



                        update.Title = events.Title;
                        update.Categoryid = events.Categoryid;
                        update.EventImage = events.EventImage;
                        update.Summary = events.Summary;
                        update.Description = events.Description;
                        update.Isaddonhomepage = events.Isaddonhomepage;


                        if (events.Externalurl != null)
                        {
                            update.Externalurl = events.Externalurl;
                        }

                        update.SeoPageName = events.SeoPageName;
                        update.SeoTitle = events.SeoTitle;
                        update.SeoKeyword = events.SeoKeyword;
                        update.SeoDescription = events.SeoDescription;


                        update.InsertDate = DateTime.Now;
                        update.IsPublish = events.IsPublish;
                        update.ProfileId = loginid;

                        _dbContext.Events.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Event updated successfully";
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



            return RedirectToPage("/admin/events/Index");



            #endregion
        }
    }
}
