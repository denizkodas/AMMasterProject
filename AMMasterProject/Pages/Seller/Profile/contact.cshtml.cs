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
    public class contactModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;
        private readonly UserHelper _userHelper;
       
        public SellerContactModel Contact { get; set; }

        public  List<ContactMetaData> listContact { get; set; }
        public ProfileCompletionMetaData profileCompletionMetaData { get; set; }
        #endregion


        #region DI

        public contactModel(MyDbContext context, UserHelper userHelper)
        {
            _dbContext = context;
          
            _userHelper = userHelper;

            Contact=new SellerContactModel();
          

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


                listContact = _userHelper.ParseMetaDataContactList(up.SecondaryContactMetaData).ToList();

                if (Request.Query.ContainsKey("ContactGUID"))
                {
                    string contactGuidString = Request.Query["ContactGUID"];

                    var parsedData = listContact.FirstOrDefault(x => x.ContactGUID.ToString() == contactGuidString);
                   
                    if (parsedData != null)
                    {
                        Contact = new SellerContactModel
                        {
                            ContactID = parsedData.ContactID,
                            ContactGUID = parsedData.ContactGUID,
                            Name = parsedData.Name,
                            Type=parsedData.Type,
                            Contact=parsedData.Contact,
                            
                            
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

                string type = GlobalHelper.GetEmailOrPhone(Contact.Contact);

               if (Contact.Type!= type)
                {
                    
                        ModelState.AddModelError("Contact.Contact", "Valid " + Contact.Type + " is required.");

                        setup();
                        return Page();
                    
                }
              

            #endregion


            #region Up-sert


            UsersProfile up = _dbContext.UsersProfiles.FirstOrDefault(u => u.ProfileGuid == ProfileGUID);

            if (up != null)
            {


                up.SecondaryContactMetaData = _userHelper.contactmetadata(Contact.ContactGUID.ToString(), Contact.ContactID.ToString(), Contact.Name, Contact.Type,Contact.Contact,  up.SecondaryContactMetaData);
             
                if (up.ProfileVerificationMetaData == null)
                {
                    up.ProfileVerificationMetaData = _userHelper.profilecompletionmetadata(false, false, true, false, false, false, false, false);

                }
                else
                {
                    // Parse the JSON string into a dynamic object
                    dynamic jsonObject = JsonConvert.DeserializeObject(up.ProfileVerificationMetaData);

                    // Update the "Contact" value
                    jsonObject.SecondaryContactDetails = true;

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

             
                return RedirectToPage("/seller/profile/contact");
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
                
                listContact = _userHelper.ParseMetaDataContactList(up.SecondaryContactMetaData).ToList();

                

                //var parsedData = _userHelper.ParseMetaDataContactList(up.SecondaryContactMetaData);

                // Find the index of the item to be deleted
                int indexToDelete = listContact.FindIndex(x => x.ContactGUID.ToString() == id);

                if (indexToDelete >= 0)
                {
                    // Remove the item from the list
                    listContact.RemoveAt(indexToDelete);

                    // Serialize the updated list back to JSON
                    string updatedJson = JsonConvert.SerializeObject(listContact);

                    // Update the SecondaryContactMetaData property with the updated JSON
                    up.SecondaryContactMetaData = updatedJson;

                    _dbContext.UsersProfiles.Update(up);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Deleted successfully";
                }
            }

            return RedirectToPage("/seller/profile/contact");
        }

    }
}
