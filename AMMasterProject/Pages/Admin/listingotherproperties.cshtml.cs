using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AMMasterProject.Pages.Admin
{

   
    [BindProperties]
    public class listingotherpropertiesModel : PageModel
    {
        private readonly MyDbContext _dbContext;
        private readonly WebsettingHelper _websettinghelper;

      


        public ProductOtherPropertiesViewModel itemotherproperties { get; set; }
        public List<ProductOtherPropertiesViewModel> listproduct { get; set; }
        public listingotherpropertiesModel(MyDbContext context, WebsettingHelper websettinghelper)
        {
            _dbContext = context;
            _websettinghelper = websettinghelper;

            itemotherproperties = new ProductOtherPropertiesViewModel();

        }
        public void setup()

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
           


                setup();
            



        }

        public IActionResult OnPost()
        {
            
                
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

          

            return RedirectToPage("/admin/listingotherproperties");
        }
        public IActionResult OnPostDelete(string videoid)
        {
            // Retrieve existing JSON data
            var itemsJson = _websettinghelper.GetWebsettingJson("ItemOtherProperties");


            // Deserialize existing JSON data into a list
            listproduct = JsonConvert.DeserializeObject<List<ProductOtherPropertiesViewModel>>(itemsJson).ToList();

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

           
            return RedirectToPage("/admin/listingotherproperties");
            

       
        }
    }
}
