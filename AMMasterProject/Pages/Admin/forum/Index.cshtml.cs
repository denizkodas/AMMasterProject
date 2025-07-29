using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin.forum
{

    [Authorize(Policy = "Community")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
