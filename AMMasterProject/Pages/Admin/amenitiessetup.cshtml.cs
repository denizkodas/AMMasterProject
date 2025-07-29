using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Admin
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class amenitiessetupModel : PageModel
    {

       
            #region Models
            private readonly MyDbContext _dbContext;

            public Guid productguid { get; set; }

            public List<ProductAmenitiesQuestionV2> productAmenitiesQuestionList { get; set; }

            public ProductAmenitiesQuestionV2 productAmenitiesQuestion { get; set; }



            public int loginid { get; set; }

            #endregion

            #region DI






            public amenitiessetupModel(MyDbContext context)
            {
                _dbContext = context;
               productAmenitiesQuestion = new ProductAmenitiesQuestionV2();
               productAmenitiesQuestion.IsPublish = true;

            }

            #endregion

            #region DataPopulat

            public void setup()

            {


            productAmenitiesQuestionList = _dbContext.ProductAmenitiesQuestionV2s.OrderBy(u => u.SortNumber).ToList();





            }



            #endregion



            public void OnGet()
            {

                loginid = 0;
                if (User.Identity.IsAuthenticated)
                {
                    loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                    // continue with loginid variable
                }
               

                setup();

                string questionGuid = Request.Query["QuestionGUID"];

                if (questionGuid != null)
                {
                    int QID = int.Parse(questionGuid.ToString());

                productAmenitiesQuestion = _dbContext.ProductAmenitiesQuestionV2s.FirstOrDefault(u => u.ProductAmenitiesId == QID);

                }


            }








        
    }
}
