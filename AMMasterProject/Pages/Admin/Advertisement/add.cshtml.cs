using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace AMMasterProject.Pages.Admin.Advertisement
{

    [Authorize(Policy = "Revenue")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public Advert advert { get; set; }



        #endregion
        #region DI

        public addModel(MyDbContext context)
        {
            _dbContext = context;


            advert = new Advert();
            advert.IsUrl = true;
            advert.StartDate = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            advert.EndDate = DateTime.ParseExact(DateTime.Now.Date.AddYears(1).ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);

        }
        #endregion
        #region DataPopulate    

        public void setup()
        {

            if (Request.Query.ContainsKey("ID"))
            {
                int adid = int.Parse(Request.Query["ID"].ToString());


                advert = _dbContext.Advert.FirstOrDefault(u => u.AdvertId == adid);


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

            if(advert.IsUrl ==true)
            {
                if(advert.Url ==null || advert.Url==string.Empty)
                {
                    ModelState.AddModelError("advert.Url ", "URL is required.");
                    setup();
                    return Page();
                }
            }


            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Duplicat

              


                #endregion



                #region Insert

                if (advert.AdvertId == 0)
                {
                    Advert insert = new Advert();

                
                    insert.AdsPageId = advert.AdsPageId;
                    insert.Image = advert.Image;
                    insert.Description = advert.Description;
                    insert.Status = advert.Status;
                    insert.IsUrl= advert.IsUrl;
                    insert.Url = advert.Url;
                    insert.InsertDate = DateTime.Now;
                    insert.StartDate = advert.StartDate;
                    insert.EndDate = advert.EndDate;

                    insert.ProfileId = loginid;

                    _dbContext.Advert.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Ads added successfully";
                }

                #endregion
                else
                {
                    Advert update = _dbContext.Advert.FirstOrDefault(a => a.AdvertId == loginid);

                    if (update != null)
                    {




                      
                        update.AdsPageId = advert.AdsPageId;
                        update.Image = advert.Image;
                        update.Description = advert.Description;
                        update.Status = advert.Status;
                        update.IsUrl = advert.IsUrl;
                        update.Url = advert.Url;

                        update.StartDate = advert.StartDate;
                        update.EndDate = advert.EndDate;

                        _dbContext.Advert.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Ads updated successfully";
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



            return RedirectToPage("/admin/Advertisement/Index");



            #endregion
        }
    }
}
