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
    public class shippingModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;
        public Guid productguid { get; set; }

        public ProductShippingViewModel product { get; set; }


        #endregion

        #region DI






        public shippingModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;
            product = new ProductShippingViewModel();
           
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

            else if(items.ItemShippingMetaData != null) 
            {

              


                ProductShippingMetaData json = JsonConvert.DeserializeObject<ProductShippingMetaData>(items.ItemShippingMetaData);
                ///assign the value
                if (json != null)
                {
                    product.IsFreeShipping = json.IsFreeShipping;
                    product.ShippingWeight = json.ShippingWeight;
                    product.ShippingWidth = json.ShippingWidth;
                    product.ShippingHeight = json.ShippingHeight;
                    product.ShippingLength = json.ShippingLength;
                    product.ShippingAddOnCharges = json.ShippingAddOnCharges;
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


                if(product.IsFreeShipping ==false && product.ShippingAddOnCharges <0)
                {
                    ModelState.AddModelError("product.ShippingAddOnCharges", "Must be greater than 0");
                    setup(productguid);
                    return Page();
                }



                #region Json
                string json = _productHelper.ProductShippingmetadata(product.IsFreeShipping, product.ShippingWeight, product.ShippingLength, product.ShippingWidth, product.ShippingHeight, product.ShippingAddOnCharges);

                #endregion



                ItemListing items = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

                if (items != null)
                {
                    items.ItemShippingMetaData = json;
                    _dbContext.Update(items);
                    _dbContext.SaveChanges();
                }

                TempData["success"] = "Updated successfully";
                return RedirectToPage("/listing/create/attribute", new { ID = ID });
            }
            return Page();

           
        }
    }
}
