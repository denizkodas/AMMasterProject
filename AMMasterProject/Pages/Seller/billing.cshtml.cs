using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Seller
{
    [Authorize(Policy = "Seller")]
    public class billingModel : PageModel
    {

     
        public void OnGet()
        {
        }
    }
}
