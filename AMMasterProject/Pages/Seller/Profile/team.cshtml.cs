using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace AMMasterProject.Pages.Seller.Profile
{
    [Authorize(Policy = "Seller")]
    [BindProperties]
    public class teamModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;

        public SellerTeamMemberModel team { get; set; }

        public List<TeamMetaData> listteam { get; set; }
        public ProfileCompletionMetaData profileCompletionMetaData { get; set; }
        #endregion


        #region DI

        public teamModel(MyDbContext context, UserHelper userHelper)
        {
            _dbContext = context;

            _userHelper = userHelper;

            team = new SellerTeamMemberModel();
           
            team.IsCreateLogin = false;

        }

        #endregion

        #region DataPopulate    

        public void setup()
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


                listteam = UserHelper.ParseMetaDataTeamList(up.TeamMembersMetaData).ToList();

                if (Request.Query.ContainsKey("TeamGUID"))
                {
                    string contactGuidString = Request.Query["TeamGUID"];

                    var parsedData = listteam.FirstOrDefault(x => x.TeamGUID.ToString() == contactGuidString);

                    if (parsedData != null)
                    {
                        team = new SellerTeamMemberModel
                        {
                            TeamID = parsedData.TeamID,
                            TeamGUID = parsedData.TeamGUID,
                            Name = parsedData.Name,
                            Role = parsedData.Role,
                            Speciality = parsedData.Speciality,
                            Experience = parsedData.Experience,
                            ContactNumber = parsedData.ContactNumber,
                            Email = parsedData.Email,
                            YearsOfExperience = parsedData.YearsOfExperience,
                           

                            IsCreateLogin=parsedData.IsCreateLogin,
                            Image = parsedData.Image
                        };
                    }
                }
            }









        }
        #endregion

        public void OnGet()
        {

            setup();
        }

        public IActionResult OnPost()
        {

            try
            {


                #region ID
                //int loginid = 0;
                Guid ProfileGUID = Guid.NewGuid();
                if (User.Identity.IsAuthenticated)
                {

                    ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value ?? "0");
                }
                #endregion

                #region ModelValidation
                if (team.Image == null)
                {
                    ModelState.AddModelError("team.Image", "Image is required.");

                    setup();
                    return Page();
                }

                #endregion


                #region Up-sert


                UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

                if (up != null)
                {


                    up.TeamMembersMetaData = _userHelper.teammetadata(team.TeamGUID.ToString(), team.TeamID.ToString(), team.Name, team.Role, team.Speciality, team.Experience, team.ContactNumber, team.Email, team.YearsOfExperience,team.IsCreateLogin, team.Image, up.TeamMembersMetaData);

                    if (up.ProfileVerificationMetaData == null)
                    {
                        up.ProfileVerificationMetaData = _userHelper.profilecompletionmetadata(false, false, false, false, false, false, false, true);

                    }
                    else
                    {
                        // Parse the JSON string into a dynamic object
                        dynamic jsonObject = JsonConvert.DeserializeObject(up.ProfileVerificationMetaData);

                        // Update the "Contact" value
                        jsonObject.TeamMemmber = true;

                        // Convert the updated object back to a JSON string
                        string updatedJson = JsonConvert.SerializeObject(jsonObject);

                        up.ProfileVerificationMetaData = updatedJson;
                    }



                    _dbContext.UsersProfiles.Update(up);
                    _dbContext.SaveChanges();

                    //TempData["success"] = "Contacts Updated successfully";
                    //setup();
                    //return Page();



                }

                TempData["success"] = "Contacts Updated successfully";


                return RedirectToPage("/seller/profile/team");
                #endregion

            }
            catch (Exception ex)
            {

                TempData["success"] = ex.Message;
                setup();
                return Page();
            }
        }

        public IActionResult OnPostDelete(string id)
        {
            Guid ProfileGUID = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {
                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value ?? "0");
            }

            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

            if (up != null)
            {
               
                listteam = UserHelper.ParseMetaDataTeamList(up.TeamMembersMetaData).ToList();



                //var parsedData = _userHelper.ParseMetaDataContactList(up.SecondaryContactMetaData);

                // Find the index of the item to be deleted
                int indexToDelete = listteam.FindIndex(x => x.TeamGUID.ToString() == id);

                if (indexToDelete >= 0)
                {
                    // Remove the item from the list
                    listteam.RemoveAt(indexToDelete);

                    // Serialize the updated list back to JSON
                    string updatedJson = JsonConvert.SerializeObject(listteam);

                    // Update the SecondaryContactMetaData property with the updated JSON
                    up.TeamMembersMetaData = updatedJson;

                    _dbContext.UsersProfiles.Update(up);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Deleted successfully";
                }
            }

            return RedirectToPage("/seller/profile/team");
        }

    }
}
