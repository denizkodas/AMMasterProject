using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AMMasterProject.Pages.Admin.Partners
{


    [Authorize(Policy = "Community")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public WebsitesetupPartner partner { get; set; }

     

        #endregion
        #region DI

        public addModel(MyDbContext context)
        {
            _dbContext = context;


            partner = new WebsitesetupPartner();
            partner.IsPublish = true;
            partner.Isaddonhomepage = true;



        }
        #endregion


        #region DataPopulate    

        public void setup()
        {


            if (Request.Query.ContainsKey("ID"))
            {
                int blogid = int.Parse(Request.Query["ID"].ToString());


                partner = _dbContext.WebsitesetupPartners.FirstOrDefault(u => u.PartnerId == blogid);


            }



        }
        #endregion
        public void OnGet()
        {
            setup();




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

            if (partner.Sortorder <= 0)
            {
                ModelState.AddModelError("partner.Sortorder", "Sort order must be greater than 1");

                setup();
                return Page();
            }
            #endregion

            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Duplicat

                WebsitesetupPartner duplicate = _dbContext.WebsitesetupPartners.FirstOrDefault(u => u.ParnerName.ToLower().Trim() == partner.ParnerName.ToLower().Trim() && u.PartnerId != partner.PartnerId);
                if (duplicate != null)
                {


                    ModelState.AddModelError("partner.ParnerName", "Parner Name already exist.");
                    setup();
                    return Page();
                }

               



                #endregion



                #region Insert

                if (partner.PartnerId == 0)
                {
                    WebsitesetupPartner insert = new WebsitesetupPartner();

                    insert.Image = partner.Image;
                    insert.ParnerName = partner.ParnerName;
                    insert.Sortorder = partner.Sortorder;
                    insert.Partnerurl = partner.Partnerurl;
                    insert.Isaddonhomepage = partner.Isaddonhomepage;


                    insert.Insertdate = DateTime.Now;
                    insert.IsPublish = partner.IsPublish;
                    insert.ProfileId = loginid;





                    _dbContext.WebsitesetupPartners.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Partner added successfully";
                }

                #endregion
                else
                {
                    WebsitesetupPartner update = _dbContext.WebsitesetupPartners.FirstOrDefault(u => u.PartnerId == partner.PartnerId);

                    if (update != null)
                    {




                        update.Image = partner.Image;
                        update.ParnerName = partner.ParnerName;
                        update.Sortorder = partner.Sortorder;
                        update.Partnerurl = partner.Partnerurl;
                        update.Isaddonhomepage = partner.Isaddonhomepage;


                        update.Insertdate = DateTime.Now;
                        update.IsPublish = partner.IsPublish;
                        update.ProfileId = loginid;


                        _dbContext.WebsitesetupPartners.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Partner updated successfully";
                    }
                }

                #region Update

                #endregion
            }

            else
            {
                setup();
                return Page();
            }



            return RedirectToPage("/admin/Partners/Index");



            #endregion
        }
    }
}
