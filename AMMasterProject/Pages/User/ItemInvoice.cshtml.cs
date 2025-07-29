using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.User
{
    [Authorize(Policy = "AllUsers")]
    public class ItemInvoiceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
