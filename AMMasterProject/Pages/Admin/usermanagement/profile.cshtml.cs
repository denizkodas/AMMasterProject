using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.usermanagement
{


    [Authorize(Policy = "Admin")]
    public class profileModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
