using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.products
{
    [Authorize(Policy = "ListingPolicy")]
    [BindProperties]
    public class ListinglistModel : PageModel
    {
        #region Models
        private readonly ProductHelper _productHelper;
        public List<ItemJsonList> productlist { get; set; }
        #endregion
        #region DI






        public ListinglistModel(ProductHelper productHelper)
        {

            _productHelper = productHelper;
        }

        #endregion

      
        public void OnGet()
        {

           

            productlist = _productHelper.ItemJsonList("vendorwise",300, 1, 0,"", 0).ToList();

            int loginid = 0;
            string usertype = "";
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                usertype = User.FindFirst("UserType")?.Value ?? "Client";
                // continue with loginid variable
            }

           
            if (usertype == "Vendor")
            {
                productlist = productlist.Where(u => u.ProfileID == loginid).ToList();
            }

        }
    }
}
