using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AMMasterProject.Pages.Admin.Team
{

    [Authorize(Policy = "Community")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public WebsitesetupTeam team { get; set; }

     

        #endregion
        #region DI

        public addModel(MyDbContext context)
        {
            _dbContext = context;


            team = new WebsitesetupTeam();
            team.IsPublish = true;
          


        }
        #endregion
        #region DataPopulate    

        public void setup()
        {

            if (Request.Query.ContainsKey("ID"))
            {
                int teamid = int.Parse(Request.Query["ID"].ToString());


                team = _dbContext.WebsitesetupTeams.FirstOrDefault(u => u.TeamId == teamid);


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

                WebsitesetupTeam duplicate = _dbContext.WebsitesetupTeams.FirstOrDefault(u => u.Name.ToLower().Trim() == team.Name.ToLower().Trim() && u.TeamId != team.TeamId);
                if (duplicate != null)
                {


                    ModelState.AddModelError("team.Name", "Name already exist.");
                    setup();
                    return Page();
                }




                #endregion



                #region Insert

                if (team.TeamId == 0)
                {
                    WebsitesetupTeam insert = new WebsitesetupTeam();

                    insert.Image= team.Image;
                    insert.Name = team.Name;
                    insert.Designation = team.Designation;
                    insert.Summary = team.Summary;
                    insert.Description  = team.Description;
                    insert.Sortorder=team.Sortorder;



                    insert.Insertdate = DateTime.Now;
                    insert.IsPublish = team.IsPublish;
                    insert.ProfileId = loginid;

                    _dbContext.WebsitesetupTeams.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Team added successfully";
                }

                #endregion
                else
                {
                    WebsitesetupTeam update = _dbContext.WebsitesetupTeams.FirstOrDefault(u => u.TeamId == team.TeamId);

                    if (update != null)
                    {




                        update.Image = team.Image;
                        update.Name = team.Name;
                        update.Designation = team.Designation;
                        update.Summary = team.Summary;
                        update.Description = team.Description;
                        update.Sortorder = team.Sortorder;


                        update.Insertdate = DateTime.Now;
                        update.IsPublish = team.IsPublish;
                        update.ProfileId = loginid;

                        _dbContext.WebsitesetupTeams.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Team updated successfully";
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



            return RedirectToPage("/admin/team/Index");



            #endregion
        }
    }
}
