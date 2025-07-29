using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AMMasterProject.Pages.Login
{
    [BindProperties]
    public class adminloginModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;

        public LoginModel loginviewmodel { get; set; }


        public adminloginModel(MyDbContext context, UserHelper userHelper)
        {
            _dbContext = context;
            _userHelper = userHelper;
            loginviewmodel = new LoginModel();

        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                string password = EncryptionHelper.encryption(loginviewmodel.Password);

                UsersProfile usernameExists = await _dbContext.UsersProfiles.FirstOrDefaultAsync(u => u.UserName == loginviewmodel.UserName && u.Password == password && u.Type == "Admin");

                if (usernameExists != null)
                {
                    ///create user agent
                    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                    string useragentModel = _userHelper.GetUserAgentAsJson(userAgent);
                    _userHelper.UserOtherMetaDataUpdate(usernameExists.ProfileId, useragentModel, "UserAgent");

                    usernameExists.Lastlogin = DateTime.Now;
                    _dbContext.UsersProfiles.Update(usernameExists);
                    _dbContext.SaveChanges();





                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usernameExists.UserName),
                new Claim("UserGUID", usernameExists.ProfileGuid.ToString()),
                new Claim("UserType", usernameExists.Type),
                new Claim("UserID", usernameExists.ProfileId.ToString()),
                new Claim("FirstName", usernameExists.Firstname ?? "admin"),
                new Claim("LastName", usernameExists.Lastname ?? ""),
                new Claim("Image", usernameExists.ProfileImage ?? ""),
                 new Claim("firstchar", usernameExists.Firstname.ToLower().Substring(0,1))
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in the user

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(30))
                    });


                    //await HttpContext.SignInAsync("AdminCookie", new ClaimsPrincipal(identity), new AuthenticationProperties
                    //{
                    //    IsPersistent = true,
                    //    ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromDays(30))
                    //});

                    return Redirect("/admin/");
                }
                else
                {
                    TempData["success"] = "User Name does not exist.";
                }
            }

            return Page();
        }
    }
}
