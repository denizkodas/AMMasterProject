using AMMasterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AMMasterProject.Pages.Admin.creditsetup
{


    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Models

        public RevenueCreditPackage revenuecreditpackage { get; set; }

        public IEnumerable<SelectListItem> Currency { get; set; }

        #endregion


        #region DI
        private readonly MyDbContext _dbContext;


        public addModel(MyDbContext context)
        {
            _dbContext = context;

            revenuecreditpackage = new RevenueCreditPackage();
            revenuecreditpackage.IsPublish = true;
            revenuecreditpackage.NoofExpiryDays = 0;
          
          

        }

        #endregion


        #region DataPopulate

        public void setup()
        {
            if (Request.Query.ContainsKey("ID"))
            {

                int creditid = int.Parse(Request.Query["ID"].ToString());
                revenuecreditpackage = _dbContext.RevenueCreditPackage.FirstOrDefault(u => u.RevenueCreditID == creditid && u.IsDeleted == false);

            }


            Currency = _dbContext.Currencies
                .Where(u => u.IsPublished == true)
                .Select(u => new SelectListItem
                {
                    Value = u.CurrencyId.ToString(),
                    Text = u.CurrencyName

                }).ToList();
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

            if (revenuecreditpackage.Sortnumber <= 0)
            {
                ModelState.AddModelError("revenuecreditpackage.SortOrder", "Sort order must be greater than 1");

                setup();
                return Page();
            }

            if (revenuecreditpackage.IsExpiry == true)
            {

                if (revenuecreditpackage.NoofExpiryDays == null)
                {
                    ModelState.AddModelError("revenuecreditpackage.NoofExpiryDays", "No. of ExpiryDays is required");
                    setup();
                    return Page();
                }

               

          


                if (revenuecreditpackage.NoofExpiryDays <= 0)
                {
                    ModelState.AddModelError("revenuecreditpackage.NoofExpiryDays", "It should be greater or equal to 1");
                    setup();
                    return Page();
                }


                
            }

            else
            {
                revenuecreditpackage.NoofExpiryDays = 0;
            }

            if (revenuecreditpackage.NoofCredit <= 0)
            {
                ModelState.AddModelError("revenuecreditpackage.NoofCredit", "It should be greater or equal to 1");

                setup();
                return Page();
            }

            if (revenuecreditpackage.CreditAmount <= 0)
            {
                ModelState.AddModelError("revenuecreditpackage.CreditAmount", "It should be greater or equal to 1");

                setup();
                return Page();
            }



            RevenueCreditPackage duplication = _dbContext.RevenueCreditPackage.FirstOrDefault(u => u.RevenueCreditName.Trim() == revenuecreditpackage.RevenueCreditName.Trim() && u.RevenueCreditID != revenuecreditpackage.RevenueCreditID && u.IsDeleted==false);

            if (duplication != null)
            {
                ModelState.AddModelError("revenuecreditpackage.RevenueCreditName", "Revenue Credit Name is already exist");

                setup();
                return Page();
            }

            #endregion


            #region Up-Sert

            if (ModelState.IsValid)
            {
                #region Insert
                if (revenuecreditpackage.RevenueCreditID == 0)
                {

                    RevenueCreditPackage insert = new RevenueCreditPackage();

                    insert.RevenueCreditName = revenuecreditpackage.RevenueCreditName.Trim();
                    insert.NoofCredit = revenuecreditpackage.NoofCredit;
                    insert.CurrencyID = revenuecreditpackage.CurrencyID;
                    insert.CreditAmount = revenuecreditpackage.CreditAmount;
                    insert.Sortnumber = revenuecreditpackage.Sortnumber;
                   
                    insert.IsPublish = revenuecreditpackage.IsPublish;
                    insert.IsRecommended = revenuecreditpackage.IsRecommended;
                    insert.ProfileId = loginid;
                    insert.InsertDate = DateTime.Now;
                    insert.IsDeleted = revenuecreditpackage.IsDeleted;




                    insert.Description = revenuecreditpackage.Description.Trim();


                    if (revenuecreditpackage.CreditImage != null)
                    {
                        insert.CreditImage = revenuecreditpackage.CreditImage.Trim();
                    }

                    insert.IsExpiry = revenuecreditpackage.IsExpiry;
                    insert.NoofExpiryDays = revenuecreditpackage.NoofExpiryDays;


                    _dbContext.RevenueCreditPackage.Add(insert);

                    _dbContext.SaveChanges();

                    TempData["submit"] = "Added successfully";


                }
                #endregion

                #region Update


                else
                {

                    RevenueCreditPackage update = _dbContext.RevenueCreditPackage.FirstOrDefault(u => u.RevenueCreditID == revenuecreditpackage.RevenueCreditID);
                    if (update != null)
                    {
                        update.RevenueCreditName = revenuecreditpackage.RevenueCreditName.Trim();
                        update.NoofCredit = revenuecreditpackage.NoofCredit;
                        update.CurrencyID = revenuecreditpackage.CurrencyID;
                        update.CreditAmount = revenuecreditpackage.CreditAmount;
                        update.Sortnumber = revenuecreditpackage.Sortnumber;
                      
                        update.IsPublish = revenuecreditpackage.IsPublish;
                        update.IsRecommended = revenuecreditpackage.IsRecommended;
                        update.ProfileId = loginid;
                        update.IsDeleted = revenuecreditpackage.IsDeleted;




                        update.Description = revenuecreditpackage.Description.Trim();

                        if (revenuecreditpackage.CreditImage != null)
                        {
                            update.CreditImage = revenuecreditpackage.CreditImage.Trim();
                        }

                        update.IsExpiry = revenuecreditpackage.IsExpiry;

                        
                        update.NoofExpiryDays = revenuecreditpackage.NoofExpiryDays;


                        _dbContext.RevenueCreditPackage.Update(update);

                        _dbContext.SaveChanges();

                        TempData["submit"] = "Updated successfully";





                    }

                }

                #endregion
            }

            else
            {
                setup();
                return Page();
            }
            #endregion



            return RedirectToPage("/admin/creditsetup/index");




        }
    }
}
