using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.Reports
{

    [Authorize(Policy = "SellerAdmin")]
    public class OrderlistModel : PageModel
    {
        #region DI
        public List<OrderViewModel> orderViewModelsList { get; set; }
        private readonly OrderHelper _orderhelper;

        public OrderlistModel(OrderHelper orderhelper)
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

                orderViewModelsList = _orderhelper.GetOrdersItem().Where(u => u.OrderStatus == "confirm"  && u.ItemType == "item").OrderByDescending(u => u.OrderDateDT).ToList();



                if (usertype == "Vendor")
                {
                    orderViewModelsList = orderViewModelsList.Where(u => u.SellerID == loginid).ToList();
                }
            }
        }
    }
}
