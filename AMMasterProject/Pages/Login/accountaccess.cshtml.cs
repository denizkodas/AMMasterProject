using AMMasterProject.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;

namespace AMMasterProject.Pages.Login
{
    public class accountaccessModel : PageModel
    {
        private readonly MyDbContext _dbContext;

        public accountaccessModel(MyDbContext context)
        {
            _dbContext = context;
           
        }
        public async Task<IActionResult> OnGet()
        {
            string returnurl = "";
            try
            {
                if (RouteData.Values.TryGetValue("guid", out var routeGuidString))
                {
                    if (Guid.TryParse(routeGuidString.ToString(), out Guid userGUID))
                    {
                        UsersProfile usernameExists = await _dbContext.UsersProfiles.FirstOrDefaultAsync(u => u.ProfileGuid == userGUID);

                        if (usernameExists != null)
                        {
                            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usernameExists.UserName),
                        new Claim("UserGUID", usernameExists.ProfileGuid.ToString()),
                        new Claim("UserType", usernameExists.Type),
                        new Claim("UserID", usernameExists.ProfileId.ToString()),
                        new Claim("FirstName", usernameExists.Firstname ?? ""),
                        new Claim("LastName", usernameExists.Lastname ?? ""),
                        new Claim("Image", usernameExists.ProfileImage ?? ""),
                        new Claim("firstchar", usernameExists.Firstname?.Substring(0, 1)?.ToLower() ?? "")
                    };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            // Sign in the user
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
                            {
                                IsPersistent = true, // make the cookie persistent
                                ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(30)) // set the expiration time to 30 days
                            });

                           
                            if (usernameExists.Type == "Client" )
                            {
                                returnurl =  "~/Index?USERGUID=" + usernameExists.ProfileGuid;
                               
                            }
                            else if (usernameExists.Type == "Vendor")
                            {
                                returnurl = "/seller/Index?USERGUID=" + usernameExists.ProfileGuid;
                            }
                        }
                        else
                        {
                            TempData["success"] = "User Name does not exist.";
                        }
                    }
                    else
                    {
                        TempData["success"] = "Invalid GUID in the route.";
                    }
                }
                else
                {
                    TempData["error"] = "GUID not found in the route.";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here, log them, or take appropriate actions.
                TempData["error"] = "An error occurred.";
                // You might also want to log the exception details for debugging.
            }

            // Return an appropriate response, such as an error page or a redirect to a login page.
            return RedirectToAction(returnurl);
        }

    }
}
