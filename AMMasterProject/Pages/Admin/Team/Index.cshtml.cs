using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Team
{

    [Authorize(Policy = "Community")]
    [BindProperties]
    public class IndexModel : PageModel
    {


        #region Model
        private readonly MyDbContext _dbContext;


        public List<WebsitesetupTeam> teamlist { get; set; }

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




            teamlist = _dbContext.WebsitesetupTeams.ToList();

        }
        #endregion

        public void OnGet()
        {

            setup();
        }

        public IActionResult OnPostDelete(int teamid)
        {
            WebsitesetupTeam del = _dbContext.WebsitesetupTeams.FirstOrDefault(u => u.TeamId == teamid);

            if (del != null)
            {

                _dbContext.WebsitesetupTeams.Remove(del);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

                setup();
                return RedirectToPage("/admin/team/Index");


            }
            setup();
            return Page();
        }
    }
}
