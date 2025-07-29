using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Seller
{

    [Authorize(Policy = "Seller")]
    public class notificationModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
