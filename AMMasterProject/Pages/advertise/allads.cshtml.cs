using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.advertise
{

    [Authorize(Policy = "SellerAdmin")]
    [BindProperties]
    public class alladsModel : PageModel
    {
        #region Model
       
        public List<AdvertiseBoostViewModel> advertiseviewmodel;

      
        private readonly OrderHelper _orderhelper;

        public alladsModel(OrderHelper orderhelper)
        {



            _orderhelper = orderhelper;
        }
        #endregion





        public void OnGet()
        {


            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {


                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                string usertype = User.FindFirst("UserType")?.Value ?? "0";
                // continue with loginid variable

                advertiseviewmodel = _orderhelper.AdvertiseBoostList().OrderByDescending(u => u.PurchaseDate).ToList();



                if (usertype == "Vendor")
                {
                    advertiseviewmodel = advertiseviewmodel.Where(u => u.BuyerId == loginid).OrderByDescending(u => u.PurchaseDate).ToList();
                }
            }
        }
    }
}
