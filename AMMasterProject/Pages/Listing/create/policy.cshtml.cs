using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Listing.create
{
    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class policyModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;

        public Guid productguid { get; set; }
        public ProductPolicyViewModel product { get; set; }



        #endregion

        #region DI






        public policyModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;
            product = new ProductPolicyViewModel();
        }

        #endregion

        #region DataPopulate

        public void setup(Guid productguid)
        {




            ItemListing items = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);
            if (items == null)
            {
                TempData["success"] = "Listing does not exist. You can create new listing.";
                RedirectToPage("/listing/create/basic");
            }

            else if(items.ItemPolicyMetaData!=null)
            {

               //var json = _productHelper.ParseMetaDataProductPolicy(items.ItemPolicyMetaData);

                
                ProductPolicyMetaData json = JsonConvert.DeserializeObject<ProductPolicyMetaData>(items.ItemPolicyMetaData);
                ///assign the value
                if (json != null)
                {
                    product.ReturnPolicy = json.ReturnPolicy;
                    product.CancellationPolicy = json.CancellationPolicy;
                }

            }







        }



        #endregion
        public void OnGet()
        {
            if (RouteData.Values["ID"] != null)
            {
                string ID = (string)RouteData.Values["ID"];

                productguid = Guid.Parse(ID);





                setup(productguid);
            }

            else
            {
                TempData["success"] = "Something went wrong. Try again later";
                RedirectToPage("/listing/create/basic");
            }



        }

        public IActionResult OnPost()
        {
            if (RouteData.Values["ID"] != null)
            {
                string ID = (string)RouteData.Values["ID"];
                Guid productguid = Guid.Parse(ID.ToString());

                #region Json
                string json = _productHelper.ProductPolicymetadata(product.ReturnPolicy, product.CancellationPolicy);

                #endregion



                ItemListing items = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

                if (items != null)
                {
                    items.ItemPolicyMetaData = json;
                    _dbContext.Update(items);
                    _dbContext.SaveChanges();
                }

                TempData["success"] = "Updated successfully";

                return RedirectToPage("/listing/create/shipping", new { ID = ID });
            }
            return Page();

        }
    }
}
