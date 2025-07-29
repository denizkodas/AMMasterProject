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
    public class videoModel : PageModel
    {

       
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;

        public Guid productguid { get; set; }
        public ProductVideoViewModel product { get; set; }
        public List<ProductVideoMetaData> listproduct { get; set; }
        public videoModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;

            product = new ProductVideoViewModel();
        }
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


                listproduct = _productHelper.ParseMetaDataProductVideo(items.VideoItemMetaData).ToList();



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

        public IActionResult OnPost()
        {

            if (RouteData.Values["ID"] != null)
            {
                string ID = (string)RouteData.Values["ID"];
                Guid productguid = Guid.Parse(ID.ToString());

                ItemListing items = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);


                #region Json
                string json = _productHelper.videometadata(0, product.Provider, product.Source, product.Poster,items.VideoItemMetaData);

                #endregion



              
                if (items != null)
                {
                    items.VideoItemMetaData = json;
                    _dbContext.Update(items);
                    _dbContext.SaveChanges();
                }

                TempData["success"] = "Updated successfully";
                return RedirectToPage("/listing/create/Video", new { ID = ID });
            }


            return Page();
        }

        public IActionResult OnPostDelete(string videoid)
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

                listproduct = _productHelper.ParseMetaDataProductVideo(up.VideoItemMetaData).ToList();



                //var parsedData = _userHelper.ParseMetaDataContactList(up.SecondaryContactMetaData);

                // Find the index of the item to be deleted
                int indexToDelete = listproduct.FindIndex(x => x.ID.ToString() == videoid);

                if (indexToDelete >= 0)
                {
                    // Remove the item from the list
                    listproduct.RemoveAt(indexToDelete);

                    // Serialize the updated list back to JSON
                    string updatedJson = JsonConvert.SerializeObject(listproduct);

                    // Update the SecondaryContactMetaData property with the updated JSON
                    up.VideoItemMetaData = updatedJson;

                    _dbContext.Update(up);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Deleted successfully";
                }
            }


            return RedirectToPage("/listing/create/video", new { ID = up.ItemGuid });
        }
    }
}
