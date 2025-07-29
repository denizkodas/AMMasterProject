using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AMMasterProject.Pages.Login
{

    [BindProperties]
    public class registerModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;

        public string usertype { get; set; }
      

      
        #endregion

        #region DI






        public registerModel(MyDbContext context)
        {
            _dbContext = context;
             
           
        }

        #endregion



        public void OnGet()
        {
            if (Request.Query.ContainsKey("usertype"))
            {
                usertype = Request.Query["usertype"].ToString();
            }

            if(User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Error?Title=Multiple Registration&Message=You are already loggedin&Body=You are already logged in. If you want to create a new account, please log out and try again.");
            }
        }
    }
}
