using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.Donation
{

    [Authorize(Policy = "Community")]
    public class categoryModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
