using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.usermanagement
{

    [Authorize(Policy = "Admin")]
    public class customerprofileModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
