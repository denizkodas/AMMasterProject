using AMMasterProject.Helpers;
using AMMasterProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AMMasterProject.Pages.orders
{

    [Authorize(Policy = "AllUsers")]
    public class IndexModel : PageModel
    {
        public List<OrderViewModel> orderViewModelsList { get; set; }
        private readonly OrderHelper _orderhelper;

        public IndexModel(OrderHelper orderhelper)
        {



            _orderhelper = orderhelper;
        }
        public void OnGet()
        {



            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {

                
                    loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                    
                    // continue with loginid variable

                    orderViewModelsList = _orderhelper.GetOrdersItem().Where(u => u.OrderStatus == "confirm" && u.BuyerID == loginid && u.ItemType == "item" ).OrderByDescending(u => u.OrderDateDT).ToList();
               
            }
        }
    }
}
