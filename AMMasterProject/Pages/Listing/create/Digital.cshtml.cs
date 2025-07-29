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
    public class DigitalModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;

        public bool IsDigital { get; set; } 
        public Guid productguid { get; set; }


        public List<ProductDigitalMetaData> listproduct { get; set; }
        public ProductDigitalMetaData product { get; set; }
        #endregion

        #region DI






        public DigitalModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;
            product = new ProductDigitalMetaData();

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
                if(items.ItemMetaData!=null)
                {
                    var json = _productHelper.ItemJsonList(items.ItemMetaData);

                  
                    if(json.ProductBasicMetaData.ListingTypeID ==1)
                    {
                        IsDigital = true;
                    }

                    else
                    {
                        IsDigital = false;
                    }
                }
             
                //listproduct = ProductHelper.ParseMetaDataProductDigital(items.ItemDigitalMetaData).ToList();
                if(items.ItemDigitalMetaData != null) { 
                   listproduct = JsonConvert.DeserializeObject<List<ProductDigitalMetaData>>(items.ItemDigitalMetaData);

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
                if (product.DigitalLink == null)
                {
                    ModelState.AddModelError("product.DigitalLink", "Digital File is required.");

                    setup(productguid);
                    return Page();
                }

                #endregion


                #region Up-sert


                ItemListing up = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

                if (up != null)
                {


                    up.ItemDigitalMetaData = _productHelper.ProductDigitalmetadata(product.DigitalLink, up.ItemDigitalMetaData);
                    up.IsPublish = true;
                   


                    _dbContext.Update(up);
                    _dbContext.SaveChanges();

                    //TempData["success"] = "Contacts Updated successfully";
                    //setup();
                    //return Page();



                }

                TempData["success"] = "Updated successfully";
                return RedirectToPage("/listing/create/digital", new { ID = up.ItemGuid });

                
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


                List<ProductDigitalMetaData> listproduct = JsonConvert.DeserializeObject<List<ProductDigitalMetaData>>(up.ItemDigitalMetaData);


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
                    up.ItemDigitalMetaData = updatedJson;

                    if(indexToDelete==0)
                    {
                          //if this is last record to be delete so make this unpublish
                        up.IsPublish = false;
                    }

                    _dbContext.Update(up);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Deleted successfully";
                }
              
            }

          
            return RedirectToPage("/listing/create/digital", new { ID = up.ItemGuid });
        }

    }
}
