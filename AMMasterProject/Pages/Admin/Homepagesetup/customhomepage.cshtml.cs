using AMMasterProject.Helpers;
using AMMasterProject.Models;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;

namespace AMMasterProject.Pages.Admin.Homepagesetup
{

    [Authorize(Policy = "Admin")]
    [BindProperties]
    public class customhomepageModel : PageModel
    {
        #region Models
        private readonly MyDbContext _dbContext;

        public int homeDesignID { get; set; }
        public List<ProductViewModel> productlist { get; set; }

        public List<ItemPageDesignChild> itempagedesignchild { get; set; }
        private readonly ProductHelper _productHelper;


       
        #endregion

        #region DI






        public customhomepageModel(MyDbContext context, ProductHelper productHelper)
        {
            _dbContext = context;
            _productHelper = productHelper;

        }

        #endregion

        #region DataPopulate

        public void setup(int id)
        {


            productlist = _productHelper.productmasterdataV2(0, "homepagedesign", 0, 0).ToList();
            itempagedesignchild = _dbContext.ItemPageDesignChild.Where(u => u.ItemPageDesignID == id).ToList();

          
         



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

            homeDesignID = int.Parse(ID);

            setup(homeDesignID);



        }
    }
}
