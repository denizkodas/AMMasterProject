using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AMMasterProject.Pages.Seller.Profile
{

    [Authorize(Policy = "Seller")]
    [BindProperties]
    public class businessModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;
        
        public SellerBusinessModel BusinessInfo { get; set; }
        public ProfileCompletionMetaData profileCompletionMetaData { get; set; }
        #endregion


        #region DI

        public businessModel(MyDbContext context, UserHelper userHelper)
        {
            _dbContext = context;
            _userHelper = userHelper;

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
            // Parse the BusinessMetaData JSON string to a JObject
           
            if (up != null)
            {
                profileCompletionMetaData = _userHelper.profilecompletestatus(up.ProfileVerificationMetaData);


                var businessMetaDataObject = new BusinessMetaData();


                if (up.BusinessMetaData == null)
                {
                    // Return a new instance of SellerProfileSettingsModel if json is null
                    businessMetaDataObject= new BusinessMetaData();
                }

                else
                {
                    businessMetaDataObject = JsonConvert.DeserializeObject<BusinessMetaData>(up.BusinessMetaData);
                }


                
               

                BusinessInfo = new SellerBusinessModel
                {
                    ProfileGuid = up.ProfileGuid,
                    BusinessType = up.BusinessType ?? 0,
                    BusinessUrlpath = up.BusinessUrlpath ?? GlobalHelper.SEOURL(up.BusinessName),
                    BusinessDescription=up.BusinessDescription,
                    Dateofbirth = businessMetaDataObject?.Dateofbirth,
                    Gender = businessMetaDataObject?.Gender,
                    FoundingYear = businessMetaDataObject?.FoundingYear,
                    NoOfEmployee = businessMetaDataObject?.NoOfEmployee





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
            try
            {

           
            #region ID
            //int loginid = 0;
            Guid ProfileGUID = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {
                //loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value ?? "0");
            }
            #endregion

            #region ModelValidation
            string message = _userHelper.BusinessURLValidation(BusinessInfo.BusinessUrlpath, ProfileGUID);

            if (message == "exist")
            {



                ModelState.AddModelError("BusinessInfo.BusinessUrlpath", "It already exist. Try another name.");

                setup();
                return Page();
            }


            if(BusinessInfo.BusinessType == 0)
            {
                if(BusinessInfo.Dateofbirth==null)
                {
                    ModelState.AddModelError("BusinessInfo.Dateofbirth", "Date of birth is required.");

                    setup();
                    return Page();
                }

                if (BusinessInfo.Gender == null)
                {
                    ModelState.AddModelError("BusinessInfo.Gender", "Gender is required.");

                    setup();
                    return Page();
                }
            }

            if (BusinessInfo.BusinessType == 1)
            {
                if (BusinessInfo.FoundingYear == null)
                {
                    ModelState.AddModelError("BusinessInfo.FoundingYear", "Founding Year is required.");

                    setup();
                    return Page();
                }

                if (BusinessInfo.NoOfEmployee == null)
                {
                    ModelState.AddModelError("BusinessInfo.NoOfEmployee", "No of Employee is required.");

                    setup();
                    return Page();
                }
            }

            #endregion


            #region Up-sert
           

                UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

                if (up != null)
                {


                    up.BusinessType = BusinessInfo.BusinessType;
                    up.BusinessUrlpath = GlobalHelper.SEOURL(BusinessInfo.BusinessUrlpath);
                    up.BusinessMetaData = _userHelper.businessinfometadata(BusinessInfo.Dateofbirth.ToString(), BusinessInfo.Gender, BusinessInfo.FoundingYear??0, BusinessInfo.NoOfEmployee??0);
                    up.BusinessDescription = BusinessInfo.BusinessDescription;
                    if (up.ProfileVerificationMetaData == null)
                    {
                        up.ProfileVerificationMetaData = _userHelper.profilecompletionmetadata(false, true, false, false, false, false, false, false);

                    }
                    else
                    {
                        // Parse the JSON string into a dynamic object
                        dynamic jsonObject = JsonConvert.DeserializeObject(up.ProfileVerificationMetaData);

                        // Update the "Business" value
                        jsonObject.BusinessInfo = true;

                        // Convert the updated object back to a JSON string
                        string updatedJson = JsonConvert.SerializeObject(jsonObject);

                        up.ProfileVerificationMetaData = updatedJson;
                    }



                 _dbContext.UsersProfiles.Update(up);
                _dbContext.SaveChanges();
              
             


            }

                TempData["success"] = "Business Info Updated successfully";
                setup();
                return Page();
                #endregion

            }
            catch (Exception ex)
            {

                TempData["success"] = ex.Message;
                setup();
                return Page();
            }
        }

    }
}
