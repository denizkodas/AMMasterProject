using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AMMasterProject.Pages.Admin
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class socialmediasetupModel : PageModel
    {

        #region Model
        private readonly MyDbContext _dbContext;

        public WebsiteSetupSocialMedia socialmedia { get; set; }


        public List<WebsiteSetupSocialMedia> Listsocialmedia { get; set; }


        public int socialmediaid { get; set; }
        #endregion

        #region DI

        public socialmediasetupModel(MyDbContext context)
        {
            _dbContext = context;


            socialmedia = new  WebsiteSetupSocialMedia();
            socialmedia.IsPublish = true;

        }

        #endregion

        #region DataPopulate    

        public void setup()
        {


            Listsocialmedia = _dbContext.WebsiteSetupSocialMedia.ToList();


            
        }
        #endregion
        public void OnGet()
        {
            setup();

            if (Request.Query.ContainsKey("ID"))
            {
                socialmediaid = int.Parse(Request.Query["ID"].ToString());


                socialmedia = _dbContext.WebsiteSetupSocialMedia.FirstOrDefault(u => u.SocialMediaId ==socialmediaid);


            }
        }

        public IActionResult OnPost()
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

            #region ModelValidation

            if (socialmedia.Icon ==null)
            {
                ModelState.AddModelError("socialmedia.Icon", "Icon is required");

                setup();
                return Page();
            }

         

            #endregion
            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Insert

                if (socialmedia.SocialMediaId == 0)
                {

                    WebsiteSetupSocialMedia insert = new WebsiteSetupSocialMedia();

                    insert.Icon = socialmedia.Icon;
                    insert.Name = socialmedia.Name;
                    insert.IsPublish = socialmedia.IsPublish;
                  
                    insert.Url = socialmedia.Url;
                    
                    _dbContext.WebsiteSetupSocialMedia.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Social Media added successfully";
                }

                #endregion
                #region Update
                else
                {
                   
                    WebsiteSetupSocialMedia update = _dbContext.WebsiteSetupSocialMedia.FirstOrDefault(u => u.SocialMediaId ==socialmedia.SocialMediaId);

                    if (update != null)
                    {

                        update.Icon = socialmedia.Icon;
                        update.Name = socialmedia.Name;
                        update.IsPublish = socialmedia.IsPublish;
                        update.Icon = socialmedia.Icon;

                        update.Url = socialmedia.Url;

                        _dbContext.WebsiteSetupSocialMedia.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Social Media updated successfully";
                    }
                   
                }
                #endregion



            }

            else
            {
                setup();
                return Page();
            }


            return RedirectToPage("/admin/socialmediasetup");
            #endregion
        }
        public IActionResult OnPostDelete(int socialmediaid)
        {
            WebsiteSetupSocialMedia websitesocialmedia = _dbContext.WebsiteSetupSocialMedia.FirstOrDefault(u => u.SocialMediaId==socialmediaid );

            if (websitesocialmedia != null)
            {

                _dbContext.WebsiteSetupSocialMedia.Remove(websitesocialmedia);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";


                return RedirectToPage("/admin/socialmediasetup");

              
            }

            setup();
            return Page();
        }
    }
}
