using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Listing.create
{

    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class crossModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;

        public Guid productguid { get; set; }
        public List<ProductViewModel> productlist { get; set; }

        public List<ProductRelatedMetaData> relatedProductslist { get; set; }
        private readonly ProductHelper _productHelper;


        #endregion

        #region DI






        public crossModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;

        }

        #endregion

        #region DataPopulate

        public void setup(Guid productguid)
        {


            productlist = _productHelper.productmasterdataV2(0, "cross",200,1).ToList();
            //relatedProductslist = _dbContext.ProductRelatedProducts.Where(u => u.Type == "CP" && u.ProductGuid==productguid).ToList();

            ItemListing itemListing = _dbContext.ItemListings.FirstOrDefault(u=>u.ItemGuid ==productguid );

            relatedProductslist = ProductHelper.ParseMetaDataProductRelated(itemListing.RelatedItemMetaData).Where(u => u.RelatedType == 1).ToList();


            int loginid = 0;
            string usertype = "";
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                usertype = User.FindFirst("UserType")?.Value ?? "Client";
                // continue with loginid variable
            }


            if (usertype == "Vendor")
            {
                productlist = productlist.Where(u => u.ProfileId == loginid).ToList();
            }




        }



        #endregion
        public void OnGet()
        {

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }
            string ID = (string)RouteData.Values["ID"];

             productguid = Guid.Parse(ID);

             setup(productguid);



        }

      
    }
}
