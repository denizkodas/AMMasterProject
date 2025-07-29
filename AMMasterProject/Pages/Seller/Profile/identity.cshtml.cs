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
    public class identityModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;

        public SellerIdentityModel identity { get; set; }

        public List<IdentityMetaData> listidentity { get; set; }
        public ProfileCompletionMetaData profileCompletionMetaData { get; set; }
        #endregion


        #region DI

        public identityModel(MyDbContext context, UserHelper userHelper)
        {
            _dbContext = context;

            _userHelper = userHelper;

            identity = new SellerIdentityModel();
           
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


                listidentity = _userHelper.ParseMetaDataIdentityList(up.IdentityProofMetaData).ToList();

                if (Request.Query.ContainsKey("IdentityGUID"))
                {
                    string contactGuidString = Request.Query["IdentityGUID"];

                    var parsedData = listidentity.FirstOrDefault(x => x.IdentityGUID.ToString() == contactGuidString);

                    if (parsedData != null)
                    {
                        identity = new SellerIdentityModel
                        {
                            IdentityID = parsedData.IdentityID,
                            IdentityGUID = parsedData.IdentityGUID,
                            IdentityType = parsedData.IdentityType,
                            IdentityProof = parsedData.IdentityProof,
                            
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
                if (identity.IdentityProof == null)
                {
                    ModelState.AddModelError("team.IdentityProof", "Identity is required.");

                    setup();
                    return Page();
                }

                #endregion


                #region Up-sert


                UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

                if (up != null)
                {


                    up.IdentityProofMetaData = _userHelper.identitymetadata(identity.IdentityGUID.ToString(), identity.IdentityID.ToString(), identity.IdentityType, identity.IdentityProof,  up.IdentityProofMetaData);

                    if (up.ProfileVerificationMetaData == null)
                    {
                        up.ProfileVerificationMetaData = _userHelper.profilecompletionmetadata(false, false, false, false, true, false, false, false);

                    }
                    else
                    {
                        // Parse the JSON string into a dynamic object
                        dynamic jsonObject = JsonConvert.DeserializeObject(up.ProfileVerificationMetaData);

                        // Update the "Contact" value
                        jsonObject.IdentityProof = true;

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

                TempData["success"] = "Identity Updated successfully";


                return RedirectToPage("/seller/profile/identity");
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
                
                listidentity = _userHelper.ParseMetaDataIdentityList(up.IdentityProofMetaData).ToList();



                //var parsedData = _userHelper.ParseMetaDataContactList(up.SecondaryContactMetaData);

                // Find the index of the item to be deleted
                int indexToDelete = listidentity.FindIndex(x => x.IdentityGUID.ToString() == id);

                if (indexToDelete >= 0)
                {
                    // Remove the item from the list
                    listidentity.RemoveAt(indexToDelete);

                    // Serialize the updated list back to JSON
                    string updatedJson = JsonConvert.SerializeObject(listidentity);

                    // Update the SecondaryContactMetaData property with the updated JSON
                    up.IdentityProofMetaData = updatedJson;

                    _dbContext.UsersProfiles.Update(up);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Deleted successfully";
                }
            }

            return RedirectToPage("/seller/profile/identity");
        }

    }
}
