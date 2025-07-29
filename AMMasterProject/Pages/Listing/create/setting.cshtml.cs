using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Listing.create
{

    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class settingModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;
        public Guid productguid { get; set; }

        public ProductBasicV2 ProductBasicV2 { get; set; }



        #endregion

        #region DI






        public settingModel(MyDbContext context)
        {
            _dbContext = context;
            ProductBasicV2 = new();
        }

        #endregion

        #region DataPopulate

        //public void setup(Guid productguid)
        //{


        //    ProductBasicV2 = _dbContext.ProductBasicV2s.FirstOrDefault(u => u.ProductGuid == productguid);







        //}



        #endregion
        //public void OnGet()
        //{

        //    int loginid = 0;
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
        //        // continue with loginid variable
        //    }
        //    string ID = (string)RouteData.Values["ID"];

        //    productguid = Guid.Parse(ID);

        //    setup(productguid);



        //}

        //public IActionResult OnPost()
        //{
        //    string ID = (string)RouteData.Values["ID"];
        //    Guid productguid = Guid.Parse(ID.ToString());

        //    ProductBasicV2 pb = _dbContext.ProductBasicV2s.FirstOrDefault(u => u.ProductGuid == productguid);

        //    if (pb != null)
        //    {
        //        pb.Producttags = ProductBasicV2.Producttags;
              
        //        _dbContext.Update(pb);
        //        _dbContext.SaveChanges();
        //    }


        //    return RedirectToPage("/reports/listinglist");
        //}
    }
}
