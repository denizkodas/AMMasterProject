using Amazon.S3.Model.Internal.MarshallTransformations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Listing.create
{

    [Authorize]
    [BindProperties]
    public class taxModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;


        public Guid productguid { get; set; }

        public  int loginid { get;set;}
        public List<SetupTaxClass> SetupTaxClassList { get; set; }

        public SetupTaxClass SetupTaxClass { get; set; }

        public List<ProductTaxV2> ProductTaxV2List { get; set; }

        #endregion

        #region DI






        public taxModel(MyDbContext context)
        {
            _dbContext = context;
           
        }

        #endregion

        #region DataPopulate

        public void setup(Guid productguid, int loginid)
        {


          


            SetupTaxClassList= _dbContext.SetupTaxClasses.Where(u=>u.ProfileId == loginid || u.ProfileId ==null && u.IsPublished==true && u.IsAdminApproved==true).ToList();

            ProductTaxV2List= _dbContext.ProductTaxV2s.Where(u=>u.ProductGuid==productguid).ToList();


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

            setup(productguid, loginid);



        }

        public IActionResult OnPost()
        {

            #region ID

          
            string ID = (string)RouteData.Values["ID"];
              productguid = Guid.Parse(ID.ToString());

              loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }
            #endregion

            #region Custom-Assignment

            SetupTaxClass.InsertDate = DateTime.Now;
            SetupTaxClass.ProfileId = loginid;
            SetupTaxClass.IsPublished = true;
            SetupTaxClass.IsAdminApproved = true;
            SetupTaxClass.Type = "User";


            #endregion

            #region Insert

            if (ModelState.IsValid) 
            {
                _dbContext.SetupTaxClasses.Add(SetupTaxClass);
                _dbContext.SaveChanges();

                
                setup(productguid, loginid);
                return Page();
            }

            else
            {

                ModelState.AddModelError("", "There are some errors in the form. Please check and try again.");

                setup(productguid, loginid);
                return Page();
            }

            #endregion


            
        }
    }
}
