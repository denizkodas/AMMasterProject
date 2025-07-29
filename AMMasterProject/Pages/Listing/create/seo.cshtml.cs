using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;


namespace AMMasterProject.Pages.Listing.create
{

    [Authorize]
    [BindProperties]
    public class seoModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;
        public Guid productguid { get; set; }

        public ProductBasicViewModel product { get; set; }

        #endregion

        #region DI






        public seoModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;

            product = new ProductBasicViewModel();

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

            else
            {

                var json = _productHelper.ItemJsonList(items.ItemMetaData);


                product.ItemId = items.ItemId;
                product.ItemGuid = Guid.Parse(items.ItemGuid.ToString());
                product.SellingType = json.ProductBasicMetaData.SellingTypeID;
                product.ListingType = json.ProductBasicMetaData.ListingTypeID;
                product.Name = json.ProductBasicMetaData.Name;
                product.Unit = json.ProductBasicMetaData.Unit;
                product.BrandName = json.ProductBasicMetaData.Brand;
                product.ShortDescription = json.ProductBasicMetaData.ShortDescription;
                product.CurrencyId = json.ProductBasicMetaData.CurrencyId;
                product.Price = json.ProductBasicMetaData.Price;
                product.ProductImage = json.ProductBasicMetaData.Image;
                product.CategoryArray = json.ProductBasicMetaData.ProductCategoryArray;
                product.SeoMetaTitle = json.ProductBasicMetaData.SeoMetaTitle;
                product.SeoKeywords = json.ProductBasicMetaData.SeoKeywords;
                product.SeoMetadescription = json.ProductBasicMetaData.SeoMetadescription;

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

        public IActionResult OnPost()
        {

            string ID = (string)RouteData.Values["ID"];

            productguid = Guid.Parse(ID);
            try
            {



                #region ID
                int loginid = 0;
                if (User.Identity.IsAuthenticated)
                {
                    loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                    // continue with loginid variable
                }



                #endregion
                #region ModelValidation
                if (product.SeoMetaTitle == null)
                {
                    ModelState.AddModelError("product.product", "SEO Title is required");
                    setup(productguid);
                    return Page();
                }

                if (product.SeoKeywords == null)
                {
                    ModelState.AddModelError("product.SeoKeywords", "SEO keyword is required ");
                    setup(productguid);
                    return Page();
                }
                if (product.SeoMetadescription == null)
                {
                    ModelState.AddModelError("product.SeoMetadescription", "Seo Description is required ");
                    setup(productguid);
                    return Page();
                }

                #endregion

                #region JsonCreator
                //json creator
                //string productdetail = _productHelper.ProductBasicmetadata(product.SellingType, product.ListingType, product.Name, product.ShortDescription, product.CurrencyId, product.Price, product.ProductImage, product.Unit, product.BrandName, product.SeoMetaTitle, product.SeoKeywords, product.SeoMetadescription);






                var json = new ItemJsonList();

                if (productdetail != null)
                {
                    json.ProductBasicMetaData = JsonConvert.DeserializeObject<ProductBasicMetaData>(productdetail);
                }


                #endregion





                ItemListing update = _dbContext.ItemListings.FirstOrDefault(u => u.ItemId == product.ItemId);
                if (update != null)
                {

                    update.ItemMetaData = JsonConvert.SerializeObject(json);
                    _dbContext.Update(update);
                    _dbContext.SaveChanges();



                    TempData["success"] = "SEO updated successfully";


                    return RedirectToPage("/listing/create/related", new { ID = ID });

                }


                setup(productguid);
                return Page();

            }
            catch (Exception ex)
            {
                TempData["success"] = ex.Message;
                setup(productguid);
                return Page();
            }


        }
    }
}
