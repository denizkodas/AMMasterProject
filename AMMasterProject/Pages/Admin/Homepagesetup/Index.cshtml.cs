using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayPal.Api;

namespace AMMasterProject.Pages.Admin.Homepagesetup
{

    [Authorize(Policy = "Setup")]
    [BindProperties]
    public class IndexModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;
        private readonly ProductHelper _productHelper;

        public List<ItemPageDesign> websitesetupproductsetting { get; set; }
        //public  List<ItemHomePageDesignViewModel> websitesetupproductsetting { get; set; }


        #endregion

        #region DI

        public IndexModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;


        }

        #endregion

        #region DataPopulate    

        public void setup()
        {

            websitesetupproductsetting = _dbContext.ItemPageDesign.ToList();



        }
        #endregion
        public void OnGet()
        {

            setup();

        }





        public IActionResult OnPostDelete(int websitesetuppageid)
        {
            ItemPageDesign websiteSetupProductSetting = _dbContext.ItemPageDesign.FirstOrDefault(u => u.ItemPageDesignID == websitesetuppageid);

            if (websiteSetupProductSetting != null)
            {

                _dbContext.ItemPageDesign.Remove(websiteSetupProductSetting);
                _dbContext.SaveChanges();


                TempData["info"] = "Deleted successfully";

               
               

            }

            return RedirectToPage("/admin/Homepagesetup/index");
        }

    }
}
