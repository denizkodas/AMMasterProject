using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.User
{
    public class usermenuModel : PageModel
    {

        public bool IsMultiVendor { get; set; }
        public void OnGet()
        {

            IsMultiVendor = bool.Parse(HttpContext.Items["IsMultiVendor"].ToString());
        }
    }
}
