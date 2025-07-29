using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data.SqlTypes;

namespace AMMasterProject.Pages.Seller.Profile
{
    [Authorize(Policy = "Seller")]
    [BindProperties]
    public class AvailabilityModel : PageModel
    {
        #region DI
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;
        public List<SellerAvailabilityModel> sellerAvailabilityList { get; set; }

        public ProfileCompletionMetaData profileCompletionMetaData { get; set; }
        public AvailabilityModel(MyDbContext context, UserHelper userhelper)
        {
            _dbContext = context;
            _userHelper = userhelper;

        }
        #endregion
        public IActionResult OnGet()
        {
            Guid ProfileGUID = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {
                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value);
                // continue with loginid variable
            }

            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

            if (up != null)
            {

                profileCompletionMetaData = _userHelper.profilecompletestatus(up.ProfileVerificationMetaData);

                // Initialize a list to store the deserialized data

                if (up.AvailabilitySetupMetaData != null)
                {
                    // Assuming that the JSON data is stored as a string in the "ItemValue" field of the websiteSetup object
                    string jsonData = up.AvailabilitySetupMetaData;

                    // Deserialize the JSON string into a list of SellerSocialMediaModel objects
                    sellerAvailabilityList = JsonConvert.DeserializeObject<List<SellerAvailabilityModel>>(jsonData);
                }
                else
                {
                    string[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                    foreach (string day in daysOfWeek)
                    {
                        // Replace the empty strings and placeholders with your actual values
                        string message = _userHelper.availabilitySetupmetadata(up.ProfileId, day, true, false, "", "");

                        // Perform your desired actions with the message here
                    }

                    // Redirect to the same page to reload it
                    return RedirectToPage();
                }

                return Page();
            }

            // Handle the case where up is null, e.g., by returning an error page or redirecting to another page
            return NotFound();
        }
    }
}
