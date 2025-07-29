using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;

namespace AMMasterProject.Pages.Listing.create
{

    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class inventoryModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;

        public Guid productguid { get; set; }
        public ProductInventoryViewModel product { get; set; }



        #endregion

        #region DI






        public inventoryModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;

            product = new ProductInventoryViewModel();
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

            else if(items.InventoryItemMetaData != null)
            {

              
                ProductInventoryMetaData json = JsonConvert.DeserializeObject<ProductInventoryMetaData>(items.InventoryItemMetaData);
                ///assign the value
                if (json != null)
                {
                    product.SKU = json.SKU;
                    product.EANCode = json.EANCode;
                    product.MINQTY = json.MINQTY;
                    product.MAXQTY= json.MAXQTY;
                    product.IsManagedInventory = json.IsManagedInventory;
                    product.IsOutOfStock = json.IsOutOfStock;

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
                string json = _productHelper.ProductInventorymetadata(product.EANCode, product.SKU, product.MINQTY, product.MAXQTY, product.IsManagedInventory, product.IsOutOfStock);

                #endregion



                ItemListing items = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

                if (items != null)
                {
                    items.InventoryItemMetaData = json;
                    _dbContext.Update(items);
                    _dbContext.SaveChanges();
                }

                TempData["success"] = "Updated successfully";
                return Page();
                //return RedirectToPage("/listing/create/images", new { ID = ID });
            }


            return Page();
        }
    }
}
