using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;

namespace AMMasterProject.Pages.Listing.create
{

    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class discountModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;

        public Guid productguid { get; set; }
        public ProductDetailViewModel product { get; set; }

        public DiscountSellerAlwaysMetaData discountselleralways { get; set; }
        public DiscountSellerCustomMetaData discountsellercustom { get; set; }

        public List<DiscountSellerCustomMetaData> discountsellercustomlist { get; set; }

        #endregion

        #region DI






        public discountModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;

            product = new ProductDetailViewModel();

            discountselleralways = new DiscountSellerAlwaysMetaData();
            discountselleralways.IsAlways = false;

            discountsellercustomlist = new List<DiscountSellerCustomMetaData>();
        }

        #endregion

        public void setup(Guid productguid)
        {


            ItemListing items = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);
            if (items == null)
            {
                TempData["success"] = "Listing does not exist. You can create new listing.";
                RedirectToPage("/listing/create/basic");
            }

            else 
            
            
            
            
            
            
            if (items.SellerDiscountMetaData != null)
            {


                discountselleralways = ProductHelper.ParseMetaDataDiscountSellerMetaData(items.SellerDiscountMetaData);

                discountsellercustomlist = ProductHelper.ParseMetaDataDiscountCustomMetaData(items.SellerDiscountMetaData);



                //var json = ParseMetaDataProductDetail(items.ItemDetailMetaData);

            }
          


        }
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

        public IActionResult OnPostAlways()
        {
            if (RouteData.Values["ID"] != null)
            {
                string ID = (string)RouteData.Values["ID"];
                Guid productguid = Guid.Parse(ID.ToString());

                ItemListing items = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

                #region DiscountAlways
                string discountAlways = _productHelper.ProductSellerDiscountAlwaysmetadata(discountselleralways.IsAlways, discountselleralways.DiscountType, discountselleralways.DiscountAmount);
                #endregion

                #region CustomDiscount

                //discountsellercustomlist = ProductHelper.ParseMetaDataDiscountCustomMetaData(items?.SellerDiscountMetaData);

                string CustomDiscount = _productHelper.ProductSellerDiscountCustommetadata("add",string.Empty, discountsellercustom.DiscountStartDate, discountsellercustom.DiscountEndDate, discountsellercustom.DiscountType, discountsellercustom.DiscountAmount, true, items.SellerDiscountMetaData);
                #endregion

                var json = new ProductSellerDiscountMetaData();

                if (discountAlways != null)
                {
                    json.DiscountSellerAlwaysMetaData = JsonConvert.DeserializeObject<DiscountSellerAlwaysMetaData>(discountAlways);
                   
                }
                if (!string.IsNullOrEmpty(CustomDiscount))
                {
                    json.DiscountSellerCustomMetaData = JsonConvert.DeserializeObject<List<DiscountSellerCustomMetaData>>(CustomDiscount);
                }

                if (items != null)
                {
                    items.SellerDiscountMetaData = JsonConvert.SerializeObject(json);
                    _dbContext.Update(items);
                    _dbContext.SaveChanges();
                }

                TempData["success"] = "Updated successfully";

             

                return RedirectToPage("/listing/create/discount", new { ID = productguid });
                //return Page();
            }

            return Page();
        }

        public IActionResult OnPostCustom()
        {
            if (RouteData.Values["ID"] != null)
            {
                string ID = (string)RouteData.Values["ID"];
                Guid productguid = Guid.Parse(ID.ToString());

                ItemListing items = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

                #region DiscountAlways
                string discountAlways = _productHelper.ProductSellerDiscountAlwaysmetadata(discountselleralways.IsAlways, discountselleralways.DiscountType, discountselleralways.DiscountAmount);
                #endregion

                #region CustomDiscount

                //discountsellercustomlist = ProductHelper.ParseMetaDataDiscountCustomMetaData(items?.SellerDiscountMetaData);

                string CustomDiscount = _productHelper.ProductSellerDiscountCustommetadata("add", string.Empty, discountsellercustom.DiscountStartDate, discountsellercustom.DiscountEndDate, discountsellercustom.DiscountType, discountsellercustom.DiscountAmount, true, items.SellerDiscountMetaData);
                #endregion

                var json = new ProductSellerDiscountMetaData();

                if (discountAlways != null)
                {
                    json.DiscountSellerAlwaysMetaData = JsonConvert.DeserializeObject<DiscountSellerAlwaysMetaData>(discountAlways);

                }
                if (!string.IsNullOrEmpty(CustomDiscount))
                {
                    json.DiscountSellerCustomMetaData = JsonConvert.DeserializeObject<List<DiscountSellerCustomMetaData>>(CustomDiscount);
                }

                if (items != null)
                {
                    items.SellerDiscountMetaData = JsonConvert.SerializeObject(json);
                    _dbContext.Update(items);
                    _dbContext.SaveChanges();
                }

                TempData["success"] = "Updated successfully";
                return RedirectToPage("/listing/create/discount", new { ID = productguid });
            }

            return Page();
        }
    }
}
