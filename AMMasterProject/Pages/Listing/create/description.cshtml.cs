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
    public class descriptionModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;

        public Guid productguid { get; set; }
        public ProductDetailViewModel product { get; set; }

        public List<ProductAmenitiesQuestionV2> productquestionlist { get; set; }
        public List<ProductAmenitiesOptionsV2> productoptionlist { get; set; }
        public List<ProductAmenitiesMetaData> productamenitiesselected { get; set; }

        #endregion

        #region DI






        public descriptionModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
             _productHelper= productHelper;

            product = new ProductDetailViewModel();
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

            else if (items.ItemDetailMetaData != null)
            {

                //var json = ParseMetaDataProductDetail(items.ItemDetailMetaData);
                ProductDetailMetaData json = JsonConvert.DeserializeObject<ProductDetailMetaData>(items.ItemDetailMetaData);
                ///assign the value
                if (json != null)
                {
                    product.DetailDescription = WebUtility.HtmlDecode(json.DetailDescription);

                   

                }
            }
            List<ProductAmenitiesMetaData> existingMetadata = JsonConvert.DeserializeObject<List<ProductAmenitiesMetaData>>(items.AmenitiesMetaData ?? "[]");
            productamenitiesselected = existingMetadata.ToList();

            productquestionlist = _dbContext.ProductAmenitiesQuestionV2s.Where(u => u.IsPublish == true && u.Type== "Amenities").OrderBy(u => u.SortNumber).ToList();

            productoptionlist = _dbContext.ProductAmenitiesOptionsV2s.Where(u => u.IsPublish == true).OrderBy(u=>u.ProductAmenitiesName).ToList();

            

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
                string json = _productHelper.ProductDetailmetadata(product.DetailDescription,  product.Sku, product.Eancode);

                #endregion



                ItemListing items = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

                if (items != null)
                {
                    items.ItemDetailMetaData = json;
                    _dbContext.Update(items);
                    _dbContext.SaveChanges();
                }

                TempData["success"] = "Updated successfully";
                return RedirectToPage("/listing/create/digital", new { ID = ID });
            }

           
            return Page();
        }
    }
}
