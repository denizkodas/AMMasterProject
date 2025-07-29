using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Stripe;
using System.Collections.Generic;

namespace AMMasterProject.Pages.Listing.create
{

    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class imagesModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;

        public Guid productguid { get; set; }


        public List<ImageItemMetaData> listproduct { get; set; }
        public ProductImagesViewModel product { get; set; }
        #endregion

        #region DI






        public imagesModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;
            product = new ProductImagesViewModel();

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

             
                listproduct = _productHelper.ParseMetaDataProductImages(items.ItemImagesMetaData).ToList();



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
            string ID = (string)RouteData.Values["ID"];

            productguid = Guid.Parse(ID);
            try
            {
               

                #region ID
                //int loginid = 0;
                Guid ProfileGUID = Guid.NewGuid();
                if (User.Identity.IsAuthenticated)
                {

                    ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value ?? "0");
                }
                #endregion

                #region ModelValidation
                if (product.Image == null)
                {
                    ModelState.AddModelError("product.Image", "Image is required.");

                    setup(productguid);
                    return Page();
                }

                #endregion


                #region Up-sert


                ItemListing up = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

                if (up != null)
                {


                    up.ItemImagesMetaData = _productHelper.ProductImagesmetadata(product.Image, up.ItemImagesMetaData) ;

                   


                    _dbContext.Update(up);
                    _dbContext.SaveChanges();

                    //TempData["success"] = "Contacts Updated successfully";
                    //setup();
                    //return Page();



                }

                TempData["success"] = "Updated successfully";
                return RedirectToPage("/listing/create/images", new { ID = up.ItemGuid });

                
                #endregion

            }
            catch (Exception ex)
            {

                TempData["success"] = ex.Message;
                setup(productguid);
                return Page();
            }
        }



        public IActionResult OnPostDelete(string imageid)
        {
            Guid ProfileGUID = Guid.NewGuid();
            if (User.Identity.IsAuthenticated)
            {
                ProfileGUID = Guid.Parse(User.FindFirst("UserGUID")?.Value ?? "0");
            }


            string ID = (string)RouteData.Values["ID"];

            productguid = Guid.Parse(ID);

            ItemListing up = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

            if (up != null)
            {

                listproduct = _productHelper.ParseMetaDataProductImages(up.ItemImagesMetaData).ToList();



                //var parsedData = _userHelper.ParseMetaDataContactList(up.SecondaryContactMetaData);

                // Find the index of the item to be deleted
                int indexToDelete = listproduct.FindIndex(x => x.ID.ToString() == imageid);

                if (indexToDelete >= 0)
                {
                    // Remove the item from the list
                    listproduct.RemoveAt(indexToDelete);

                    // Serialize the updated list back to JSON
                    string updatedJson = JsonConvert.SerializeObject(listproduct);

                    // Update the SecondaryContactMetaData property with the updated JSON
                    up.ItemImagesMetaData = updatedJson;

                    _dbContext.Update(up);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Deleted successfully";
                }
            }

          
            return RedirectToPage("/listing/create/images", new { ID = up.ItemGuid });
        }

    }
}
