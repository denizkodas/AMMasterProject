using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Stripe;

namespace AMMasterProject.Pages.Listing.create
{
    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class itemotherfieldsModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        private readonly WebsettingHelper _websettinghelper;

        public Guid productguid { get; set; }


        public ProductOtherPropertiesViewModel itemotherproperties { get; set; }
        public List<ProductOtherPropertiesViewModel> listproduct { get; set; }
        public itemotherfieldsModel(MyDbContext context, WebsettingHelper websettinghelper)
        {
            _dbContext = context;
            _websettinghelper = websettinghelper;

            itemotherproperties = new ProductOtherPropertiesViewModel();

        }
        public void setup(Guid productguid)

        {

            var itemsJson = _websettinghelper.GetWebsettingJson("ItemOtherProperties");
            if (itemsJson != null)
            {
                listproduct = JsonConvert.DeserializeObject<List<ProductOtherPropertiesViewModel>>(itemsJson);
            }
            else
            {
                listproduct = new List<ProductOtherPropertiesViewModel>();
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

                // Retrieve existing JSON data
                var itemsJson = _websettinghelper.GetWebsettingJson("ItemOtherProperties");

                // Deserialize existing JSON data into a list
                var existingItems = string.IsNullOrEmpty(itemsJson)
                    ? new List<ProductOtherPropertiesViewModel>()
                    : JsonConvert.DeserializeObject<List<ProductOtherPropertiesViewModel>>(itemsJson);

                // Create new item
                var newItem = new ProductOtherPropertiesViewModel
                {
                    ID = existingItems.Any() ? existingItems.Max(x => x.ID) + 1 : 1, // Ensure unique ID
                    LabelName = itemotherproperties.LabelName,
                    
                };

                // Add new item to the list
                existingItems.Add(newItem);

                // Serialize the combined list back to JSON
                string combinedJson = JsonConvert.SerializeObject(existingItems);

                // Update the database with the combined JSON
                var updateResult = _websettinghelper.UpdateWebsettingJsonList("ItemOtherProperties", combinedJson);

                TempData["success"] = "Updated successfully";
                return RedirectToPage("/listing/create/itemotherfields", new { ID = ID });
            }

            return Page();
        }
        public IActionResult OnPostDelete(string videoid)
        {
            if (RouteData.Values["ID"] != null)
            {
                string ID = (string)RouteData.Values["ID"];
                productguid = Guid.Parse(ID);

                ItemListing up = _dbContext.ItemListings.FirstOrDefault(u => u.ItemGuid == productguid);

                if (up != null)
                {
                    // Deserialize existing JSON data into a list
                    listproduct = JsonConvert.DeserializeObject<List<ProductOtherPropertiesViewModel>>(up.ItemOtherProperites).ToList();

                    // Find the index of the item to be deleted
                    int indexToDelete = listproduct.FindIndex(x => x.ID.ToString() == videoid);

                    if (indexToDelete >= 0)
                    {
                        // Remove the item from the list
                        listproduct.RemoveAt(indexToDelete);

                        // Serialize the updated list back to JSON
                        string updatedJson = JsonConvert.SerializeObject(listproduct);

                        // Update the database with the updated JSON
                        var updateResult = _websettinghelper.DeletedJson("ItemOtherProperties", updatedJson);

                        TempData["success"] = "Deleted successfully";
                    }
                }

                return RedirectToPage("/listing/create/itemotherfields", new { ID = productguid });
            }

            return Page();
        }
    }
}
