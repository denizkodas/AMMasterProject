using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AMMasterProject;
using AMMasterProject.ViewModel;
using AMMasterProject.Helpers;
using System.Linq;
using System.Net.NetworkInformation;
using AMMasterProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject.Pages
{
    public class IndexModel : PageModel
    {

        #region DI

      

        //private readonly MyDbContext _dbContext;
      
        //private readonly ProductHelper _producthelper;

        //public List<HomePageFirstLevel> productmodel { get; set; }

        //public  HomePageDesignModelV2 productmodel { get; set; }
        public IndexModel()
        {
            
        }
        #endregion

        

        public async void OnGet()
        {


          

            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }

            //List<HomePageFirstLevel> model = _producthelper.GetItemDesignData();

            //productmodel = _producthelper.itemsettingv2();
            //var productData = _producthelper.GetProductDataByItemDesignId(itemDesignData);

            


            //var q  = _producthelper.itemsetting();


            // productmodel = q;

            //productmodel = new HomePageListingModel
            //{
            //    ItemSettingList = _producthelper.itemsetting(), //itemseetinglist,
            //    productviewModel = _producthelper.productmasterdataV2(loginid, "home", 30, 1, 0, "")

            //};

        }

        
    }
}