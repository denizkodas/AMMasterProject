using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Payment
{
    public class failedModel : PageModel
    {
        public string ReturnUrl { get; set; }
        public void OnGet()
        {

            if (Request.Query.ContainsKey("returnurl"))
            {
                ReturnUrl = Request.Query["returnurl"].ToString();
            }
        }
    }
}
