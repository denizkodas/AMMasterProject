using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Seller.Profile
{
    [Authorize(Policy = "Seller")]
    [BindProperties]
    public class socialmedialinksModel : PageModel
    {

        #region DI
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;
        public List<SellerSocialMediaModel> sellerSocialMediaList { get; set; }

        public List<SocialMediaSettingViewModel> socialMediaList { get; set; }
        public ProfileCompletionMetaData profileCompletionMetaData { get; set; }
        public socialmedialinksModel(MyDbContext context, UserHelper userhelper)
        {
            _dbContext = context;
            _userHelper = userhelper;

        }
        #endregion
        public void OnGet()
        {

            Guid ProfileGUID = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {

                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value);
                // continue with loginid variable
            }



           
            // Fetch the JSON data from the "websitesetup" table
            // Fetch the JSON data from the "websitesetup" table
            Websetting websiteSetup = _dbContext.Websettings.FirstOrDefault(u => u.WebsettingKey == "SocialMediaSettings");


          
            if (websiteSetup != null)
            {
                // Assuming that the JSON data is stored as a string in the "ItemValue" field of the websiteSetup object
                string jsonData = websiteSetup.WebsettingValue;

                // Deserialize the JSON string into a list of SellerSocialMediaModel objects
                socialMediaList = JsonConvert.DeserializeObject<List<SocialMediaSettingViewModel>>(jsonData);

                socialMediaList = socialMediaList.Where(st => st.IsPublish == true).ToList();


                
            }



            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

            if(up!=null)
            {

                profileCompletionMetaData = _userHelper.profilecompletestatus(up.ProfileVerificationMetaData);
                // Initialize a list to store the deserialized data


                if (up.SocialMediaMetaData != null)
                {
                    // Assuming that the JSON data is stored as a string in the "ItemValue" field of the websiteSetup object
                    string jsonData = up.SocialMediaMetaData;

                    // Deserialize the JSON string into a list of SellerSocialMediaModel objects
                    sellerSocialMediaList = JsonConvert.DeserializeObject<List<SellerSocialMediaModel>>(jsonData);

                 


                }
                else
                {
                    sellerSocialMediaList = socialMediaList.Select(s => new SellerSocialMediaModel
                    {
                        SocialMediaID = s.ID,
                        URL = string.Empty // Initialize with empty URL or any other default value
                    }).ToList();
                }
            }
            
        }
    }
}
