using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.User
{
    public class billingModel : PageModel
    {

        [Authorize(Policy = "AllUsers")]
        public void OnGet()
        {
        }
    }
}
