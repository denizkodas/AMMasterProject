using AMMasterProject.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.login
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            string currentUrl = GlobalHelper.GetCurrentDomainName();
            GlobalHelper.SetCookie("returnurl", currentUrl);

            var returnUrl = HttpContext.Request.Query["ReturnUrl"].ToString();

            // In case if user types /admin, redirect the user to /admin/login
            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Contains("/admin", StringComparison.OrdinalIgnoreCase))
            {
                // Set login path for admin
                Response.Redirect("/admin/login");
                return; // Ensure the method exits after redirecting
            }

            // If the user is already logged in, redirect to the error page
            
            if (User.Identity.IsAuthenticated)
            {
                // Redirect user to the error page
                Response.Redirect($"/Error?Title={Uri.EscapeDataString("Already Logged In")}&Body={Uri.EscapeDataString("You are already logged in. If you want to login as another user, log out first and then log in again.")}");
                return; // Ensure the method exits after redirecting
            }
        }
    }
}
