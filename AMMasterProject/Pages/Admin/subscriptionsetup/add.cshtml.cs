using AMMasterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AMMasterProject.Pages.Admin.subscriptionsetup
{
    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Models

        public RevenueSubscriptionPackage revenuesubscriptionpackage { get; set; }

        public IEnumerable<SelectListItem> Currency { get; set; }

        #endregion


        #region DI
        private readonly MyDbContext _dbContext;


        public addModel(MyDbContext context)
        {
            _dbContext = context;

            revenuesubscriptionpackage = new RevenueSubscriptionPackage();
            revenuesubscriptionpackage.IsPublish = true;
          

        }

        #endregion


        #region DataPopulate

        public void setup()
        {
            if (Request.Query.ContainsKey("ID"))
            {

                int subscriptionid = int.Parse(Request.Query["ID"].ToString());
                revenuesubscriptionpackage = _dbContext.RevenueSubscriptionPackage.FirstOrDefault(u => u.RevenueSubscriptionPackageID == subscriptionid && u.IsDeleted==false);

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




            if (revenuesubscriptionpackage.Sortnumber <= 0)
            {
                ModelState.AddModelError("revenuesubscriptionpackage.SortOrder", "Sort order must be greater than 1");

                setup();
                return Page();
            }




            if (revenuesubscriptionpackage.CreditAmount <= 0)
            {
                ModelState.AddModelError("revenuesubscriptionpackage.CreditAmount", "It should be greater or equal to 1");

                setup();
                return Page();
            }

            if (revenuesubscriptionpackage.RecurringPeriodInDays <= 0)
            {
                ModelState.AddModelError("revenuesubscriptionpackage.RecurringPeriodInDays", "It should be greater or equal to 1");

                setup();
                return Page();
            }




            RevenueSubscriptionPackage duplication = _dbContext.RevenueSubscriptionPackage.FirstOrDefault(u => u.RevenuePackageName.Trim() == revenuesubscriptionpackage.RevenuePackageName.Trim() && u.RevenueSubscriptionPackageID != revenuesubscriptionpackage.RevenueSubscriptionPackageID && u.IsDeleted == false);

            if (duplication != null)
            {
                ModelState.AddModelError("revenuesubscriptionpackage.RevenuePackageName", "Revenue Subscription Name is already exist");

                setup();
                return Page();
            }

            #endregion


            #region Up-Sert

            if (ModelState.IsValid)
            {
                #region Insert
                if (revenuesubscriptionpackage.RevenueSubscriptionPackageID == 0)
                {

                    RevenueSubscriptionPackage insert = new RevenueSubscriptionPackage();

                    insert.RevenuePackageName = revenuesubscriptionpackage.RevenuePackageName.Trim();
                   
                    insert.CurrencyID = revenuesubscriptionpackage.CurrencyID;
                    insert.CreditAmount = revenuesubscriptionpackage.CreditAmount;
                    insert.RecurringPeriodInDays = revenuesubscriptionpackage.RecurringPeriodInDays;
                    insert.Sortnumber = revenuesubscriptionpackage.Sortnumber;

                    insert.IsPublish = revenuesubscriptionpackage.IsPublish;
                    insert.IsRecommended = revenuesubscriptionpackage.IsRecommended;
                    insert.ProfileId = loginid;
                    insert.InsertDate = DateTime.Now;
                    insert.IsDeleted = revenuesubscriptionpackage.IsDeleted;




                    insert.Description = revenuesubscriptionpackage.Description.Trim();

                    if (revenuesubscriptionpackage.SubscriptionImage != null)
                    {
                        insert.SubscriptionImage = revenuesubscriptionpackage.SubscriptionImage.Trim();

                    }


                    _dbContext.RevenueSubscriptionPackage.Add(insert);

                    _dbContext.SaveChanges();

                    TempData["submit"] = "Added successfully";


                }
                #endregion

                #region Update


                else
                {

                    RevenueSubscriptionPackage update = _dbContext.RevenueSubscriptionPackage.FirstOrDefault(u => u.RevenueSubscriptionPackageID == revenuesubscriptionpackage.RevenueSubscriptionPackageID);
                    if (update != null)
                    {
                        update.RevenuePackageName = revenuesubscriptionpackage.RevenuePackageName.Trim();

                        update.CurrencyID = revenuesubscriptionpackage.CurrencyID;
                        update.CreditAmount = revenuesubscriptionpackage.CreditAmount;
                        update.RecurringPeriodInDays = revenuesubscriptionpackage.RecurringPeriodInDays;
                        update.Sortnumber = revenuesubscriptionpackage.Sortnumber;

                        update.IsPublish = revenuesubscriptionpackage.IsPublish;
                        update.IsRecommended = revenuesubscriptionpackage.IsRecommended;
                        update.ProfileId = loginid;
                        update.InsertDate = DateTime.Now;
                        update.IsDeleted = revenuesubscriptionpackage.IsDeleted;




                        update.Description = revenuesubscriptionpackage.Description.Trim();

                        if (revenuesubscriptionpackage.SubscriptionImage != null)
                        {
                            update.SubscriptionImage = revenuesubscriptionpackage.SubscriptionImage.Trim();
                        }

                        _dbContext.RevenueSubscriptionPackage.Update(update);

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



            return RedirectToPage("/admin/subscriptionsetup/index");




        }
    }
}
