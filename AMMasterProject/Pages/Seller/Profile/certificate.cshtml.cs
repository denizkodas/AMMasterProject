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
    public class certificateModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;

        public SellerCertificateModel certificate { get; set; }

        public List<CertificateMetaData> listcertificate { get; set; }
        public ProfileCompletionMetaData profileCompletionMetaData { get; set; }
        #endregion


        #region DI

        public certificateModel(MyDbContext context, UserHelper userHelper)
        {
            _dbContext = context;

            _userHelper = userHelper;

            certificate = new SellerCertificateModel();

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


                listcertificate = UserHelper.ParseMetaDataCertificateList(up.CertificateProofMetaData).ToList();

                if (Request.Query.ContainsKey("CertificateGUID"))
                {
                    string contactGuidString = Request.Query["CertificateGUID"];

                    var parsedData = listcertificate.FirstOrDefault(x => x.CertificateGUID.ToString() == contactGuidString);

                    if (parsedData != null)
                    {
                        certificate = new SellerCertificateModel
                        {
                            CertificateID = parsedData.CertificateID,
                            CertificateGUID = parsedData.CertificateGUID,
                            CertificateName = parsedData.CertificateName,
                            CertificateAttachment = parsedData.CertificateAttachment,
                            CourseContent = parsedData.CourseContent,
                            InstituteName = parsedData.InstituteName,



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
                if (certificate.CertificateAttachment == null)
                {
                    ModelState.AddModelError("certificate.CertificateAttachment", "Certificate is required.");

                    setup();
                    return Page();
                }

                #endregion


                #region Up-sert


                UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

                if (up != null)
                {


                    up.CertificateProofMetaData = _userHelper.certificatemetadata(certificate.CertificateGUID.ToString(), certificate.CertificateID.ToString(), certificate.CertificateAttachment,certificate.CertificateName, certificate.InstituteName, certificate.CourseContent, up.CertificateProofMetaData);

                    if (up.ProfileVerificationMetaData == null)
                    {
                        up.ProfileVerificationMetaData = _userHelper.profilecompletionmetadata(false, false, false, false, false, true, false, false);

                    }
                    else
                    {
                        // Parse the JSON string into a dynamic object
                        dynamic jsonObject = JsonConvert.DeserializeObject(up.ProfileVerificationMetaData);

                        // Update the "Contact" value
                        jsonObject.Certificate = true;

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

                TempData["success"] = "Certificate Updated successfully";


                return RedirectToPage("/seller/profile/certificate");
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

                listcertificate = UserHelper.ParseMetaDataCertificateList(up.CertificateProofMetaData).ToList();



                //var parsedData = _userHelper.ParseMetaDataContactList(up.SecondaryContactMetaData);

                // Find the index of the item to be deleted
                int indexToDelete = listcertificate.FindIndex(x => x.CertificateGUID.ToString() == id);

                if (indexToDelete >= 0)
                {
                    // Remove the item from the list
                    listcertificate.RemoveAt(indexToDelete);

                    // Serialize the updated list back to JSON
                    string updatedJson = JsonConvert.SerializeObject(listcertificate);

                    // Update the SecondaryContactMetaData property with the updated JSON
                    up.CertificateProofMetaData = updatedJson;

                    _dbContext.UsersProfiles.Update(up);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Deleted successfully";
                }
            }

            return RedirectToPage("/seller/profile/certificate");
        }
    }
}
