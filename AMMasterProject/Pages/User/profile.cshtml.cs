using AMMasterProject.Helpers;
using AMMasterProject.Migrations;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PayPal.Api;
using System.Security.Claims;

namespace AMMasterProject.Pages.User
{
    [Authorize(Policy = "AllUsers")]
    [BindProperties]
    public class profileModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;
        private readonly UserHelper    _userHelper;

        private readonly IWebHostEnvironment _hostingEnvironment;
        public ClientProfileModel UserProfile { get; set; }

      
        #endregion

        #region DI

        public profileModel(MyDbContext context, UserHelper userHelper, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = context;
            _userHelper = userHelper;
            _hostingEnvironment = hostingEnvironment;
           
        }

        #endregion

        #region DataPopulate    

        public void setup()
        {
          
            Guid ProfileGUID= Guid.NewGuid();   
            if (User.Identity.IsAuthenticated)
            {
               
                ProfileGUID= Guid.Parse(User.FindFirst("UserGUID")?.Value);
                // continue with loginid variable
            }



            ClientViewModel clientview = _userHelper.ClientByGUID(ProfileGUID);

            if (clientview != null)
            {
                UserProfile = new ClientProfileModel
                {
                    ProfileGuid = clientview.ProfileGuid,
                    ProfileImage = clientview.Image ?? "/images/user-image-not-found.png",
                    Firstname = clientview.FirstName,
                    Lastname = clientview.LastName,
                    Email = clientview.Email,
                    ClientDisplayName = clientview.Displayname,
                    About = clientview.About,
                    Contactnumber = clientview.Contact,
                    
                   
                };
            }

        }
        #endregion

        
        public void OnGet()
        {

            setup();

          


        }

        public IActionResult OnPost()
        {
            #region ID
            int loginid = 0;
            Guid ProfileGUID = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value);
                // continue with loginid variable
            }
            #endregion


            #region Up-sert
           

                UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == UserProfile.ProfileGuid);

                if (up != null)
                {
                    up.ProfileImage = UserProfile.ProfileImage?? "/images/user-image-not-found.png";
                up.Firstname = UserProfile.Firstname;
                    up.Lastname = UserProfile.Lastname;
                    up.Email = UserProfile.Email;
                    up.ClientDisplayName = UserProfile.ClientDisplayName;
                    up.About = UserProfile.About;
                    up.ContactNumber = UserProfile.Contactnumber;

                    _dbContext.UsersProfiles.Update(up);
                    _dbContext.SaveChanges();

                    // Update the "Image" claim
                    var identity = (ClaimsIdentity)User.Identity;

                    // Remove existing claims
                    identity.RemoveClaim(identity.FindFirst("Image"));
                    identity.RemoveClaim(identity.FindFirst("FirstName"));
                    identity.RemoveClaim(identity.FindFirst("LastName"));
                    identity.RemoveClaim(identity.FindFirst("firstchar"));

                    // Add new claims
                    if (UserProfile.ProfileImage != null)
                    { 
                        identity.AddClaim(new Claim("Image", UserProfile.ProfileImage));
                }
                    identity.AddClaim(new Claim("FirstName", UserProfile.Firstname));
                    identity.AddClaim(new Claim("LastName", UserProfile.Lastname));
                    identity.AddClaim(new Claim("firstchar", UserProfile.Firstname.ToLower().Substring(0, 1)));

                    TempData["success"] = "Profile Updated successfully";
                
                }
            

            else
            {
                setup();
                return Page();
            }
            return Page();
            #endregion


        }

        
    }
}
