using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AMMasterProject.Pages.Admin.faq
{

    [Authorize(Policy = "Community")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public FAQ faq { get; set; }

       
        #endregion

        #region DI

        public addModel(MyDbContext context)
        {
            _dbContext = context;


            faq = new FAQ();
            faq.IsPublish = true;



        }
        #endregion

        #region DataPopulate    

        public void setup()
        {



            if (Request.Query.ContainsKey("ID"))
            {
                int faqid = int.Parse(Request.Query["ID"].ToString());


                faq = _dbContext.FAQs.FirstOrDefault(u => u.FAQId == faqid);


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

            if (faq.Sortorder <= 0)
            {
                ModelState.AddModelError("faq.Sortorder", "Sort order must be greater than 1");

                setup();
                return Page();
            }
            #endregion

            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Duplicat

                FAQ duplicate = _dbContext.FAQs.FirstOrDefault(u => u.Question.ToLower().Trim() == faq.Question.ToLower().Trim() && u.FAQId != faq.FAQId);
                if (duplicate != null)
                {


                    ModelState.AddModelError("faq.Question", "Question already exist.");
                    setup();
                    return Page();
                }


                #endregion



                #region Insert

                if (faq.FAQId == 0)
                {
                    FAQ insert = new FAQ();


                    insert.Question = faq.Question.Trim();
                    insert.Answer = faq.Answer.Trim();
                    insert.Sortorder    = faq.Sortorder;


                    insert.ProfileId = loginid;
                    insert.Insertdate = DateTime.Now;
                    insert.IsPublish = faq.IsPublish;

                    _dbContext.FAQs.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Faq added successfully";
                }

                #endregion
                else
                {
                    FAQ update = _dbContext.FAQs.FirstOrDefault(u => u.FAQId == faq.FAQId);

                    if (update != null)
                    {



                        update.Question = faq.Question.Trim();
                        update.Answer = faq.Answer.Trim();
                        update.Sortorder = faq.Sortorder;

                        _dbContext.FAQs.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "FAQ updated successfully";
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



            return RedirectToPage("/admin/faq/index");



            #endregion
        }

    }
}
