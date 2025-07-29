using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages
{
    public class licenseModel : PageModel
    {
        public string VersionNumber { get; set; }

        public string VersionDate { get; set; }
        public void OnGet()
        {
            VersionNumber = HttpContext.Items["VersionNumber"].ToString();
            VersionDate = HttpContext.Items["VersionDate"].ToString();
        }
    }
}
