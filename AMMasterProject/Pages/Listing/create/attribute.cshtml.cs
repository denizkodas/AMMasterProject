using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace AMMasterProject.Pages.Listing.create
{


    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class attributeModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;

        public Guid productguid { get; set; }
        
        public List<ProductAttributeQuestionV2> productAttributeQuestionList { get; set; }
       
        public ProductAttributeQuestionV2 productAttributeQuestion { get; set; }

       
     
        public int loginid { get; set; }

        #endregion

        #region DI






        public attributeModel(MyDbContext context)
        {
            _dbContext = context;
            productAttributeQuestion = new ProductAttributeQuestionV2();
            productAttributeQuestion.IsPublish = true;

        }

        #endregion

        #region DataPopulat

        public void setup(Guid productguid)

        {


            productAttributeQuestionList = _dbContext.ProductAttributeQuestionV2s.Where(u=>u.ProductGuid == productguid).OrderBy(u=>u.SortNumber).ToList();



           

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
            string ID = (string)RouteData.Values["ID"];

             productguid = Guid.Parse(ID);

             setup(productguid);

            string questionGuid = Request.Query["QuestionGUID"];

            if (questionGuid != null)
            {
                Guid QID = Guid.Parse(questionGuid.ToString());

                productAttributeQuestion = _dbContext.ProductAttributeQuestionV2s.FirstOrDefault(u=>u.ProductAttributeGuid== QID);

            }


        }

       
        




        
    }
}
